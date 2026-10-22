# Cálculo de precio total

## 1. Qué modela

El importe de una operación concreta según recurso, franja, condiciones tarifarias, plan, horas disponibles, aforo y servicios extra, según corresponda.

## 2. Lenguaje propio

Precio, franja, horas incluidas, hora extra, descuento del plan, aforo, servicio extra; Precio calculado es un evento candidato. Ver el [glosario por contexto](../../docs/02-glosario.md).

## 3. Reglas y límites

INV-06, INV-07 y R-02 a R-04; R-06 aporta el importe por cancelación. No define los beneficios del plan ni registra consumo por una consulta. Ver el [inventario de invariantes](../../docs/06-reglas-invariantes.md).

## 4. Colaboraciones

Recibe referencias de Tarifario y datos de Membresías, Espacios, Reservas y Eventos. Entrega importes a Reservas, Eventos y Cotizaciones; los de consumo real, a Facturación. No emite facturas ni modifica tarifas. Ver el [context map](../../docs/05-context-map.md).

## 5. Estado

Carpeta provisional, sin código. El trabajo continuará en las siguientes sesiones del curso.
