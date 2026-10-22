# Reglas e invariantes candidatas

## 1. Clasificación de las reglas

Una **invariante candidata** expresa una condición que debe mantenerse siempre; una **regla comercial** determina un cálculo o una condición; una **política** describe una reacción «cuando… entonces…». Una regla comercial puede convertirse en una invariante del modelo, pero aún no se decide cómo implementarla.

## 2. Invariantes candidatas

| Código | Condición que debe garantizarse | Responsable del modelo | Ejemplo o caso límite |
|---|---|---|---|
| INV-01 | Solo Dirección puede modificar las tarifas de Nodo. | Tarifario | Un intento de Recepción no cambia tarifas ni produce Tarifa actualizada. |
| INV-02 | Una modificación tarifaria no altera el precio de una reserva previamente confirmada. | Reservas mantiene el precio; Tarifario modifica la referencia. | Un aumento posterior no recalcula la reserva confirmada. Una solicitud no confirmada no adquiere esta protección por esa sola condición. |
| INV-03 | Emitir o aceptar una cotización no reserva automáticamente un recurso. | Cotizaciones; Reservas controla ocupación. | Dos clientes pueden disponer de propuestas para una misma fecha; aceptar no garantiza el salón. |
| INV-04 | Una cotización vigente conserva el precio originalmente ofrecido aunque cambien las tarifas. | Cotizaciones | Una subida durante la vigencia no modifica el importe de esa propuesta. Cambios de alcance: H-09. |
| INV-05 | No se acepta una cotización fuera de su vigencia. | Cotizaciones | Si caducó sin aceptación, se requiere una nueva propuesta. Límite temporal exacto: H-08. |
| INV-06 | Las horas de sala que exceden las incluidas se identifican como consumo adicional facturable. | Cálculo de precio total; Membresías aporta las horas disponibles. | Si el uso iguala las horas disponibles, no hay excedente; si las supera, la porción excedente es adicional. Una consulta no registra uso. |
| INV-07 | Las horas extra del plan de equipo se calculan considerando el descuento correspondiente al miembro. | Cálculo de precio total | El descuento se considera aunque las horas estén fuera del beneficio incluido. Base de aplicación y posible 5 % adicional: H-03 y H-05. |

INV-02 es un compromiso entre contextos, no una razón para reunir Tarifario y Reservas en un mismo modelo. INV-06 requiere conocer el consumo y los beneficios disponibles; quién registra el uso y cómo coordinar solicitudes simultáneas permanece pendiente en I-02. Las invariantes no dan por resuelta esa colaboración.

## 3. Reglas comerciales adicionales

| Código | Regla | Contexto y límite |
|---|---|---|
| R-01 | Dirección puede ajustar tarifas por temporada u horario pico. | Tarifario; combinación pendiente en H-01. |
| R-02 | El precio considera tipo de recurso, franja y condiciones del plan. | Cálculo; cruces de franja pendientes en H-06. |
| R-03 | El precio de un evento considera aforo y servicios adicionales. | Cálculo recibe datos de Eventos y Espacios; fórmula pendiente en H-07. |
| R-04 | Las horas extra se incorporan a la facturación mensual. | Cálculo determina importes; Facturación administra factura y cobro. |
| R-05 | Confirmar un evento requiere depósito y, si es grande, aprobación de Dirección. | Agenda de eventos / Facturación; definir «grande» y el depósito corresponde a los vecinos. |
| R-06 | Una reserva de sala puede cancelarse sin costo hasta cuatro horas antes; después se cobra la franja completa. | Reservas determina cancelación y Cálculo su importe. A dos horas corresponde la franja completa; a exactamente cuatro horas, la cancelación es sin costo. La referencia horaria y el tratamiento de beneficios quedan como preguntas para discutir en clase. |

Los porcentajes, redondeos, impuestos, importes de depósito y fórmulas no definidos quedan como preguntas para las próximas clases.

## 4. Políticas de negocio

| Código | Cuando… | Entonces… | Colaboración |
|---|---|---|---|
| P-01 | Se registra Tarifa actualizada. | Solicitar la notificación del cambio a los miembros con plan. | Tarifario — Notificaciones; destinatarios pendientes en I-06. |
| P-02 | Se registra consumo adicional efectivamente realizado y se determina su importe. | Incorporar el importe a los consumos facturables del miembro. | Cálculo — Facturación; registro de consumo pendiente y duplicados en I-05. |
| P-03 | Una cotización caduca y el cliente desea continuar. | Solicitar una nueva propuesta económica. | Cotizaciones solicita otro cálculo cuando se prepara la nueva propuesta. |

P-02 es candidata. No se activa al consultar un precio ni significa cobrar inmediatamente; R-04 establece el ciclo mensual. La solicitud de aviso de P-01 tampoco equivale a garantizar la entrega del mensaje. Las reacciones no se confunden con las condiciones que protegen INV-01 a INV-07.

## 5. Casos límite y preguntas pendientes

| Caso | Resultado conocido | Decisión abierta |
|---|---|---|
| Aumento con reservas confirmadas | Conservar su precio: INV-02. | I-01: identificar la referencia aplicable y separar acuerdos anteriores. |
| Aumento con cotización vigente | Conservar su importe: INV-04. | H-09: cambios solicitados de fecha, aforo o servicios. |
| Coinciden temporada y hora pico | Ambos son criterios previstos: R-01. | H-01: prioridad o combinación. |
| Horas incluidas sin utilizar | Son beneficios del período. | H-02: acumulación; no se presume caducidad ni arrastre. |
| Horas extra de equipo | Aplicar descuento del miembro: INV-07. | H-03 y H-05: base y posible descuento adicional del 5 %. |
| Aceptación con salón ocupado | No hay reserva automática: INV-03. | H-04 / I-03: respuesta comercial y orden de colaboración. |
| Aceptación al límite de vigencia | Solo se acepta en vigencia: INV-05. | H-08: instante de corte y referencia horaria. |
| Repetición de información sobre un consumo | Solo el consumo real origina P-02. | I-05: ¿cómo reconocer el mismo consumo sin duplicar su facturación? |

El catálogo completo de H-01 a H-10 está en [EventStorming](03-event-storming.md); I-01 a I-06 están en el [context map](05-context-map.md).

## 6. Continuidad en las próximas clases

En la Sesión 4 se discutirán las preguntas sobre precio acordado (I-01), horas disponibles (I-02), aceptación y disponibilidad (I-03), servicios extra (I-04), consumos repetidos (I-05) y destinatarios de los avisos (I-06).

Las reglas y casos identificados servirán para continuar el trabajo en las Sesiones 5 y 6.

La relación entre eventos y políticas está en [EventStorming](03-event-storming.md); las historias que muestran estas reglas están en [Domain Storytelling](04-domain-stories.md).
