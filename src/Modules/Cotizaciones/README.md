# Cotizaciones

Módulo `Nodo.Cotizaciones` · Bounded context **Cotizaciones**.

## 1. Qué modela

Las propuestas económicas emitidas a clientes, su importe ofrecido, vigencia y caminos de aceptación o caducidad.

## 2. Lenguaje propio

Cotización como término preferente de presupuesto, vigencia, Cotización emitida, Cotización aceptada y Cotización caducada. Ver el [glosario por contexto](../../../docs/02-glosario.md).

## 3. Reglas y límites

INV-03, INV-04 e INV-05; P-03 orienta la continuidad tras el vencimiento. Emitir o aceptar no reserva automáticamente. Ver el [inventario de invariantes](../../../docs/06-reglas-invariantes.md).

El agregado conserva el precio ofrecido y comprueba la vigencia al aceptar, aunque la caducidad todavía no se haya registrado.

## 4. Colaboraciones

Recibe el importe de Cálculo (L-02) llamando a `ServicioCalculoPrecio` y comparte propuestas y aceptación con Reservas y Eventos (L-06 y L-08). No calcula beneficios ni confirma ocupación; la emisión operativa sigue pendiente en H-10. Ver el [context map](../../../docs/05-context-map.md).

En el prototipo, la aceptación solo cambia el estado de la cotización: todavía no se comunica a Reservas ni a Agenda de eventos.

## 5. Estado de implementación

- **Proyecto:** `src/Modules/Cotizaciones/Nodo.Cotizaciones`; referencia a `Nodo.CalculoPrecioTotal` y a `Nodo.SharedKernel`.
- **Dominio:** agregado `Cotizacion`, enum `EstadoCotizacion`, VO `Vigencia`, contrato `IRepositorioCotizacion` y eventos `CotizacionEmitida`, `CotizacionAceptada` y `CotizacionCaducada`.
- **Aplicación:** `ServicioCotizacion`.
- **Infraestructura:** EF Core sobre SQLite, esquema `cotizaciones`, `CotizacionesDbContext`, `EfRepositorioCotizacion` y `CotizacionConfiguration`.
- **Migraciones:** `20261027042058_Initial`.
- **Datos de demostración:** `CotizacionesSeed` crea una cotización vigente, una aceptada, una caducada y una vencida sin registrar.
- **Presentación:** página `/Cotizaciones` en `src/Web/Nodo.Web/Pages/Cotizaciones`.
- **Eventos:** manejadores de toast en `Nodo.Web` para los tres eventos.
- **Pruebas:** no hay un proyecto de pruebas automatizadas; las pruebas de INV-03, INV-04 e INV-05 siguen pendientes.

## 6. Modelo táctico

### 6.1 Aggregate Root — `Cotizacion`

| Atributo | Tipo | Notas |
|---|---|---|
| `Id` | `Guid` | Identificador de la cotización (`cotizacionId`). |
| `ClienteId` | `Guid` | Referencia externa por identificador; no se incorpora el modelo de Cliente. |
| `RecursoId` | `Guid` | Referencia externa por identificador; no se incorpora el modelo de Recurso. |
| `PrecioOfrecido` | `Money` | Inmutable; no se recalcula si cambia el tarifario. |
| `Vigencia` | `Vigencia` | Fecha de emisión y vencimiento. |
| `Estado` | `EstadoCotizacion` | `Emitida`, `Aceptada` o `Caducada`. |

| Método | Comportamiento | Regla / evento |
|---|---|---|
| `Emitir(cotizacionId, clienteId, recursoId, precio, fechaEmision)` | Crea la cotización con estado `Emitida` y una vigencia de 4 días. Valida cliente, recurso y precio. | **INV-03** · `CotizacionEmitida` |
| `Aceptar(DateTime ahora)` | Exige estado `Emitida` y comprueba la vigencia con la fecha actual. Si es válida, cambia a `Aceptada` y conserva el precio. | **INV-04**, **INV-05** · `CotizacionAceptada` |
| `RegistrarCaducidad(DateTime ahora)` | Si el estado no es `Emitida`, no hace nada. Si la vigencia sigue activa, rechaza. Si ya venció, cambia a `Caducada`. | `CotizacionCaducada` |

`Emitir` es un método estático de creación. No reserva el recurso: el agregado no contiene ocupación ni disponibilidad (INV-03).

### 6.2 Entidades

No hay entidades internas. `Cotizacion` es la única entidad y también la raíz del agregado. `EstadoCotizacion` es un enum, no una entidad.

### 6.3 Value Objects

| VO | Datos | Responsabilidad |
|---|---|---|
| `Money` | `Amount`, `Currency` | VO compartido en `Nodo.SharedKernel`; representa el precio ofrecido con su moneda. |
| `Vigencia` | `Emision`, `Vencimiento` (`DateTime`) | Exige que el vencimiento no sea anterior a la emisión. `Crear(emision, dias)` genera la vigencia; `EstaVigente(ahora)` comprueba el plazo. |

`Vigencia.EstaVigente` usa `ahora <= Vencimiento`; el instante de vencimiento todavía se considera vigente.

### 6.4 Repositorio

| Componente | Tipo | Responsabilidad |
|---|---|---|
| `IRepositorioCotizacion` | Repository (contrato de dominio) | `ObtenerPorIdAsync(cotizacionId, ct)` y `GuardarAsync(cotizacion, ct)`. |
| `EfRepositorioCotizacion` | Implementación de infraestructura | Recupera y persiste la cotización con EF Core. |

`CotizacionConfiguration` mapea `Estado` como texto, `PrecioOfrecido` y `Vigencia` como tipos propios, e ignora `DomainEvents`.

## 7. Servicio de aplicación

`ServicioCotizacion` coordina el ciclo de vida de la propuesta:

| Método | Responsabilidad |
|---|---|
| `EmitirAsync(clienteId, recursoId, horas, fechaHoraInicio, clienteIdParaPlan, ct)` | Solicita el precio a Cálculo, crea la cotización, la guarda y despacha `CotizacionEmitida`. |
| `AceptarAsync(cotizacionId, ct)` | Recupera la cotización, invoca `Aceptar` con la hora actual, guarda y despacha `CotizacionAceptada`. |
| `RegistrarCaducidadAsync(cotizacionId, ct)` | Recupera la cotización, invoca `RegistrarCaducidad`, guarda y despacha los eventos que correspondan. |

`EmitirAsync` construye la solicitud de cálculo con el recurso, las horas, la fecha y hora de inicio y el cliente que define el plan. El precio calculado se usa una sola vez al emitir; después queda almacenado en `PrecioOfrecido`.

No hay métodos de consulta en el servicio: la página `/Cotizaciones` lee las cotizaciones directamente con `CotizacionesDbContext`.

## 8. Flujos

### 8.1 Emitir cotización

1. La página recibe `clienteId`, `recursoId`, `horas` y `fechaHoraInicio`.
2. `ServicioCotizacion.EmitirAsync` solicita el cálculo a `ServicioCalculoPrecio`, usando el mismo cliente para identificar su plan.
3. Si el cálculo falla, la excepción se propaga y no se crea la cotización.
4. `Cotizacion.Emitir` valida las referencias y el precio, fija una vigencia de 4 días y el estado `Emitida`.
5. El agregado emite `CotizacionEmitida`.
6. El servicio guarda la cotización con `IRepositorioCotizacion`.
7. El servicio despacha el evento y limpia la lista de eventos del agregado.
8. La página recarga la lista y muestra la cotización emitida.

La operación no reserva el recurso (INV-03). El precio queda fijado en la cotización y no se recalcula si el tarifario cambia (INV-04).

### 8.2 Aceptar cotización

1. La página recibe `cotizacionId`.
2. El servicio recupera la cotización; si no existe, lanza `DomainException`.
3. Invoca `Cotizacion.Aceptar(DateTime.UtcNow)`.
4. El agregado rechaza la operación si el estado no es `Emitida` o si la vigencia ya terminó (INV-05).
5. Si es válida, cambia el estado a `Aceptada`, conserva `PrecioOfrecido` y emite `CotizacionAceptada`.
6. El servicio guarda, despacha el evento y devuelve la cotización.

La aceptación no confirma una reserva ni un evento: eso corresponde a Reservas y disponibilidad y a Agenda de eventos.

### 8.3 Registrar caducidad

1. La página recibe `cotizacionId`.
2. El servicio recupera la cotización; si no existe, lanza `DomainException`.
3. Invoca `Cotizacion.RegistrarCaducidad(DateTime.UtcNow)`.
4. Si el estado no es `Emitida`, el método no hace nada y no emite eventos.
5. Si la vigencia sigue activa, lanza `DomainException`.
6. Si ya venció, cambia a `Caducada` y emite `CotizacionCaducada`.
7. El servicio guarda y despacha los eventos que existan.

| Situación | Resultado |
|---|---|
| Cotización inexistente | `DomainException`. |
| Aceptar una cotización no emitida | `DomainException`. |
| Aceptar una cotización vencida | `DomainException`; aunque figure como `Emitida`. |
| Aceptación válida | Estado `Aceptada`, precio conservado y evento. |
| Caducidad de una cotización vigente | `DomainException`. |
| Caducidad válida | Estado `Caducada` y evento. |
| Caducidad de una cotización ya aceptada o caducada | Sin cambio y sin evento; la página lo informa como si se hubiera registrado. |

## 9. Eventos de dominio

| Evento | Disparador | Campos principales | Consumidor actual |
|---|---|---|---|
| `CotizacionEmitida` | `Cotizacion.Emitir` | `CotizacionId`, `ClienteId`, `RecursoId`, `PrecioOfrecido`, `Emision`, `Vencimiento`. | `CotizacionEmitidaToastHandler`. |
| `CotizacionAceptada` | `Cotizacion.Aceptar` | `CotizacionId`, `ClienteId`, `RecursoId`, `PrecioOfrecido`, `AceptadaEn`. | `CotizacionAceptadaToastHandler`. |
| `CotizacionCaducada` | `Cotizacion.RegistrarCaducidad` | `CotizacionId`, `CaducadaEn`. | `CotizacionCaducadaToastHandler`. |

Los tres eventos implementan `IDomainEvent` y se despachan en proceso después de guardar. No hay eventos de integración ni outbox.

Los consumidores previstos para `CotizacionAceptada` son Reservas y disponibilidad y Agenda de eventos, para continuar el proceso comercial. Esa integración no está implementada.

## 10. Brechas y pendientes

- No hay servicio de aplicación de consulta; la página lista las cotizaciones directamente con `CotizacionesDbContext`.
- No hay integración con Reservas ni Agenda de eventos: la aceptación no comprueba disponibilidad ni confirma una reserva (L-06 y L-08 pendientes).
- La decisión pendiente 5 del modelo táctico sigue abierta: qué ocurre si se acepta una cotización vigente pero el recurso ya no está disponible.
- No hay un proceso automático de caducidad; `RegistrarCaducidad` debe invocarse explícitamente.
- `RegistrarCaducidad` no lanza excepción cuando la cotización ya no está `Emitida`; solo no hace nada. La interfaz, sin embargo, muestra “Caducidad registrada”.
- El límite exacto de vigencia sigue pendiente (H-08); la implementación considera vigente el instante igual a `Vencimiento`.
- Los eventos se despachan después de guardar y no son transaccionales; no existe patrón outbox.
- P-03 no está automatizado: no se genera ni se solicita una nueva cotización al caducar.
- `Money` es compartido en `Nodo.SharedKernel`, no una copia propia del módulo.
- No hay pruebas automatizadas para INV-03, INV-04 e INV-05.
- No se modelan Cliente ni Recurso; solo se guardan sus identificadores.

## 11. Referencias

- [Modelo táctico de Tarifas y precios](../../../modelo-tactico-tarifas-y-precios.md)
- [Glosario por contexto](../../../docs/02-glosario.md)
- [Reglas e invariantes](../../../docs/06-reglas-invariantes.md)
- [Context map](../../../docs/05-context-map.md)
