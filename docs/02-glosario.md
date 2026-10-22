# Lenguaje ubicuo

## 1. Tarifario

| Término | Significado acordado | Qué NO es | Regla o invariante asociada | Evento |
|---|---|---|---|---|
| **Tarifa** | Lista de precios establecida por Nodo para sus recursos y servicios. | No es necesariamente el precio final de un cliente concreto. | Solo Dirección puede establecer o modificar tarifas. | Tarifa actualizada |
| **Tarifa base** | Valor de referencia para un recurso y una franja antes de los ajustes comerciales. | No es el importe final tras descuentos o condiciones particulares. | Dirección establece la tarifa base como referencia para los ajustes. | Tarifa base establecida |
| **Temporada** | Período del calendario en que Dirección puede modificar tarifas de ciertos servicios. | No es hora pico (condición horaria). | Dirección puede ajustar por temporada. Combinación con hora pico: **pendiente**. | Tarifa de temporada actualizada |
| **Hora pico** | Franja horaria de mayor demanda con tarifa diferente. | No es una temporada ni un descuento del miembro. | Dirección define las condiciones para horarios pico. Cómo se identifican: **pendiente**. | Tarifa de horario pico actualizada |

## 2. Cálculo de precio total

| Término | Significado acordado | Qué NO es | Regla o invariante asociada | Evento |
|---|---|---|---|---|
| **Precio** | Importe calculado para una operación concreta con las tarifas y condiciones aplicables al cliente. | No es necesariamente la tarifa base. | Una reserva confirmada conserva el precio acordado aunque cambien las tarifas. | Precio calculado |
| **Franja** | Bloque horario para el que se solicita o usa una sala y que puede generar un importe de alquiler. | No es el margen de limpieza entre reservas. | Cancelación con menos de 4 horas: se cobra la franja completa. | Franja cobrada *(candidato)* |
| **Horas incluidas** | Horas de sala contempladas como beneficio de un plan en el período. | No son horas extra ni un beneficio ilimitado. | Se consideran antes de cobrar horas adicionales. Acumulación entre períodos: **pendiente**. | Horas incluidas consumidas *(candidato)* |
| **Hora extra** | Hora de uso de sala que excede las incluidas en el plan. | No es una hora cubierta por el beneficio mensual. | Se cobran a fin de mes; plan de equipo recibe descuento. | Hora extra registrada *(candidato)* |
| **Descuento del plan** | Reducción del importe de alquiler de salas por ciertos planes. | No es una promoción general ni un descuento negociado individualmente. | Depende del plan. Combinación con temporada/hora pico: **pendiente**. | Descuento aplicado *(candidato)* |
| **Aforo** | Capacidad máxima de personas en un espacio para eventos. | No es el precio del evento ni la cantidad de servicios extra. | Influye en el precio del evento; fórmula **pendiente**. | Precio de evento calculado *(candidato)* |
| **Servicio extra** | Servicio adicional para un evento (catering, audiovisual) que Nodo puede subcontratar. | No forma necesariamente parte del alquiler básico. | Se considera al determinar el precio del evento. | Servicio extra incorporado a la propuesta *(candidato)* |

## 3. Cotizaciones

| Término | Significado acordado | Qué NO es | Regla o invariante asociada | Evento |
|---|---|---|---|---|
| **Cotización** (sinónimo de negocio: *presupuesto*) | Propuesta económica a un cliente con importe estimado y vigencia. | No es una reserva confirmada ni garantiza ocupación. | Vigencia limitada, no reserva espacios; conserva el precio mientras esté vigente. | Cotización emitida / aceptada / caducada |
| **Vigencia de cotización** | Período en que la propuesta puede aceptarse bajo las condiciones acordadas. | No es la fecha del evento ni la duración de una reserva. | Vencido el plazo sin aceptación, caduca y se necesita una nueva propuesta. | Cotización caducada |

## 4. Polisemias y ambigüedades documentadas

| Palabra / par | Sentido A | Sentido B | Decisión |
|---|---|---|---|
| **Tarifa vs precio** | Tarifa: referencia comercial de Nodo (Tarifario). | Precio: importe de una operación concreta (Cálculo). | Dos términos distintos, cada uno en su contexto. |
| **Cotización vs reserva** | Cotización: propuesta económica. | Reserva confirmada: se cumplieron las condiciones para contratar el espacio. | Nunca se usan como sinónimos. |
| **Cotización vs presupuesto** | Sinónimos en el negocio. | — | Término preferente: **cotización**. |
| **Horas incluidas vs horas extra** | Beneficio del plan. | Uso que excede el beneficio. | Dos términos; las incluidas se consumen primero. |
| **Franja** *(hipótesis)* | En Cálculo: bloque que genera importe. | En Reservas y disponibilidad: bloque de ocupación de un recurso. | **Diferencia de uso por discutir en clase** |
| **Precio** *(hipótesis)* | En Cálculo: resultado de un cálculo. | En Cotizaciones: importe ofrecido y conservado durante la vigencia. | En Cotizaciones es un dato recibido del cálculo, no se recalcula. |

## 5. Relación del glosario con las fronteras

- **Tarifario** concentra tarifa, tarifa base, temporada y hora pico: administración de condiciones por Dirección.
- **Cálculo de precio total** agrupa precio, franja, horas incluidas/extra, descuento, aforo y servicio extra: determinar importes concretos.
- **Cotizaciones** mantiene cotización y vigencia: propuestas, aceptación y caducidad.

## 6. Prueba de lenguaje con ejemplos y casos límite

Los ejemplos no fijan importes ni fórmulas nuevas. Permiten comprobar el significado de cada término y descubrir reglas pendientes.

| Contexto | Término | ¿Qué pasa si…? | Consecuencia o pregunta |
|---|---|---|---|
| Tarifario | Tarifa | Recepción intenta modificarla | No debe aplicarse el cambio: INV-01. |
| Tarifario | Tarifa base | El cliente tiene descuento | La referencia no equivale al precio final; la base del descuento sigue pendiente: H-03. |
| Tarifario | Temporada | Coincide con hora pico | Combinación y prioridad pendientes: H-01. |
| Tarifario | Hora pico | Una reserva atraviesa dos franjas | Falta precisar el corte y la tarifa aplicable: H-06. |
| Cálculo | Precio | Cambia la tarifa después de confirmar | El precio acordado de la reserva se conserva: INV-02. |
| Cálculo | Franja | Se cancela dos horas antes | Corresponde cobrar la franja completa: R-06. No es el margen de limpieza. |
| Cálculo | Horas incluidas | Quedan horas al terminar el período | Su acumulación no está definida: H-02. |
| Cálculo | Hora extra | El uso iguala exactamente las horas disponibles | No hay excedente. Si las supera, solo el excedente es adicional: INV-06. |
| Cálculo | Descuento del plan | El miembro de equipo consume horas extra | Se considera su descuento: INV-07; el posible 5 % adicional sigue abierto: H-05. |
| Cálculo | Aforo | Cambia el espacio del evento | Debe revisarse el dato de capacidad y su impacto; la fórmula no está acordada: H-07. |
| Cálculo | Servicio extra | Se agrega catering a una propuesta emitida | Afecta el cálculo del servicio; cómo revisar una cotización vigente está pendiente: H-09. |
| Cotizaciones | Cotización | El cliente la acepta y el salón está ocupado | Aceptación y reserva son hechos distintos: INV-03 y H-04. |
| Cotizaciones | Vigencia de cotización | Se acepta justo al vencer el plazo | Instante de corte y referencia horaria pendientes: H-08. |

Los eventos del glosario son vocabulario de descubrimiento. «Horas incluidas consumidas» corresponde al hecho de consumo; su relación con los otros contextos se discutirá en clase; una consulta de precio no consume beneficios ni genera facturación. «Descuento aplicado» y «Precio calculado» siguen siendo candidatos, no eventos en código.

Ver [EventStorming](03-event-storming.md) y [reglas e invariantes](06-reglas-invariantes.md) para la trazabilidad de los códigos.
