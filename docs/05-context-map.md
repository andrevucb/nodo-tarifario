# Context map

- **Propios:** Reservas y disponibilidad (Core), Membresías y planes, Agenda de eventos, Espacios y recursos y los tres contextos de nuestra área.
- **Externo:** Facturación y cobros (fuera del alcance de implementación del equipo; forma parte del negocio Nodo).
- **Transversal:** Notificaciones.

```mermaid
flowchart TB
  subgraph TP[Tarifas y precios - Supporting]
    TAR[Tarifario]
    CAL[Cálculo de precio total]
    COT[Cotizaciones]
  end
  RES[Reservas y disponibilidad - Core]
  MEM[Membresías y planes]
  EVT[Agenda de eventos]
  ESP[Espacios y recursos]
  FAC[Facturación y cobros - Externo]
  NOT[Notificaciones - Transversal]
  TAR --- CAL
  CAL --- COT
  MEM --- CAL
  ESP --- CAL
  RES --- CAL
  EVT --- CAL
  RES --- COT
  EVT --- COT
  FAC --- CAL
  TAR -.- NOT
```

## 1. Relaciones y datos que cruzan

| Relación | ¿Qué se solicita? | ¿Qué se entrega o comparte? |
|---|---|---|
| L-01 Tarifario — Cálculo | Tarifas y condiciones vigentes | Tarifas base y ajustes aplicables |
| L-02 Cálculo — Cotizaciones | Importe de los servicios solicitados | Precio calculado para la propuesta |
| L-03 Membresías y planes — Cálculo | Plan, horas incluidas y descuentos | Beneficios para determinar el precio; no se definió respuesta hacia Membresías |
| L-04 Espacios y recursos — Cálculo | Tipo de recurso y capacidad | Características necesarias para calcular el importe |
| L-05 Reservas — Cálculo | Recurso y franja solicitada | Precio calculado para la reserva |
| L-06 Reservas — Cotizaciones | Disponibilidad del recurso solicitado | Datos de la cotización aceptada para continuar la contratación |
| L-07 Eventos — Cálculo | Aforo y servicios adicionales | Importe calculado para el evento |
| L-08 Eventos — Cotizaciones | Datos del evento y sus condiciones | Propuesta económica y aceptación |
| L-09 Facturación — Cálculo | Facturación necesita los importes de consumos adicionales efectivamente realizados | Cálculo entrega el importe facturable; no emite facturas ni procesa pagos |
| T-01 Tarifario — Notificaciones | Comunicación de cambios tarifarios | Solicitud de aviso a miembros con plan |

## 2. Por vecino: qué le pido / qué le entrego

| Vecino | Le pido | Le entrego |
|---|---|---|
| Reservas y disponibilidad | Disponibilidad del recurso; recurso y franja solicitada | Precio calculado para la reserva; datos de la cotización aceptada |
| Membresías y planes | Plan, horas incluidas, descuentos | Nada definido todavía |
| Agenda de eventos | Datos del evento, aforo, servicios extra | Importe del evento; propuesta económica y aceptación |
| Espacios y recursos | Tipo de recurso y capacidad | Nada (solo consume) |
| Facturación y cobros (externo) | — | Importes facturables (horas extra y consumos) |
| Notificaciones (transversal) | Envío de avisos | Solicitud de aviso por tarifa actualizada |

## 3. Hotspots de integración

| Código | Relación | Pregunta pendiente |
|---|---|---|
| I-01 | Tarifario — Cálculo | ¿Cómo identificar la tarifa aplicable sin afectar precios previamente acordados? |
| I-02 | Membresías — Cálculo | ¿Cómo consultar horas disponibles y descuentos actualizados, y quién registra el uso para evitar consumir dos veces el mismo beneficio? |
| I-03 | Cotizaciones — Reservas | ¿Qué sucede si se acepta una cotización vigente pero el recurso ya no está disponible? |
| I-04 | Eventos — Cálculo | ¿Cómo se comunicarán el aforo y los importes de servicios extra? |
| I-05 | Cálculo — Facturación | ¿Cómo evitar que un mismo consumo adicional se facture dos veces? |
| I-06 | Tarifario — Notificaciones | ¿Cómo identificar a los miembros que deben recibir el aviso? |

## 4. Preguntas para discutir en la Sesión 4

Las diez líneas del diagrama corresponden a L-01 a L-09 y T-01. Todas son simples y sin flechas. «Propios» identifica contextos del negocio Nodo, no implica que el equipo implemente a todos; el esqueleto de este repositorio abarca solo los tres de Tarifas y precios.

| Hotspot | Dato o compromiso que debe protegerse | Pregunta para discutir en clase |
|---|---|---|
| I-01 | Precio ya acordado frente a una nueva referencia tarifaria | ¿Qué condiciones y referencia temporal usa cada operación y quién conserva el acuerdo? |
| I-02 | Beneficios del miembro y consumo efectivamente realizado | ¿Quién informa horas disponibles y quién registra el uso, especialmente ante solicitudes simultáneas? |
| I-03 | Diferencia entre aceptación y ocupación | ¿Cuándo se comprueba disponibilidad y qué se comunica si el recurso ya no está disponible? |
| I-04 | Aforo, servicios solicitados e importes de extras | ¿Quién responde por esos datos y qué sucede si cambian después de cotizar? |
| I-05 | Un mismo consumo adicional | ¿Qué información permite reconocerlo sin facturarlo dos veces? |
| I-06 | Destinatarios del aviso tarifario | ¿Quién identifica a los miembros con plan y qué resultado del aviso necesita conocer Tarifario? |

Estas preguntas se retomarán en la Sesión 4 para continuar el análisis de las relaciones entre contextos. La relación entre las fronteras y las reglas se desarrolla en las [decisiones](../decisiones/0001-frontera-tarifario-calculo.md) y el [inventario de invariantes](06-reglas-invariantes.md).
