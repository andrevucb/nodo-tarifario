# Separar Cálculo de precio total de Cotizaciones

## 1. Estado

**Propuesta estratégica hasta la Sesión 3.** La frontera y sus reglas se discutirán en las próximas clases.

## 2. Contexto y señales

Calcular produce un importe. Una cotización agrega condiciones, vigencia y resultados alternativos: aceptación o caducidad. «Precio» designa en Cálculo un resultado de operación; en Cotizaciones, el importe ofrecido que se conserva durante la vigencia. La diferencia de uso señala una frontera, y queda como diferencia de lenguaje para discutir en clase.

## 3. Decisión y alternativas de partición

Se conserva **Cotizaciones** como contexto separado que recibe un importe de Cálculo (L-02) y gestiona el ciclo de la propuesta. INV-04 e INV-05 cambian por decisiones comerciales sobre vigencia y aceptación; no obligan a cambiar cómo se obtiene el importe.

| Alternativa tentadora | Motivo para no adoptarla ahora |
|---|---|
| Reunir cálculo y cotización porque ambos hablan de precio | Calcular no implica emitir una propuesta; una cotización puede caducar sin un nuevo cálculo. |
| Separar cotizaciones vigentes, aceptadas y caducadas en contextos | Son estados del mismo ciclo y comparten lenguaje y reglas. |
| Crear un cálculo para salas y otro para eventos | Cambian los factores, pero ambos determinan importes. No hay evidencia suficiente de modelos autónomos. |
| Reunir Cotizaciones con Reservas | Una propuesta no reserva espacios; aceptación y disponibilidad tienen reglas distintas. |
| Incorporar Facturación al cálculo | Determinar un importe no es facturar ni cobrar; P-02 necesita consumo real. |

## 4. Consecuencias y preguntas abiertas

Cotizaciones no administra disponibilidad ni beneficios del plan. La confirmación del evento requiere condiciones de los vecinos, entre ellas R-05. H-04 / I-03 mantiene abierta la respuesta a aceptación sin disponibilidad; H-08 y H-09 precisan vencimiento y cambios de alcance. El trabajo continuará en las siguientes sesiones.

Los tres contextos siguen perteneciendo al mismo subdominio **Tarifas y precios**: administrar referencias, calcular importes y conservar propuestas son capacidades comerciales relacionadas, con fronteras de modelo distintas. Ver [historias](../docs/04-domain-stories.md) e [invariantes](../docs/06-reglas-invariantes.md).
