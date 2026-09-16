-- Actualiza el correo y la contrasena de las 4 cuentas de funcionario de prueba
-- (etapa1, etapa2, etapa3, inventario) a un patron @gmail.com con contrasena "123456".
-- Seguro de correr en local o en el servidor de despliegue: solo actualiza si encuentra
-- la cuenta por su correo actual (@rethus.local); si ya fue actualizada antes, no hace nada.

SET QUOTED_IDENTIFIER ON;

UPDATE AspNetUsers
SET Email = 'etapa1@gmail.com',
    NormalizedEmail = 'ETAPA1@GMAIL.COM',
    UserName = 'etapa1@gmail.com',
    NormalizedUserName = 'ETAPA1@GMAIL.COM',
    PasswordHash = 'AQAAAAEAAYagAAAAEMTlVTgKQs6DZdZCDKGfSp1Wle4E0IWiXiVszMs2ZnhgXw/06UM6MATLjFDPrbgDZA==',
    LockoutEnd = NULL,
    AccessFailedCount = 0
WHERE Email = 'etapa1@rethus.local';

UPDATE AspNetUsers
SET Email = 'etapa2@gmail.com',
    NormalizedEmail = 'ETAPA2@GMAIL.COM',
    UserName = 'etapa2@gmail.com',
    NormalizedUserName = 'ETAPA2@GMAIL.COM',
    PasswordHash = 'AQAAAAEAAYagAAAAEJ4ZFgBbgzSFF0rUT2BdP1Itwm2Ij7URT+An2mWbJR7XcaLHghXKsp8zO7cXPQDlPQ==',
    LockoutEnd = NULL,
    AccessFailedCount = 0
WHERE Email = 'etapa2@rethus.local';

UPDATE AspNetUsers
SET Email = 'etapa3@gmail.com',
    NormalizedEmail = 'ETAPA3@GMAIL.COM',
    UserName = 'etapa3@gmail.com',
    NormalizedUserName = 'ETAPA3@GMAIL.COM',
    PasswordHash = 'AQAAAAEAAYagAAAAELLoxXOmDimjzisblPelFvr7DvyKK8fvQAc+lDsmNKKqaVI9WMBRxEdIw16RXHbw9Q==',
    LockoutEnd = NULL,
    AccessFailedCount = 0
WHERE Email = 'etapa3@rethus.local';

UPDATE AspNetUsers
SET Email = 'inventario@gmail.com',
    NormalizedEmail = 'INVENTARIO@GMAIL.COM',
    UserName = 'inventario@gmail.com',
    NormalizedUserName = 'INVENTARIO@GMAIL.COM',
    PasswordHash = 'AQAAAAEAAYagAAAAEO+sCWCfoIpgPsvdLP2/153bN9jKgYskV3Yqprm6ib7vCGoYWdpkYvyC1+3f+GQ6Zg==',
    LockoutEnd = NULL,
    AccessFailedCount = 0
WHERE Email = 'inventario@rethus.local';

SELECT Email, NombreCompleto FROM AspNetUsers
WHERE Email IN ('etapa1@gmail.com', 'etapa2@gmail.com', 'etapa3@gmail.com', 'inventario@gmail.com');
