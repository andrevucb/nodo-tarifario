# Separar Tarifario de Cálculo de precio total

## 1. Estado

**Propuesta estratégica hasta la Sesión 3.** La frontera y sus reglas se discutirán en las próximas clases.

## 2. Contexto y señales

Dirección modifica referencias por temporada u hora pico. En cambio, una operación combina esas referencias con recurso, franja y beneficios del miembro. «Tarifa» es la condición general; «precio» es un importe concreto. Las reservas confirmadas conservan ese importe aunque cambie la referencia (INV-02).

## 3. Decisión y alternativas

Se mantienen **Tarifario** y **Cálculo de precio total** separados: autorización y cambio de condiciones pertenecen al primero; determinar el importe de una operación, al segundo. No se adopta «un área = un contexto»: el subdominio Supporting contiene responsabilidades y reglas que cambian por su cuenta.

Un contexto único reduciría los intercambios, pero mezclaría administración y cálculo. Tampoco se crea un contexto para temporada y otro para hora pico: ambos son condiciones administradas por Dirección, sin evidencia de lenguaje ni ciclo de vida independientes. Los descuentos se consideran en el cálculo; definir los beneficios del plan pertenece a Membresías, no a un nuevo contexto propio.

## 4. Consecuencias y dudas

L-01 comparte referencias tarifarias y L-03 beneficios del miembro. Tarifario no factura ni modifica precios de reservas; Cálculo no autoriza cambios de tarifa ni administra planes. I-01, H-01 y H-03 deben precisar referencia temporal, combinación de ajustes y base del descuento. Si el cálculo resulta ser una capacidad sin modelo propio, esta frontera deberá reevaluarse antes de implementarla.

Ver [glosario](../docs/02-glosario.md), [context map](../docs/05-context-map.md) e [invariantes](../docs/06-reglas-invariantes.md).
