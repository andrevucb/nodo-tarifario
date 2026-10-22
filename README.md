# Diseño estratégico del subdominio *Tarifas y precios*

## 1. ¿Qué es Nodo y cuál es nuestra área?

Nodo ofrece espacios de coworking, salas de reuniones y ambientes para eventos. Su operación coordina reservas, membresías, eventos, precios, facturación y control de acceso.

Nuestra área es **Tarifas y precios** (a la que llamamos *tarifario*): determina cuánto cuestan los servicios de Nodo. Se clasifica como **Supporting**; el **Core Domain** del negocio es **Reservas y disponibilidad**. Ver [docs/01-subdominios.md](docs/01-subdominios.md).

Las reglas de precio consideran el tipo de recurso, la franja, el plan del miembro, las horas adicionales y los servicios extra de eventos. Las reservas ya confirmadas conservan el precio acordado ante cambios posteriores de tarifas.

## 2. Bounded contexts identificados y por qué

| Contexto | Responsabilidad | Señal principal de frontera |
|---|---|---|
| **Tarifario** | Administrar las tarifas (base, temporada, hora pico). Solo Dirección las modifica. | Regla de autorización propia; cambia por decisión de Dirección. |
| **Cálculo de precio total** | Determinar el precio de una operación concreta. | Combina datos de varios contextos; ni modifica tarifas ni factura. |
| **Cotizaciones** | Ciclo de vida de la propuesta económica (vigente, aceptada, caducada). | Ciclo de vida y reglas de vigencia propios. |

Tarifario establece condiciones generales; Cálculo determina importes concretos. Cálculo obtiene un precio; Cotizaciones administra la propuesta y su vigencia. Las fronteras son una propuesta inicial que se discutirá en las próximas clases.

## 3. Glosario

El [glosario](docs/02-glosario.md) está separado por bounded context. Distingue **tarifa** de **precio**, **cotización** de **reserva** y **horas incluidas** de **horas extra**. **Presupuesto** es sinónimo de cotización; se utiliza **cotización** como término preferente.

## 4. Context map de nuestra área y vecinos

El [context map](docs/05-context-map.md) relaciona los tres contextos de Tarifas y precios con Reservas y disponibilidad, Membresías y planes, Agenda de eventos, Espacios y recursos, Facturación y cobros y Notificaciones.

Facturación queda fuera del alcance de implementación del equipo, aunque forma parte del negocio Nodo. Notificaciones es un servicio transversal. Las relaciones se muestran sin tipo ni dirección. Su detalle se discutirá en la Sesión 4.

## 5. Índice de documentación

| Documento | Contenido |
|---|---|
| [docs/01-subdominios.md](docs/01-subdominios.md) | Mapa de subdominios, clasificación, construir vs integrar |
| [docs/02-glosario.md](docs/02-glosario.md) | Lenguaje ubicuo por contexto y ambigüedades |
| [docs/03-event-storming.md](docs/03-event-storming.md) | Actualización de tarifas, cálculo del precio total y gestión de cotizaciones |
| [docs/04-domain-stories.md](docs/04-domain-stories.md) | Historias as-is/to-be, fricciones y excepciones |
| [docs/05-context-map.md](docs/05-context-map.md) | Relaciones, datos intercambiados y hotspots de integración |
| [docs/06-reglas-invariantes.md](docs/06-reglas-invariantes.md) | Invariantes, reglas comerciales, políticas y casos límite |

## 6. EventStorming de los procesos

| Proceso | Actor inicial | Comando y resultado | Política |
|---|---|---|---|
| **Actualización de tarifas** | Dirección | Actualizar tarifa → Tarifa actualizada. | P-01: cuando se actualiza una tarifa, solicitar notificación a los miembros con plan. |
| **Cálculo del precio total** | Solicitante | Calcular precio → Precio calculado, como evento candidato. | P-02: cuando se registra consumo extra efectivamente realizado, incorporar su importe a los consumos facturables. Una consulta de precio no activa esta política. |
| **Gestión de cotizaciones** | Cliente | Emitir cotización → Cotización emitida; luego aceptación dentro de vigencia o caducidad sin aceptación. | P-03: cuando una cotización caduca y el cliente desea continuar, solicitar una nueva propuesta. |

En Cálculo se aplican las condiciones tarifarias, se consideran las horas disponibles del plan, se cobra el excedente, se aplica el descuento del plan de equipo a sus horas extra y se consideran aforo y servicios extra del evento.

En Cotizaciones, la propuesta tiene vigencia limitada, conserva su precio mientras está vigente y no reserva automáticamente. Solo puede aceptarse dentro de su vigencia. La aceptación permite continuar la contratación, pero no confirma por sí sola el evento: se requiere depósito y, cuando es grande, aprobación de Dirección.

## 7. Domain Storytelling as-is / to-be

La historia de actualización de tarifas parte de un proceso manual: Dirección revisa y modifica la planilla, informa a Recepción y esta comunica el cambio a los miembros. La participación específica de Recepción en el aviso tarifario es una hipótesis de la historia.

En el to-be, Dirección consulta el tarifario, solicita la modificación y el sistema valida la autorización, actualiza la tarifa y solicita el aviso a Notificaciones. El contraste muestra mejoras en centralización, autorización y comunicación.

Las historias alternativas profundizan la conservación del precio de reservas confirmadas y la aceptación de una cotización cuando el recurso ya no está disponible. Ver [docs/04-domain-stories.md](docs/04-domain-stories.md).

## 8. Reglas e invariantes candidatas

| Código | Invariante candidata |
|---|---|
| INV-01 | Solo Dirección puede modificar las tarifas de Nodo. |
| INV-02 | Una modificación tarifaria no debe alterar el precio de una reserva previamente confirmada. |
| INV-03 | Una cotización no debe reservar automáticamente un recurso. |
| INV-04 | Una cotización vigente debe conservar el precio originalmente ofrecido, aunque cambien las tarifas. |
| INV-05 | No debe aceptarse una cotización que ya haya caducado. |
| INV-06 | Las horas de sala que exceden las incluidas en una membresía deben identificarse como consumo adicional facturable. |
| INV-07 | Las horas extra correspondientes al plan de equipo deben calcularse considerando el descuento del miembro. |

Las reglas comerciales R-01 a R-06 desarrollan ajustes por temporada/hora pico, factores del precio, aforo y servicios extra, facturación mensual, condiciones para confirmar eventos y cancelación de salas. Las políticas P-01 a P-03 describen las reacciones a cambios de tarifa, consumo adicional y caducidad de cotizaciones. El detalle y los casos límite están en [docs/06-reglas-invariantes.md](docs/06-reglas-invariantes.md).

Quedan abiertas la combinación de temporada y hora pico, la acumulación de horas no utilizadas, la base del descuento, el posible 5 % adicional y la respuesta a una cotización aceptada sin disponibilidad.

## 9. Decisiones y esqueleto por contexto

| Decisión | Justificación |
|---|---|
| [0001](decisiones/0001-frontera-tarifario-calculo.md) | Separar la administración de tarifas del cálculo de una operación |
| [0002](decisiones/0002-clasificacion-del-area.md) | Reservas como Core y Tarifas y precios como Supporting |
| [0003](decisiones/0003-frontera-calculo-cotizaciones.md) | Separar el cálculo del ciclo de vida de una propuesta |
| [0004](decisiones/0004-hotspots-y-validacion.md) | Mantener las preguntas pendientes para las Sesiones 4 y 5 |

El esqueleto provisional contiene [Tarifario](src/tarifario/README.md), [Cálculo de precio total](src/calculo-precio-total/README.md) y [Cotizaciones](src/cotizaciones/README.md). Cada carpeta describe qué modela y su lenguaje, sin código.

## 10. Estado del proyecto

**Diseño estratégico hasta la Sesión 3.** En la Sesión 4 se discutirán las relaciones entre contextos y las preguntas de integración. En las Sesiones 5 y 6 se continuará con el desarrollo del proyecto.

## 11. Integrantes y roles

### 11.1. Alcance

El equipo operativo de Nodo está compuesto por Recepción, responsable de operaciones y Dirección. En **Tarifas y precios**, sus responsabilidades se relacionan con administrar tarifas, atender solicitudes y explicar las reglas comerciales. Los clientes participan al consultar precios, solicitar cotizaciones y aceptar propuestas.

### 11.2. Roles internos de Nodo

| Rol | Responsabilidad en nuestra área | Límite |
|---|---|---|
| **Dirección** | Establece la tarifa base y modifica las condiciones por temporada u hora pico. | Solo Dirección puede modificar el tarifario: INV-01. La aprobación de eventos grandes corresponde al proceso de Eventos. |
| **Responsable de operaciones** | Conoce el funcionamiento cotidiano y las reglas y excepciones de precios, planes, consumo y cotizaciones. | Este rol no implica autorización para modificar tarifas ni emitir cotizaciones. |
| **Recepción** | Atiende la operación y comunica avisos manuales a los miembros. Participa en la historia de actualización tarifaria. | Su intervención en el aviso específico de cambio de tarifas no está definida. No modifica el tarifario. |

Dirección ejecuta **Actualizar tarifa**. El responsable de operaciones ayuda a precisar el lenguaje y las reglas. La responsabilidad operativa de **Emitir cotización** sigue pendiente en H-10.

### 11.3. Clientes y otros participantes

| Participante | Participación en Tarifas y precios | Condición que debe respetarse |
|---|---|---|
| **Cliente / solicitante** | Pide una cotización para un evento o una reserva mensual y puede aceptarla. | Solicitar una propuesta no equivale a emitirla. Aceptarla no garantiza disponibilidad ni confirma por sí solo un evento. |
| **Miembro** | Utiliza los espacios y tiene un plan con horas incluidas y descuentos. | Sus beneficios se consideran en el cálculo; el uso que excede las horas disponibles genera consumo adicional. |
| **Miembro con plan de equipo** | Utiliza horas de sala y puede generar horas extra. | Las horas extra reciben su descuento de miembro: INV-07. El posible descuento adicional del 5 % sigue pendiente en H-05. |
| **Visitante** | Solicita una sala y necesita conocer el precio de la operación. | No se le aplican beneficios de miembro sin acreditar que le corresponden. Las condiciones de confirmación y pago para visitantes quedan como pregunta para discutir en clase. |
| **Cliente de evento / organizador** | Solicita una propuesta e informa la cantidad de asistentes y los servicios extra que necesita. | La cantidad solicitada debe distinguirse del aforo máximo del recurso. La confirmación requiere las condiciones de R-05. |
| **Proveedor de servicios extra** | Presta catering o servicios audiovisuales que pueden formar parte del importe de un evento. | La coordinación se realiza mediante Agenda de eventos; no se define una interacción directa con Cálculo o Cotizaciones. |

Una misma persona puede actuar como miembro, visitante o cliente de evento en operaciones distintas. Su condición de miembro y los beneficios aplicables se verifican para cada cálculo.

### 11.4. Responsabilidades de los contextos y sistemas

| Contexto o sistema | Qué aporta o recibe | Responsabilidad que conserva |
|---|---|---|
| Tarifario | Tarifas base y condiciones de ajuste | Administración autorizada de referencias. |
| Membresías y planes | Plan, horas disponibles y descuentos | Definición de derechos y estado de la membresía. |
| Espacios y recursos | Tipo de recurso y capacidad máxima | Características del recurso. |
| Reservas y disponibilidad | Recurso, franja y disponibilidad; recibe importe y propuesta aceptada | Ocupación y conservación del precio confirmado. |
| Agenda de eventos | Datos del evento y servicios extra; recibe importe y propuesta aceptada | Confirmación según depósito y aprobación cuando corresponda. |
| Facturación y cobros | Recibe importes de consumo real para el ciclo mensual | Factura, cobro y estado de pago. |
| Notificaciones | Recibe solicitudes de aviso por cambio tarifario | Comunicación transversal. |

Estas relaciones se representan en el [context map](docs/05-context-map.md). En [EventStorming](docs/03-event-storming.md), el actor es quien ejecuta el comando; los sistemas externos aparecen cuando reciben o entregan información al proceso.

### 11.5. Actores y acciones

| Acción | Actor o participante | Regla o pregunta pendiente |
|---|---|---|
| Actualizar tarifa | Dirección | INV-01: solo Dirección modifica tarifas. |
| Consultar o calcular precio | Solicitante de la operación | Considerar recurso, franja, plan y condiciones aplicables. |
| Solicitar cotización | Cliente | Informar servicios y condiciones de la propuesta. |
| Emitir cotización | Responsable de emisión por definir | H-10: definir quién puede emitirla. |
| Aceptar cotización | Cliente | INV-05: la propuesta debe estar vigente. |
| Consultar beneficios del plan | Solicitante del cálculo | I-02: ¿cómo consultar las horas incluidas disponibles y los descuentos actualizados? |
| Solicitar aviso de cambio tarifario | Sistema, como reacción a Tarifa actualizada | P-01; destinatarios pendientes en I-06. |
