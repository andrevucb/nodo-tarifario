# Reglas e invariantes candidatas

## 1. Clasificación de las reglas

Una **invariante candidata** expresa una condición que debe mantenerse siempre; una **regla comercial** determina un cálculo o una condición; una **política** describe una reacción «cuando… entonces…». Una regla comercial puede convertirse en una invariante del modelo, pero aún no se decide cómo implementarla.

## 2. Invariantes candidatas

| Código | Invariante candidata | Contexto responsable | Ejemplo o caso límite |
|---|---|---|---|
| INV-01 | Solo Dirección puede modificar las tarifas de Nodo. | Tarifario | Un intento de Recepción no cambia tarifas ni produce Tarifa actualizada. |
| INV-02 | Una modificación tarifaria no debe alterar el precio de una reserva previamente confirmada. | Tarifario / Reservas y disponibilidad | Un aumento posterior no recalcula la reserva confirmada. Una solicitud no confirmada no adquiere esta protección por esa sola condición. |
| INV-03 | Una cotización no debe reservar automáticamente un recurso. | Cotizaciones | Dos clientes pueden disponer de propuestas para una misma fecha; aceptar no garantiza el salón. |
| INV-04 | Una cotización vigente debe conservar el precio originalmente ofrecido, aunque cambien las tarifas. | Cotizaciones | Una subida durante la vigencia no modifica el importe de esa propuesta. Cambios de alcance: H-09. |
| INV-05 | No debe aceptarse una cotización que ya haya caducado. | Cotizaciones | Si caducó sin aceptación, se requiere una nueva propuesta. Límite temporal exacto: H-08. |
| INV-06 | Las horas de sala que exceden las incluidas en una membresía deben identificarse como consumo adicional facturable. | Cálculo de precio total | Si el uso iguala las horas disponibles, no hay excedente; si las supera, la porción excedente es adicional. Una consulta no registra uso. |
| INV-07 | Las horas extra correspondientes al plan de equipo deben calcularse considerando el descuento del miembro. | Cálculo de precio total | El descuento se considera aunque las horas estén fuera del beneficio incluido. Base de aplicación y posible 5 % adicional: H-03 y H-05. |

En INV-02 participan Tarifario y Reservas y disponibilidad: Tarifario modifica las condiciones generales y Reservas mantiene el precio acordado. Las horas incluidas y los descuentos se consideran al determinar los importes de INV-06 e INV-07.

## 3. Reglas comerciales adicionales

| Código | Regla de negocio | Contexto |
|---|---|---|
| R-01 | Dirección puede ajustar tarifas según temporada u horario pico. | Tarifario |
| R-02 | El precio depende del tipo de recurso, la franja y las condiciones del plan del miembro. | Cálculo de precio total |
| R-03 | El precio de un evento considera el aforo y los servicios adicionales solicitados. | Cálculo de precio total |
| R-04 | Las horas extra se incorporan a la facturación mensual. | Cálculo de precio total / Facturación y cobros |
| R-05 | Para confirmar un evento se requiere depósito y, cuando es grande, aprobación de Dirección. | Agenda de eventos / Facturación y cobros |
| R-06 | Una reserva de sala puede cancelarse sin costo hasta cuatro horas antes; después se cobra la franja completa. | Reservas y disponibilidad / Cálculo de precio total |

Los porcentajes, redondeos, impuestos, importes de depósito y fórmulas no definidos quedan como preguntas para las próximas clases.

## 4. Políticas de negocio

| Código | Evento o condición | Política |
|---|---|---|
| P-01 | Tarifa actualizada | Solicitar la notificación del cambio a los miembros con plan. |
| P-02 | Consumo adicional de horas registrado | Incorporar el importe correspondiente a los consumos facturables del miembro. |
| P-03 | Cotización caducada | Si el cliente desea continuar, solicitar una nueva propuesta económica. |

P-02 es candidata. No se activa al consultar un precio ni significa cobrar inmediatamente; R-04 establece el ciclo mensual. La solicitud de aviso de P-01 tampoco equivale a garantizar la entrega del mensaje. Las reacciones no se confunden con las condiciones que protegen INV-01 a INV-07.

## 5. Casos límite y preguntas pendientes

| Caso límite | Comportamiento conocido o decisión pendiente |
|---|---|
| Dirección aumenta tarifas cuando existen reservas confirmadas. | Las reservas confirmadas conservan sus precios anteriores. |
| Dirección modifica tarifas mientras una cotización sigue vigente. | La cotización conserva su importe hasta finalizar su vigencia. |
| Un miembro supera las horas incluidas en su plan. | Las horas adicionales generan cobro; debe precisarse el posible descuento adicional del 5 %. |
| Un cliente acepta una cotización vigente, pero el espacio ya no está disponible. | La cotización no garantiza el recurso. El procedimiento posterior permanece como hotspot. |
| Coinciden temporada y horario pico. | Está pendiente determinar cómo se combinan ambos ajustes. |

El catálogo completo de H-01 a H-10 está en [EventStorming](03-event-storming.md); I-01 a I-06 están en el [context map](05-context-map.md).

## 6. Continuidad en las próximas clases

En la Sesión 4 se discutirán las preguntas sobre precio acordado (I-01), horas disponibles (I-02), aceptación y disponibilidad (I-03), servicios extra (I-04), consumos repetidos (I-05) y destinatarios de los avisos (I-06).

Las reglas y casos identificados servirán para continuar el trabajo en las Sesiones 5 y 6.

La relación entre eventos y políticas está en [EventStorming](03-event-storming.md); las historias que muestran estas reglas están en [Domain Storytelling](04-domain-stories.md).
