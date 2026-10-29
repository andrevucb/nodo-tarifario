# Tarifario

Módulo `Nodo.Tarifario` · Bounded context **Tarifario**.

## 1. Qué modela

Las referencias tarifarias por recurso y las condiciones de temporada y hora pico que administra Dirección.

## 2. Lenguaje propio

Tarifa, tarifa base, temporada, hora pico, Tarifa actualizada. Ver el [glosario por contexto](../../../docs/02-glosario.md).

## 3. Reglas y límites

INV-01 y R-01. La actualización debe respetar los compromisos INV-02 e INV-04 sin modificar reservas ni propuestas. Ver el [inventario de invariantes](../../../docs/06-reglas-invariantes.md).

La raíz del agregado es quien autoriza el cambio: si el solicitante no tiene el rol `Direccion`, la operación se rechaza sin modificar la tarifa ni emitir el evento.

## 4. Colaboraciones

Entrega referencias a Cálculo (L-01) mediante el contrato `ITarifasQuery`, publicado en `Nodo.Tarifario.Contracts` e implementado por `EfTarifasQuery` en la infraestructura del módulo, que lee `TarifarioDbContext`. Solicita avisos a Notificaciones (T-01), aunque la integración real todavía no existe: hoy el evento solo se muestra como notificación de la interfaz. No calcula el importe de cada operación ni factura. Ver el [context map](../../../docs/05-context-map.md).

## 5. Estado de implementación

- **Proyecto:** `src/Modules/Tarifario/Nodo.Tarifario`.
- **Contratos publicados:** `src/Modules/Tarifario/Nodo.Tarifario.Contracts` contiene `ITarifasQuery` y `TarifaAplicable`; Cálculo depende de este contrato, no del módulo Tarifario.
- **Dominio:** agregado `Tarifario`, entidad `Tarifa`, VOs `Franja` y `Temporada`, contrato `IRepositorioTarifario` y evento `TarifaActualizada`.
- **Aplicación:** `ServicioActualizarTarifa`.
- **Infraestructura:** EF Core sobre SQLite, esquema `tarifario`, configuración de entidades, `EfRepositorioTarifario` y `EfTarifasQuery` en `Infrastructure/Queries`.
- **Migraciones:** `20261027042048_Initial`.
- **Datos de demostración:** `TarifarioSeed` crea tarifas para `SalaReuniones`, `SalonEventos` y `EscritorioFlexible`, con variantes de horario normal, hora pico y temporada alta.
- **Presentación:** página `/Tarifario` en `src/Web/Nodo.Web/Pages/Tarifario`.
- **Eventos:** `TarifaActualizadaToastHandler` en `Nodo.Web` consume `TarifaActualizada` para mostrar el cambio.
- **Pruebas:** no hay un proyecto de pruebas automatizadas en el repositorio; las pruebas de INV-01 e INV-02 siguen pendientes.

## 6. Modelo táctico

### 6.1 Aggregate Root — `Tarifario`

| Atributo | Tipo | Notas |
|---|---|---|
| `Id` | `Guid` | Identificador del tarifario (`tarifarioId`). |
| `Tarifas` | `IReadOnlyCollection<Tarifa>` | Colección interna; solo se modifica a través de la raíz. |

| Método | Comportamiento | Regla / evento |
|---|---|---|
| `Crear(Guid tarifarioId)` | Crea la raíz. Si el identificador llega vacío, genera uno nuevo. | — |
| `AgregarTarifa(Tarifa tarifa)` | Incorpora una tarifa a la colección interna. No expone una lista modificable. | — |
| `ActualizarTarifa(Guid tarifaId, Money nuevoImporte, string solicitanteUsuarioId, string solicitanteRol)` | Comprueba el rol, localiza la tarifa, conserva el importe anterior y actualiza. | **INV-01** · `TarifaActualizada` |

Si el rol no es `Direccion` (comparación sin distinguir mayúsculas) o la tarifa no existe, se lanza `DomainException` y no hay cambio ni evento.

### 6.2 Entidades

| Entidad | Rol | Datos principales |
|---|---|---|
| `Tarifa` | Entidad interna del agregado. Mantiene su identidad aunque cambie el importe. | `TarifaId`, `TipoRecurso`, `EsHoraPico`, `TarifaBase` (`Money`), `Franja`, `Temporada?` y `ActualizarImporte(Money)`. |

`Tarifa` hereda de `Entity<Guid>`; su identidad es `TarifaId`.

### 6.3 Value Objects

| VO | Datos | Responsabilidad |
|---|---|---|
| `Money` | `Amount`, `Currency` | VO compartido en `Nodo.SharedKernel`. Valida monto no negativo y moneda obligatoria; ofrece `Add`, `Subtract` y `Multiply`. |
| `Franja` | `Inicio`, `Fin` (`TimeOnly`) | Exige `Fin > Inicio` y determina si una hora cae dentro de la franja con `Cubre(hora)`. |
| `Temporada` | `Inicio`, `Fin` (`DateOnly`) | Exige `Fin >= Inicio` y determina si una fecha pertenece a la temporada con `Cubre(fecha)`. |

### 6.4 Repositorio y contrato publicado

| Componente | Tipo | Responsabilidad |
|---|---|---|
| `IRepositorioTarifario` | Repository (contrato de dominio) | `ObtenerPorIdAsync(tarifarioId, ct)` y `GuardarAsync(tarifario, ct)`. |
| `EfRepositorioTarifario` | Implementación de infraestructura | Recupera el agregado con sus tarifas (`Include`) y persiste los cambios con EF Core. |
| `ITarifasQuery` | Contrato publicado (`Nodo.Tarifario.Contracts`) | `ObtenerTarifaAplicableAsync(tipoRecurso, fechaHora, ct)` para los consumidores de tarifas. |
| `EfTarifasQuery` | Adaptador de infraestructura (`Infrastructure/Queries`) | Lee `TarifarioDbContext`, filtra por tipo, franja y temporada, y devuelve `TarifaAplicable`. |

## 7. Servicio de aplicación

`ServicioActualizarTarifa` coordina el caso de uso:

```csharp
Task ActualizarAsync(
    Guid tarifarioId,
    Guid tarifaId,
    decimal nuevoImporte,
    string moneda,
    string solicitanteUsuarioId,
    string solicitanteRol,
    CancellationToken ct = default)
```

Responsabilidades:

1. Recuperar el agregado `Tarifario` o rechazar si no existe.
2. Construir el VO `Money` con el importe y la moneda recibidos.
3. Delegar la modificación en `Tarifario.ActualizarTarifa`.
4. Guardar el agregado mediante `IRepositorioTarifario`.
5. Despachar los eventos de dominio y limpiarlos del agregado.

## 8. Flujos

### 8.1 Actualizar tarifa

1. Dirección envía el formulario de `/Tarifario` con `tarifarioId`, `tarifaId`, `nuevoImporte` y `moneda`.
2. La página obtiene `usuarioId` y `rol` de los claims del usuario autenticado.
3. El servicio recupera el agregado; si no existe, lanza `DomainException`.
4. El servicio construye `Money`; un importe negativo o una moneda vacía se rechazan.
5. La raíz valida el rol. Si no es `Direccion`, se rechaza la operación por INV-01.
6. La raíz localiza la tarifa, guarda el importe anterior y actualiza `TarifaBase`.
7. La raíz emite `TarifaActualizada`.
8. El servicio guarda el agregado y despacha el evento.
9. `TarifaActualizadaToastHandler` muestra el cambio en la interfaz.

| Situación | Resultado |
|---|---|
| Rol distinto de `Direccion` | `DomainException`; sin cambio ni evento. |
| Tarifario inexistente | `DomainException`. |
| Tarifa inexistente | `DomainException`. |
| Importe negativo o moneda vacía | `DomainException` al construir `Money`. |
| Actualización válida | Tarifa persistida y `TarifaActualizada` despachado. |

El guardado y el despacho de eventos no comparten una transacción explícita ni un patrón outbox.

## 9. Eventos de dominio

### `TarifaActualizada`

| Campo | Tipo | Contenido |
|---|---|---|
| `TarifarioId` | `Guid` | Tarifario que contiene la tarifa. |
| `TarifaId` | `Guid` | Tarifa modificada. |
| `TipoRecurso` | `string` | Tipo de recurso de la tarifa. |
| `EsHoraPico` | `bool` | Condición de hora pico. |
| `FranjaInicio`, `FranjaFin` | `TimeOnly` | Franja de aplicación. |
| `TemporadaInicio`, `TemporadaFin` | `DateOnly?` | Temporada, si existe. |
| `ImporteAnterior`, `ImporteNuevo` | `Money` | Importes antes y después del cambio. |
| `ActualizadoPorUsuarioId` | `string` | Usuario que solicitó el cambio. |
| `ActualizadoPorRol` | `string` | Rol del solicitante. |
| `OccurredOn` | `DateTime` | Momento del cambio, en UTC. |

- **Emisor:** `Tarifario.ActualizarTarifa`.
- **Disparador:** `ServicioActualizarTarifa`.
- **Consumidor actual:** `TarifaActualizadaToastHandler` en `Nodo.Web`.
- **Consumidor previsto:** Notificaciones, para avisar a miembros con plan (P-01 / T-01).

## 10. Brechas y pendientes

- `Money` es un VO compartido en `Nodo.SharedKernel`, no una copia por módulo como suponía el modelo táctico.
- `Crear` solo recibe el identificador y `AgregarTarifa` es un método separado; el modelo táctico describía la creación con tarifas iniciales.
- No hay servicio de aplicación de consulta: la página `/Tarifario` lista los tarifarios directamente con `TarifarioDbContext`.
- La integración con Notificaciones no existe; el evento solo alimenta un toast de la interfaz.
- INV-02 se apoya en Reservas y disponibilidad, que debe conservar el precio confirmado. En este módulo no hay una comprobación de esa invariante.
- El solicitante se modela con dos cadenas (`usuarioId` y `rol`); falta un tipo de dominio para el solicitante (decisión pendiente 7 del modelo táctico).
- No se valida que no existan tarifas duplicadas al agregarlas.
- El despacho de eventos ocurre después de guardar y no es transaccional.
- No hay pruebas automatizadas para INV-01 ni para la conservación del precio de reservas.
- Sigue pendiente cómo combinar temporada y hora pico cuando coinciden (decisión pendiente 1 del modelo táctico); la selección actual está en `EfTarifasQuery`, dentro de `Infrastructure/Queries`.

## 11. Referencias

- [Modelo táctico de Tarifas y precios](../../../modelo-tactico-tarifas-y-precios.md)
- [Glosario por contexto](../../../docs/02-glosario.md)
- [Reglas e invariantes](../../../docs/06-reglas-invariantes.md)
- [Context map](../../../docs/05-context-map.md)
