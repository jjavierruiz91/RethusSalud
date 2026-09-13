-- Crea 3 usuarios de prueba para RethusSalud: Etapa 2, Etapa 3 e Inventario.
-- Contraseña para los tres: Rethus2026*
-- El hash de contraseña ya viene generado con el mismo algoritmo que usa
-- ASP.NET Core Identity (PBKDF2), así que el login funciona igual que si
-- se hubieran creado desde la pantalla de "Usuarios" de la aplicación.
-- Es seguro volver a ejecutar este script: si el correo ya existe, lo omite.

USE RethusSaludDB;
GO

SET NOCOUNT ON;

DECLARE @PasswordHash nvarchar(max) = N'AQAAAAIAAYagAAAAEEngj29MwN7C1CdevQR/4EnmAGsNMaLIqeIijxFFu8PLOHX5qwpTnqO96Eqi0frM7w==';

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'FuncionarioEtapa2')
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (CONVERT(nvarchar(450), NEWID()), 'FuncionarioEtapa2', 'FUNCIONARIOETAPA2', CONVERT(nvarchar(max), NEWID()));

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'FuncionarioEtapa3')
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (CONVERT(nvarchar(450), NEWID()), 'FuncionarioEtapa3', 'FUNCIONARIOETAPA3', CONVERT(nvarchar(max), NEWID()));

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'Inventario')
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (CONVERT(nvarchar(450), NEWID()), 'Inventario', 'INVENTARIO', CONVERT(nvarchar(max), NEWID()));

-- Usuario 1: Etapa 2 (Aprobó)
IF NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE NormalizedEmail = 'DIANA.HERRERA@RETHUSSALUD.TEST')
BEGIN
    DECLARE @Id1 nvarchar(450) = CONVERT(nvarchar(450), NEWID());
    INSERT INTO AspNetUsers
        (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed,
         PasswordHash, SecurityStamp, ConcurrencyStamp,
         PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount,
         NombreCompleto, Activo, Cargo, Telefono, NumeroIdentificacion)
    VALUES
        (@Id1, N'diana.herrera@rethussalud.test', N'DIANA.HERRERA@RETHUSSALUD.TEST',
         N'diana.herrera@rethussalud.test', N'DIANA.HERRERA@RETHUSSALUD.TEST', 1,
         @PasswordHash, CONVERT(nvarchar(max), NEWID()), CONVERT(nvarchar(max), NEWID()),
         0, 0, 1, 0,
         N'Diana Marcela Herrera Peña', 1,
         N'Profesional universitario contratista secretaria de salud Departamental del Cesar',
         N'3012345678', N'1065823417');

    INSERT INTO AspNetUserRoles (UserId, RoleId)
    SELECT @Id1, Id FROM AspNetRoles WHERE Name = 'FuncionarioEtapa2';
END

-- Usuario 2: Etapa 3 (Revisó)
IF NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE NormalizedEmail = 'JORGE.IBARRA@RETHUSSALUD.TEST')
BEGIN
    DECLARE @Id2 nvarchar(450) = CONVERT(nvarchar(450), NEWID());
    INSERT INTO AspNetUsers
        (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed,
         PasswordHash, SecurityStamp, ConcurrencyStamp,
         PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount,
         NombreCompleto, Activo, Cargo, Telefono, NumeroIdentificacion)
    VALUES
        (@Id2, N'jorge.ibarra@rethussalud.test', N'JORGE.IBARRA@RETHUSSALUD.TEST',
         N'jorge.ibarra@rethussalud.test', N'JORGE.IBARRA@RETHUSSALUD.TEST', 1,
         @PasswordHash, CONVERT(nvarchar(max), NEWID()), CONVERT(nvarchar(max), NEWID()),
         0, 0, 1, 0,
         N'Jorge Andrés Ibarra Cuello', 1,
         N'Asesor Jurídico secretaria de salud Departamental del Cesar',
         N'3023456789', N'77123456');

    INSERT INTO AspNetUserRoles (UserId, RoleId)
    SELECT @Id2, Id FROM AspNetRoles WHERE Name = 'FuncionarioEtapa3';
END

-- Usuario 3: Inventario (Generó / Consecutivo)
IF NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE NormalizedEmail = 'LINA.OSPINA@RETHUSSALUD.TEST')
BEGIN
    DECLARE @Id3 nvarchar(450) = CONVERT(nvarchar(450), NEWID());
    INSERT INTO AspNetUsers
        (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed,
         PasswordHash, SecurityStamp, ConcurrencyStamp,
         PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount,
         NombreCompleto, Activo, Cargo, Telefono, NumeroIdentificacion)
    VALUES
        (@Id3, N'lina.ospina@rethussalud.test', N'LINA.OSPINA@RETHUSSALUD.TEST',
         N'lina.ospina@rethussalud.test', N'LINA.OSPINA@RETHUSSALUD.TEST', 1,
         @PasswordHash, CONVERT(nvarchar(max), NEWID()), CONVERT(nvarchar(max), NEWID()),
         0, 0, 1, 0,
         N'Lina Patricia Ospina Vega', 1,
         N'Profesional universitario - Coordinación de Prestación y Desarrollo de Servicios de Salud',
         N'3034567890', N'1065912345');

    INSERT INTO AspNetUserRoles (UserId, RoleId)
    SELECT @Id3, Id FROM AspNetRoles WHERE Name = 'Inventario';
END
GO
