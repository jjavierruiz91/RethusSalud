# Migrations

## se crea una carpeta migracion
dotnet ef migrations add InitialCreate 

## para crear la base de datos
dotnet ef database update

## se elimina la base de datos de sqlserver
dotnet ef database drop 

## Se elimina la migración
dotnet ef migrations remove

# Publish 
Estos comandos nos permiten configurar el entorno en el que vamos a trabajar y el ambiente para realizar los release

NOTA: cada que se quiera generar un release a produccion se tiene que cambiar al entorno de Production y para desarrollo el comando de Develoment

## Develoment
set ASPNETCORE_ENVIRONMENT=Development
dotnet publish -c Development -o ./publish/development

## Production 
set ASPNETCORE_ENVIRONMENT=Production

### Windows
dotnet publish -c ReleaseTest -r win-x64 --self-contained -o ./publish/production

### 
dotnet publish -c ReleaseV3 -o ./publish/production