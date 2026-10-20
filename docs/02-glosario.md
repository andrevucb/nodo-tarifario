# Lenguaje ubicuo

## Tarifario

| Término | Significado acordado | Qué NO es? | Regla o invariante asociada | Evento |
|---|---|---|---|---|
| **Tarifa** | Lista de precios establecida por Nodo para sus recursos y servicios. | No es necesariamente el precio final de un cliente concreto. | Solo Dirección puede establecer o modificar tarifas. | Tarifa actualizada |
| **Tarifa base** | Valor de referencia para un recurso y una franja antes de los ajustes comerciales. | No es el importe final tras descuentos o condiciones particulares. | Dirección establece la tarifa base como referencia para los ajustes. | Tarifa base establecida |
| **Temporada** | Período del calendario en que Dirección puede modificar tarifas de ciertos servicios. | No es hora pico (condición horaria). | Dirección puede ajustar por temporada. Combinación con hora pico: **pendiente**. | Tarifa de temporada actualizada |
| **Hora pico** | Franja horaria de mayor demanda con tarifa diferente. | No es una temporada ni un descuento del miembro. | Dirección define las condiciones para horarios pico. Cómo se identifican: **pendiente**. | Tarifa de horario pico actualizada |

## Cálculo de precio total

| Término | Significado acordado | Qué NO es? | Regla o invariante asociada | Evento |
|---|---|---|---|---|
| **Precio** | Importe calculado para una operación concreta con las tarifas y condiciones aplicables al cliente. | No es necesariamente la tarifa base. | Una reserva confirmada conserva el precio acordado aunque cambien las tarifas. | Precio calculado |
| **Franja** | Bloque horario para el que se solicita o usa una sala y que puede generar un importe de alquiler. | No es el margen de limpieza entre reservas. | Cancelación con menos de 4 horas: se cobra la franja completa. | Franja cobrada *(candidato)* |
| **Horas incluidas** | Horas de sala contempladas como beneficio de un plan en el período. | No son horas extra ni un beneficio ilimitado. | Se consideran antes de cobrar horas adicionales. Acumulación entre períodos: **pendiente**. | Horas incluidas consumidas *(candidato)* |
| **Hora extra** | Hora de uso de sala que excede las incluidas en el plan. | No es una hora cubierta por el beneficio mensual. | Se cobran a fin de mes; plan de equipo recibe descuento. | Hora extra registrada *(candidato)* |
| **Descuento del plan** | Reducción del importe de alquiler de salas por ciertos planes. | No es una promoción general ni un descuento negociado individualmente. | Depende del plan. Combinación con temporada/hora pico: **pendiente**. | Descuento aplicado *(candidato)* |
| **Aforo** | Capacidad máxima de personas en un espacio para eventos. | No es el precio del evento ni la cantidad de servicios extra. | Influye en el precio del evento; fórmula **pendiente**. | Precio de evento calculado *(candidato)* |
| **Servicio extra** | Servicio adicional para un evento (catering, audiovisual) que Nodo puede subcontratar. | No forma necesariamente parte del alquiler básico. | Se considera al determinar el precio del evento. | Servicio extra incorporado a la propuesta *(candidato)* |

## Cotizaciones

| Término | Significado acordado | Qué NO es? | Regla o invariante asociada | Evento |
|---|---|---|---|---|
| **Cotización** (sinónimo de negocio: *presupuesto*) | Propuesta económica a un cliente con importe estimado y vigencia. | No es una reserva confirmada ni garantiza ocupación. | Vigencia limitada, no reserva espacios; conserva el precio mientras esté vigente. | Cotización emitida / aceptada / caducada |
| **Vigencia de cotización** | Período en que la propuesta puede aceptarse bajo las condiciones acordadas. | No es la fecha del evento ni la duración de una reserva. | Vencido el plazo sin aceptación, caduca y se necesita una nueva propuesta. | Cotización caducada |

## Polisemias y ambigüedades documentadas

| Palabra / par | Sentido A | Sentido B | Decisión |
|---|---|---|---|
| **Tarifa vs precio** | Tarifa: referencia comercial de Nodo (Tarifario). | Precio: importe de una operación concreta (Cálculo). | Dos términos distintos, cada uno en su contexto. |
| **Cotización vs reserva** | Cotización: propuesta económica. | Reserva confirmada: se cumplieron las condiciones para contratar el espacio. | Nunca se usan como sinónimos. |
| **Cotización vs presupuesto** | Sinónimos en el negocio. | — | Término preferente: **cotización**. |
| **Horas incluidas vs horas extra** | Beneficio del plan. | Uso que excede el beneficio. | Dos términos; las incluidas se consumen primero. |
| **Franja** *(hipótesis)* | En Cálculo: bloque que genera importe. | En Reservas y disponibilidad: bloque de ocupación de un recurso. | **Require validacion por el experto** |
| **Precio** *(hipótesis)* | En Cálculo: resultado de un cálculo. | En Cotizaciones: importe ofrecido y conservado durante la vigencia. | En Cotizaciones es un dato recibido del cálculo, no se recalcula. |

## Relación del glosario con las fronteras

- **Tarifario** concentra tarifa, tarifa base, temporada y hora pico: administración de condiciones por Dirección.
- **Cálculo de precio total** agrupa precio, franja, horas incluidas/extra, descuento, aforo y servicio extra: determinar importes concretos.
- **Cotizaciones** mantiene cotización y vigencia: propuestas, aceptación y caducidad.