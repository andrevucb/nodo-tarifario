# Diseño estratégico del subdominio *Tarifas y precios*

## 1. ¿Qué es Nodo y cuál es nuestra área?

Nodo ofrece espacios de coworking, salas de reuniones y ambientes para eventos. Su operación coordina reservas, membresías, eventos, precios, facturación y control de acceso.

Nuestra área es **Tarifas y precios** (a la que llamamos *tarifario*): determina cuánto cuestan los servicios de Nodo. Se clasifica como **Supporting**; el **Core Domain** del negocio es **Reservas y disponibilidad**. Ver [docs/01-subdominios.md](docs/01-subdominios.md)

## 2. Bounded contexts identificados y por qué

| Contexto | Responsabilidad | Señal principal de frontera |
|---|---|---|
| **Tarifario** | Administrar las tarifas (base, temporada, hora pico). Solo Dirección las modifica. | Regla de autorización propia; cambia por decisión de Dirección. |
| **Cálculo de precio total** | Determinar el precio de una operación concreta. | Combina datos de varios contextos; ni modifica tarifas ni factura. |
| **Cotizaciones** | Ciclo de vida de la propuesta económica (vigente, aceptada, caducada). | Ciclo de vida y reglas de vigencia propios. |

## 3. Glosario

Separado por bounded context y con polisemias documentadas: [docs/02-glosario.md](docs/02-glosario.md).

## 4. Context map de nuestra área y vecinos

Líneas simples, sin tipo ni dirección: [docs/05-context-map.md](docs/05-context-map.md).

## 5. Índice de documentación

| Documento | Contenido |
|---|---|
| [docs/01-subdominios.md](docs/01-subdominios.md) | Mapa de subdominios, clasificación, construir vs integrar |
| [docs/02-glosario.md](docs/02-glosario.md) | Lenguaje ubicuo por contexto |
| [docs/03-eventstorming.md](docs/03-eventstorming.md) | Tres procesos con la gramática completa |
| [docs/05-context-map.md](docs/05-context-map.md) | Relaciones con vecinos y hotspots de integración |