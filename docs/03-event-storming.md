# Event Storming

## 1. Actualización de tarifas (Tarifario)

```mermaid
flowchart LR
  A[Actor: Dirección] --> M[Modelo de lectura: tarifas vigentes, temporadas y horas pico]
  M --> C[Comando: Actualizar tarifa]
  C --> E[Evento: Tarifa actualizada]
  E --> P[Política: cuando se actualiza una tarifa, notificar a miembros con plan]
  P --> N[Sistema externo: Notificaciones]
  R1{{Regla: solo Dirección modifica tarifas}} -.-> C
  R2{{Regla: ajustes por temporada u hora pico}} -.-> C
  R3{{Regla: reservas confirmadas conservan su precio}} -.-> C
  R4{{Regla: cotizaciones vigentes conservan su precio}} -.-> C
  H((Hotspot: ¿cómo se combinan temporada y hora pico?)) -.-> C
```

## 2. Cálculo del precio total

```mermaid
flowchart LR
  A[Actor: Solicitante] --> M[Modelo de lectura: recurso, franja, tarifa aplicable, plan, horas incluidas, descuento, aforo, servicios extra]
  M --> C[Comando: Calcular precio]
  C --> E[Evento candidato: Precio calculado]
  E --> P[Política: cuando se registra consumo extra realizado, incorporar importe a consumos facturables]
  P --> V1[Vecino: Tarifario]
  P --> V2[Vecino: Membresías y planes]
  P --> V3[Vecino: Facturación y cobros]
  R1{{Aplicar condiciones tarifarias}} -.-> C
  R2{{Considerar horas disponibles del plan}} -.-> C
  R3{{Horas que exceden las incluidas generan cobro extra}} -.-> C
  R4{{Horas extra del plan de equipo reciben descuento}} -.-> C
  R5{{Precio de evento considera aforo y servicios extra}} -.-> C
  H1((H-02: ¿se acumulan horas incluidas no utilizadas?)) -.-> C
  H2((H-03: ¿sobre qué tarifa se aplican los descuentos?)) -.-> C
```

## 3. Gestión de cotizaciones

```mermaid
flowchart LR
  A[Actor: Cliente] --> M[Modelo de lectura: precio calculado, servicios, condiciones]
  M --> C[Comando: Emitir cotización]
  C --> E[Evento: Cotización emitida]
  E --> CA[Comando: Aceptar cotización]
  CA --> EA[Evento: Cotización aceptada]
  EA --> V[Vecino: Reservas o eventos]
  E --> T[Condición temporal: vence sin aceptación]
  T --> EC[Evento: Cotización caducada]
  EC --> P[Política: si el cliente desea continuar, solicitar nueva cotización]
```

## 4. Resumen de hotspots

| Código | Pregunta | Proceso |
|---|---|---|
| H-01 | ¿Cómo se combinan temporada y hora pico? | Actualización de tarifas |
| H-02 | ¿Las horas incluidas no usadas se acumulan al siguiente período? | Cálculo |
| H-03 | ¿Sobre qué tarifa se aplican los descuentos del plan? | Cálculo |
| H-04 | ¿Qué pasa si se acepta una cotización y el espacio ya no está disponible? | Cotizaciones |
| H-05 | ¿Cómo se aplica el posible descuento adicional del 5 % en plan de equipo? | Cálculo |