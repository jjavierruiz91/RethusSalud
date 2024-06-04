# Migrations

## se crea una carpeta migracion
dotnet ef migrations add InitialCreate 

## para crear la base de datos
dotnet ef database update  para crear la base de datos

## se elimina la base de datos de sqlserver
dotnet ef database drop 

## Se elimina la migración
dotnet ef migrations remove