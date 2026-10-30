# Instalación y ejecución

## Requisitos

- Git
- **.NET SDK 10** (versión principal 10.0). El proyecto apunta a .NET 10 (`net10.0`).

Descarga e instala el **SDK**, no solo el Runtime, desde la [página oficial de .NET 10](https://dotnet.microsoft.com/download/dotnet/10.0). Selecciona el instalador correspondiente a tu sistema operativo. El SDK incluye las herramientas necesarias para compilar y ejecutar el proyecto.

Comprueba la instalación con:

```bash
dotnet --version
```

La versión mostrada debe comenzar con `10.`.

## Preparar el proyecto

```bash
git clone https://github.com/andrevucb/nodo-tarifario.git
cd nodo-tarifario
dotnet restore Nodo.slnx
```

## Ejecutar la aplicación

```bash
dotnet run --project src/Web/Nodo.Web/Nodo.Web.csproj --launch-profile http
```

Abre [http://localhost:5181](http://localhost:5181) en el navegador. La base de datos se crea y se actualiza al iniciar la aplicación; en entorno de desarrollo también se cargan datos de prueba.
