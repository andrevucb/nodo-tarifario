# Cálculo de precio total

Módulo `Nodo.CalculoPrecioTotal` · Bounded context **Cálculo de precio total**.

## 1. Qué modela

El importe de una operación concreta según recurso, franja, condiciones tarifarias, plan, horas disponibles, aforo y servicios extra, según corresponda.

## 2. Lenguaje propio

Precio, franja, horas incluidas, hora extra, descuento del plan, aforo, servicio extra; Precio calculado es un evento candidato. Ver el [glosario por contexto](../../../docs/02-glosario.md).

## 3. Reglas y límites

INV-06, INV-07 y R-02 a R-04; R-06 aporta el importe por cancelación. No define los beneficios del plan ni registra consumo por una consulta. Ver el [inventario de invariantes](../../../docs/06-reglas-invariantes.md).

En el código actual se implementan INV-06, INV-07 y R-02. R-03 se recibe en la solicitud, pero todavía no se aplica al cálculo.

## 4. Colaboraciones

Recibe referencias de Tarifario y datos de Membresías, Espacios, Reservas y Eventos. Entrega importes a Reservas, Eventos y Cotizaciones; los de consumo real, a Facturación. No emite facturas ni modifica tarifas. Ver el [context map](../../../docs/05-context-map.md).

La colaboración con Cotizaciones es directa: `ServicioCotizacion` llama a `ServicioCalculoPrecio` y usa el `DesglosePrecio` resultante. No se intercambia un evento.

## 5. Estado de implementación

- **Proyecto:** `src/Modules/CalculoPrecioTotal/Nodo.CalculoPrecioTotal`.
- **Dominio:** `ServicioCalculoPrecio`, VOs `Horas`, `Descuento` y `DesglosePrecio`, y contratos de consulta en `Domain/Contracts`.
- **Sin agregado, entidades, repositorio, DbContext ni migraciones:** calcular un precio no persiste estado.
- **Adaptadores de infraestructura:** `InMemoryMembresiasQuery` e `InMemoryEspaciosQuery` están registrados por el módulo. `InMemoryTarifasQuery` se eliminó porque Tarifario ya aporta la implementación real.
- **Integración con Tarifario:** `ITarifasQuery` es un contrato publicado en `Nodo.Tarifario.Contracts`; lo implementa `EfTarifasQuery` en `Nodo.Tarifario.Infrastructure/Queries` y se registra desde `AddTarifarioModule`.
- **Presentación:** página `/Calculo` en `src/Web/Nodo.Web/Pages/Calculo`; llama directamente al servicio de dominio.
- **Eventos:** no emite eventos de dominio.
- **Pruebas:** no hay un proyecto de pruebas automatizadas; las pruebas de INV-06 e INV-07 siguen pendientes.

## 6. Modelo táctico

### 6.1 Elemento central — Domain Service

El módulo **no tiene Aggregate Root ni Entities**. Su elemento central es el servicio de dominio `ServicioCalculoPrecio`, que combina datos de Tarifario, Membresías y Espacios para producir un resultado inmutable.

| Método | Entrada | Resultado |
|---|---|---|
| `CalcularPrecioAsync(SolicitudCalculo solicitud, CancellationToken ct)` | Recurso, horas, fecha y hora de inicio, cliente —y opcionalmente aforo y servicios extra—. | `DesglosePrecio`; no persiste ni emite eventos. |

`SolicitudCalculo` es un `record` con `RecursoId`, `Horas`, `FechaHoraInicio`, `ClienteId`, `Aforo` y `ServiciosExtra`. En el flujo actual, `Aforo` y `ServiciosExtra` llegan en `null`.

### 6.2 Entidades

No hay entidades propias. `RecursoInfo`, `PlanInfo` y `TarifaAplicable` son datos de lectura que provienen de otros contextos; no tienen identidad ni ciclo de vida dentro de este módulo.

### 6.3 Value Objects

| VO | Datos | Regla / uso |
|---|---|---|
| `Horas` | `Cantidad` (`int`) | Exige valores no negativos. Está definido, pero el servicio todavía usa `int` directamente. |
| `Descuento` | `Porcentaje` (`decimal`) | Exige un valor entre 0 y 100. Está definido, pero el servicio usa `decimal` directamente. |
| `DesglosePrecio` | Horas solicitadas, cubiertas y adicionales; importes; descuento; precio total; `Facturable`. | Resultado inmutable del cálculo. |
| `Money` | `Amount`, `Currency` | VO compartido en `Nodo.SharedKernel`; representa los importes y su moneda. |

### 6.4 Contratos de integración

| Contrato | Datos que entrega | Implementación actual |
|---|---|---|
| `ITarifasQuery` | `TarifaAplicable`: tarifa y tarifario, tipo de recurso, hora pico, franja, temporada y `TarifaBase`. | Contrato en `Nodo.Tarifario.Contracts`; `EfTarifasQuery` en `Nodo.Tarifario.Infrastructure/Queries`, registrado por `AddTarifarioModule`. |
| `IMembresiasQuery` | `PlanInfo`: código de plan, horas incluidas y porcentaje de descuento. | `InMemoryMembresiasQuery`. |
| `IEspaciosQuery` | `RecursoInfo`: identificador, tipo de recurso y capacidad. | `InMemoryEspaciosQuery`. |

No hay repositorio: el módulo no guarda precios ni consumos.

## 7. Servicios de aplicación

No existe un Application Service propio. La página `/Calculo` construye la `SolicitudCalculo` y llama directamente a `ServicioCalculoPrecio`.

| Componente actual | Tipo | Responsabilidad |
|---|---|---|
| `Pages/Calculo/IndexModel` | Presentación | Recibir el formulario y llamar al servicio de dominio. |
| `ServicioCalculoPrecio` | Domain Service | Aplicar las reglas de cálculo y devolver `DesglosePrecio`. |

La llamada directa es aceptable mientras la operación sea una consulta sin efectos de estado. Si se expone una API o se agregan consumos facturables, hará falta un servicio de aplicación que coordine la consulta y las políticas correspondientes.

## 8. Flujos

### 8.1 Calcular precio

1. La página recibe `recursoId`, `horas`, `fechaHoraInicio` y `clienteId`.
2. `ServicioCalculoPrecio` obtiene el recurso con `IEspaciosQuery`; si no existe, lanza `DomainException`.
3. Si hay `clienteId`, obtiene su `PlanInfo` con `IMembresiasQuery`; si no hay cliente o plan, se calcula sin horas incluidas ni descuento.
4. Obtiene la tarifa aplicable con `ITarifasQuery`, usando el tipo de recurso y la fecha y hora de inicio; si no hay tarifa, lanza `DomainException`.
5. Calcula las horas cubiertas y adicionales.
6. Calcula el importe bruto de las horas adicionales y aplica el descuento del plan.
7. Devuelve el `DesglosePrecio` con el detalle y la marca `Facturable`.

Fórmulas actuales:

```text
horasCubiertas   = min(horasSolicitadas, horasIncluidas)
horasAdicionales = max(0, horasSolicitadas - horasIncluidas)
importeBruto     = tarifaBase × horasAdicionales
importeNeto      = importeBruto × (1 - descuento / 100)
precioTotal      = importeNeto
facturable       = horasAdicionales > 0
```

Las horas cubiertas por el plan se registran con importe cero.

Ejemplo: 6 horas solicitadas, 4 incluidas, tarifa de 50 BOB/h y descuento del 10 % producen 2 horas adicionales, 100 BOB brutos, 90 BOB netos y un total de 90 BOB.

### 8.2 Selección de la tarifa aplicable

La implementación de `EfTarifasQuery`:

1. Carga todos los tarifarios con sus tarifas.
2. Filtra por tipo de recurso.
3. Filtra por franja horaria con `Franja.Cubre(hora)`.
4. Filtra por temporada: acepta tarifas sin temporada o cuya temporada cubra la fecha.
5. Ordena primero las tarifas con temporada y devuelve la primera.

No hay una regla explícita para combinar temporada y hora pico cuando ambas aplican; la implementación actual prioriza la tarifa de temporada.

### 8.3 Resultados y rechazos

| Situación | Resultado |
|---|---|
| Recurso inexistente | `DomainException`. |
| Sin tarifa aplicable para el tipo, la franja o la temporada | `DomainException`. |
| Cliente sin plan | Sin horas incluidas ni descuento. |
| Horas solicitadas dentro de las incluidas | Sin horas adicionales; total 0; `Facturable = false`. |
| Horas solicitadas por encima de las incluidas | Se cobran las adicionales y se aplica el descuento del plan. |

## 9. Eventos de dominio

Este módulo **no emite eventos de dominio**. `DesglosePrecio` es un resultado consultado, no un evento.

| Evento candidato | Emisor previsto | Estado |
|---|---|---|
| `PrecioCalculado` | `ServicioCalculoPrecio` | No implementado. El resultado se devuelve de forma síncrona como `DesglosePrecio`. |

Tampoco se publican eventos de integración hacia Facturación, Reservas o Agenda de eventos. La política P-02 y las relaciones L-05, L-07 y L-09 quedan pendientes.

## 10. Brechas y pendientes

- R-03 no está implementado: `Aforo` y `ServiciosExtra` se aceptan en la solicitud, pero el servicio los ignora.
- `Horas` y `Descuento` existen como VOs, pero el servicio usa `int` y `decimal`; por eso no valida horas negativas.
- El descuento se aplica a cualquier plan, no solo al plan de equipo, y `PlanInfo.PlanCodigo` no se utiliza.
- `ITarifasQuery` se publica desde `Nodo.Tarifario.Contracts` y se registra en `AddTarifarioModule`; Cálculo necesita que el módulo Tarifario esté registrado en el host. No hay una dependencia directa entre los proyectos Cálculo y Tarifario.
- No hay Application Service; la presentación llama directamente al servicio de dominio.
- No existe una regla para combinar temporada y hora pico (decisión pendiente 1 del modelo táctico).
- La base del descuento y el posible 5 % adicional siguen pendientes (decisión pendiente 2); hoy se aplica sobre el importe bruto de las horas adicionales.
- Las horas incluidas no utilizadas no se acumulan al siguiente período (decisión pendiente 3).
- No se emiten eventos ni se comunican consumos a Facturación; P-02 y L-09 no están implementados.
- No hay pruebas automatizadas para INV-06 e INV-07.
- No se define una política de redondeo ni de impuestos.

## 11. Referencias

- [Modelo táctico de Tarifas y precios](../../../modelo-tactico-tarifas-y-precios.md)
- [Glosario por contexto](../../../docs/02-glosario.md)
- [Reglas e invariantes](../../../docs/06-reglas-invariantes.md)
- [Context map](../../../docs/05-context-map.md)
