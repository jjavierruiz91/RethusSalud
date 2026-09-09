SET NOCOUNT ON;
BEGIN TRANSACTION;
DECLARE @sid INT, @solid INT;
-- Registro 1: Alejandro Garcia Gomez - Etapa1
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-1', 1, '900500001', 'Valledupar', 1,
         'Alejandro', 'Garcia Gomez', 1, 11, 11,
         DATEADD(year, -41, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 10 # 5-20', NULL, '3001000000', 'ciudadano.prueba1@example.com', 0, DATEADD(day, -1, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 16, 2, 1, 1, DATEADD(day, -0, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -0, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 16', DATEADD(year, -4, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -0, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/1/1.pdf', 'application/pdf', 1024, DATEADD(day, -0, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/1/2.pdf', 'application/pdf', 1024, DATEADD(day, -0, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/1/3.pdf', 'application/pdf', 1024, DATEADD(day, -0, SYSUTCDATETIME()));

-- Registro 2: Valentina Lopez Molina - Etapa2
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-2', 1, '900500002', 'Valledupar', 2,
         'Valentina', 'Lopez Molina', 1, 11, 11,
         DATEADD(year, -29, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 11 # 6-21', NULL, '3001000001', 'ciudadano.prueba2@example.com', 0, DATEADD(day, -2, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 6, 2, 1, 2, DATEADD(day, -1, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -1, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 6', DATEADD(year, -7, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -1, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -1, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/2/1.pdf', 'application/pdf', 1024, DATEADD(day, -1, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/2/2.pdf', 'application/pdf', 1024, DATEADD(day, -1, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/2/3.pdf', 'application/pdf', 1024, DATEADD(day, -1, SYSUTCDATETIME()));

-- Registro 3: Jose Gomez Martinez - Etapa3
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-3', 1, '900500003', 'Valledupar', 1,
         'Jose', 'Gomez Martinez', 1, 11, 11,
         DATEADD(year, -38, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 12 # 7-22', NULL, '3001000002', 'ciudadano.prueba3@example.com', 0, DATEADD(day, -3, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 8, 2, 1, 3, DATEADD(day, -2, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -2, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 8', DATEADD(year, -1, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -2, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -2, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -2, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/3/1.pdf', 'application/pdf', 1024, DATEADD(day, -2, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/3/2.pdf', 'application/pdf', 1024, DATEADD(day, -2, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/3/3.pdf', 'application/pdf', 1024, DATEADD(day, -2, SYSUTCDATETIME()));

-- Registro 4: Juliana Hernandez Alvarez - Etapa Inventario
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-4', 1, '900500004', 'Valledupar', 2,
         'Juliana', 'Hernandez Alvarez', 1, 11, 11,
         DATEADD(year, -50, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 13 # 8-23', NULL, '3001000003', 'ciudadano.prueba4@example.com', 0, DATEADD(day, -4, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 15, 2, 1, 4, DATEADD(day, -3, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -3, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 15', DATEADD(year, -4, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -3, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -3, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -3, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 4, 'seed-script', 'Aprobada', DATEADD(day, -3, DATEADD(hour, 3, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/4/1.pdf', 'application/pdf', 1024, DATEADD(day, -3, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/4/2.pdf', 'application/pdf', 1024, DATEADD(day, -3, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/4/3.pdf', 'application/pdf', 1024, DATEADD(day, -3, SYSUTCDATETIME()));

-- Registro 5: Santiago Munoz Sanchez - Etapa1
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-5', 1, '900500005', 'Valledupar', 1,
         'Santiago', 'Munoz Sanchez', 1, 11, 11,
         DATEADD(year, -24, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 14 # 9-24', NULL, '3001000004', 'ciudadano.prueba5@example.com', 0, DATEADD(day, -5, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 17, 2, 1, 1, DATEADD(day, -4, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -4, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 17', DATEADD(year, -3, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -4, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/5/1.pdf', 'application/pdf', 1024, DATEADD(day, -4, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/5/2.pdf', 'application/pdf', 1024, DATEADD(day, -4, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/5/3.pdf', 'application/pdf', 1024, DATEADD(day, -4, SYSUTCDATETIME()));

-- Registro 6: Gabriela Castro Torres - Etapa2
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-6', 1, '900500006', 'Valledupar', 2,
         'Gabriela', 'Castro Torres', 1, 11, 11,
         DATEADD(year, -33, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 15 # 10-25', NULL, '3001000005', 'ciudadano.prueba6@example.com', 0, DATEADD(day, -6, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 9, 2, 1, 2, DATEADD(day, -5, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -5, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 9', DATEADD(year, -4, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -5, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -5, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/6/1.pdf', 'application/pdf', 1024, DATEADD(day, -5, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/6/2.pdf', 'application/pdf', 1024, DATEADD(day, -5, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/6/3.pdf', 'application/pdf', 1024, DATEADD(day, -5, SYSUTCDATETIME()));

-- Registro 7: Felipe Torres Garcia - Etapa3
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-7', 1, '900500007', 'Valledupar', 1,
         'Felipe', 'Torres Garcia', 1, 11, 11,
         DATEADD(year, -48, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 16 # 11-26', NULL, '3001000006', 'ciudadano.prueba7@example.com', 0, DATEADD(day, -7, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 6, 2, 1, 3, DATEADD(day, -6, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -6, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 6', DATEADD(year, -2, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -6, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -6, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -6, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/7/1.pdf', 'application/pdf', 1024, DATEADD(day, -6, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/7/2.pdf', 'application/pdf', 1024, DATEADD(day, -6, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/7/3.pdf', 'application/pdf', 1024, DATEADD(day, -6, SYSUTCDATETIME()));

-- Registro 8: Andrea Pena Florez - Etapa Inventario
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-8', 1, '900500008', 'Valledupar', 2,
         'Andrea', 'Pena Florez', 1, 11, 11,
         DATEADD(year, -40, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 17 # 12-27', NULL, '3001000007', 'ciudadano.prueba8@example.com', 0, DATEADD(day, -8, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 14, 2, 1, 4, DATEADD(day, -7, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -7, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 14', DATEADD(year, -1, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -7, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -7, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -7, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 4, 'seed-script', 'Aprobada', DATEADD(day, -7, DATEADD(hour, 3, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/8/1.pdf', 'application/pdf', 1024, DATEADD(day, -7, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/8/2.pdf', 'application/pdf', 1024, DATEADD(day, -7, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/8/3.pdf', 'application/pdf', 1024, DATEADD(day, -7, SYSUTCDATETIME()));

-- Registro 9: Daniel Ortiz Moreno - Etapa1
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-9', 1, '900500009', 'Valledupar', 1,
         'Daniel', 'Ortiz Moreno', 1, 11, 11,
         DATEADD(year, -48, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 18 # 13-28', NULL, '3001000008', 'ciudadano.prueba9@example.com', 0, DATEADD(day, -9, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 6, 2, 1, 1, DATEADD(day, -8, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -8, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 6', DATEADD(year, -2, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -8, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/9/1.pdf', 'application/pdf', 1024, DATEADD(day, -8, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/9/2.pdf', 'application/pdf', 1024, DATEADD(day, -8, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/9/3.pdf', 'application/pdf', 1024, DATEADD(day, -8, SYSUTCDATETIME()));

-- Registro 10: Juliana Ramirez Guerrero - Etapa2
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-10', 1, '900500010', 'Valledupar', 2,
         'Juliana', 'Ramirez Guerrero', 1, 11, 11,
         DATEADD(year, -47, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 19 # 14-29', NULL, '3001000009', 'ciudadano.prueba10@example.com', 0, DATEADD(day, -10, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 15, 2, 1, 2, DATEADD(day, -9, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -9, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 15', DATEADD(year, -4, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -9, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -9, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/10/1.pdf', 'application/pdf', 1024, DATEADD(day, -9, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/10/2.pdf', 'application/pdf', 1024, DATEADD(day, -9, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/10/3.pdf', 'application/pdf', 1024, DATEADD(day, -9, SYSUTCDATETIME()));

-- Registro 11: Daniel Martinez Rodriguez - Etapa3
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-11', 1, '900500011', 'Valledupar', 1,
         'Daniel', 'Martinez Rodriguez', 1, 11, 11,
         DATEADD(year, -38, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 20 # 15-30', NULL, '3001000010', 'ciudadano.prueba11@example.com', 0, DATEADD(day, -11, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 15, 2, 1, 3, DATEADD(day, -10, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -10, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 15', DATEADD(year, -5, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -10, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -10, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -10, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/11/1.pdf', 'application/pdf', 1024, DATEADD(day, -10, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/11/2.pdf', 'application/pdf', 1024, DATEADD(day, -10, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/11/3.pdf', 'application/pdf', 1024, DATEADD(day, -10, SYSUTCDATETIME()));

-- Registro 12: Laura Pena Perez - Etapa Inventario
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-12', 1, '900500012', 'Valledupar', 2,
         'Laura', 'Pena Perez', 1, 11, 11,
         DATEADD(year, -30, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 21 # 16-31', NULL, '3001000011', 'ciudadano.prueba12@example.com', 0, DATEADD(day, -12, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 18, 2, 1, 4, DATEADD(day, -11, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -11, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 18', DATEADD(year, -7, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -11, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -11, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -11, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 4, 'seed-script', 'Aprobada', DATEADD(day, -11, DATEADD(hour, 3, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/12/1.pdf', 'application/pdf', 1024, DATEADD(day, -11, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/12/2.pdf', 'application/pdf', 1024, DATEADD(day, -11, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/12/3.pdf', 'application/pdf', 1024, DATEADD(day, -11, SYSUTCDATETIME()));

-- Registro 13: Juan Ortiz Suarez - Etapa1
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-13', 1, '900500013', 'Valledupar', 1,
         'Juan', 'Ortiz Suarez', 1, 11, 11,
         DATEADD(year, -47, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 22 # 17-32', NULL, '3001000012', 'ciudadano.prueba13@example.com', 0, DATEADD(day, -13, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 18, 2, 1, 1, DATEADD(day, -12, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -12, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 18', DATEADD(year, -3, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -12, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/13/1.pdf', 'application/pdf', 1024, DATEADD(day, -12, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/13/2.pdf', 'application/pdf', 1024, DATEADD(day, -12, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/13/3.pdf', 'application/pdf', 1024, DATEADD(day, -12, SYSUTCDATETIME()));

-- Registro 14: Andrea Florez Hernandez - Etapa2
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-14', 1, '900500014', 'Valledupar', 2,
         'Andrea', 'Florez Hernandez', 1, 11, 11,
         DATEADD(year, -41, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 23 # 18-33', NULL, '3001000013', 'ciudadano.prueba14@example.com', 0, DATEADD(day, -1, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 15, 2, 1, 2, DATEADD(day, -0, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -0, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 15', DATEADD(year, -2, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -0, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -0, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/14/1.pdf', 'application/pdf', 1024, DATEADD(day, -0, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/14/2.pdf', 'application/pdf', 1024, DATEADD(day, -0, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/14/3.pdf', 'application/pdf', 1024, DATEADD(day, -0, SYSUTCDATETIME()));

-- Registro 15: Sebastian Suarez Gonzalez - Etapa3
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-15', 1, '900500015', 'Valledupar', 1,
         'Sebastian', 'Suarez Gonzalez', 1, 11, 11,
         DATEADD(year, -39, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 24 # 19-34', NULL, '3001000014', 'ciudadano.prueba15@example.com', 0, DATEADD(day, -2, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 13, 2, 1, 3, DATEADD(day, -1, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -1, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 13', DATEADD(year, -3, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -1, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -1, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -1, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/15/1.pdf', 'application/pdf', 1024, DATEADD(day, -1, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/15/2.pdf', 'application/pdf', 1024, DATEADD(day, -1, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/15/3.pdf', 'application/pdf', 1024, DATEADD(day, -1, SYSUTCDATETIME()));

-- Registro 16: Natalia Vargas Sanchez - Etapa Inventario
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-16', 1, '900500016', 'Valledupar', 2,
         'Natalia', 'Vargas Sanchez', 1, 11, 11,
         DATEADD(year, -38, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 25 # 20-35', NULL, '3001000015', 'ciudadano.prueba16@example.com', 0, DATEADD(day, -3, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 19, 2, 1, 4, DATEADD(day, -2, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -2, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 19', DATEADD(year, -6, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -2, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -2, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -2, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 4, 'seed-script', 'Aprobada', DATEADD(day, -2, DATEADD(hour, 3, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/16/1.pdf', 'application/pdf', 1024, DATEADD(day, -2, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/16/2.pdf', 'application/pdf', 1024, DATEADD(day, -2, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/16/3.pdf', 'application/pdf', 1024, DATEADD(day, -2, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 4, 'Tarjeta profesional.pdf', 'seed/16/4.pdf', 'application/pdf', 1024, DATEADD(day, -2, SYSUTCDATETIME()));

-- Registro 17: Mateo Medina Medina - Etapa1
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-17', 1, '900500017', 'Valledupar', 1,
         'Mateo', 'Medina Medina', 1, 11, 11,
         DATEADD(year, -38, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 26 # 21-36', NULL, '3001000016', 'ciudadano.prueba17@example.com', 0, DATEADD(day, -4, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 5, 2, 1, 1, DATEADD(day, -3, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -3, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 5', DATEADD(year, -1, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -3, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/17/1.pdf', 'application/pdf', 1024, DATEADD(day, -3, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/17/2.pdf', 'application/pdf', 1024, DATEADD(day, -3, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/17/3.pdf', 'application/pdf', 1024, DATEADD(day, -3, SYSUTCDATETIME()));

-- Registro 18: Mariana Torres Vargas - Etapa2
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-18', 1, '900500018', 'Valledupar', 2,
         'Mariana', 'Torres Vargas', 1, 11, 11,
         DATEADD(year, -28, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 27 # 22-37', NULL, '3001000017', 'ciudadano.prueba18@example.com', 0, DATEADD(day, -5, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 9, 2, 1, 2, DATEADD(day, -4, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -4, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 9', DATEADD(year, -4, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -4, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -4, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/18/1.pdf', 'application/pdf', 1024, DATEADD(day, -4, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/18/2.pdf', 'application/pdf', 1024, DATEADD(day, -4, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/18/3.pdf', 'application/pdf', 1024, DATEADD(day, -4, SYSUTCDATETIME()));

-- Registro 19: Nicolas Munoz Reyes - Etapa3
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-19', 1, '900500019', 'Valledupar', 1,
         'Nicolas', 'Munoz Reyes', 1, 11, 11,
         DATEADD(year, -44, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 28 # 23-38', NULL, '3001000018', 'ciudadano.prueba19@example.com', 0, DATEADD(day, -6, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 16, 2, 1, 3, DATEADD(day, -5, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -5, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 16', DATEADD(year, -4, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -5, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -5, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -5, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/19/1.pdf', 'application/pdf', 1024, DATEADD(day, -5, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/19/2.pdf', 'application/pdf', 1024, DATEADD(day, -5, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/19/3.pdf', 'application/pdf', 1024, DATEADD(day, -5, SYSUTCDATETIME()));

-- Registro 20: Isabella Rincon Vargas - Etapa Inventario
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-20', 1, '900500020', 'Valledupar', 2,
         'Isabella', 'Rincon Vargas', 1, 11, 11,
         DATEADD(year, -53, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 29 # 24-39', NULL, '3001000019', 'ciudadano.prueba20@example.com', 0, DATEADD(day, -7, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 19, 2, 1, 4, DATEADD(day, -6, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -6, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 19', DATEADD(year, -3, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -6, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -6, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -6, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 4, 'seed-script', 'Aprobada', DATEADD(day, -6, DATEADD(hour, 3, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/20/1.pdf', 'application/pdf', 1024, DATEADD(day, -6, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/20/2.pdf', 'application/pdf', 1024, DATEADD(day, -6, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/20/3.pdf', 'application/pdf', 1024, DATEADD(day, -6, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 4, 'Tarjeta profesional.pdf', 'seed/20/4.pdf', 'application/pdf', 1024, DATEADD(day, -6, SYSUTCDATETIME()));

-- Registro 21: Juan Lopez Perez - Etapa1
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-21', 1, '900500021', 'Valledupar', 1,
         'Juan', 'Lopez Perez', 1, 11, 11,
         DATEADD(year, -40, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 30 # 25-40', NULL, '3001000020', 'ciudadano.prueba21@example.com', 0, DATEADD(day, -8, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 16, 2, 1, 1, DATEADD(day, -7, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -7, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 16', DATEADD(year, -7, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -7, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/21/1.pdf', 'application/pdf', 1024, DATEADD(day, -7, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/21/2.pdf', 'application/pdf', 1024, DATEADD(day, -7, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/21/3.pdf', 'application/pdf', 1024, DATEADD(day, -7, SYSUTCDATETIME()));

-- Registro 22: Alejandra Munoz Vargas - Etapa2
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-22', 1, '900500022', 'Valledupar', 2,
         'Alejandra', 'Munoz Vargas', 1, 11, 11,
         DATEADD(year, -38, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 31 # 26-41', NULL, '3001000021', 'ciudadano.prueba22@example.com', 0, DATEADD(day, -9, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 10, 2, 1, 2, DATEADD(day, -8, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -8, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 10', DATEADD(year, -3, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -8, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -8, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/22/1.pdf', 'application/pdf', 1024, DATEADD(day, -8, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/22/2.pdf', 'application/pdf', 1024, DATEADD(day, -8, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/22/3.pdf', 'application/pdf', 1024, DATEADD(day, -8, SYSUTCDATETIME()));

-- Registro 23: Camilo Rincon Martinez - Etapa3
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-23', 1, '900500023', 'Valledupar', 1,
         'Camilo', 'Rincon Martinez', 1, 11, 11,
         DATEADD(year, -27, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 32 # 27-42', NULL, '3001000022', 'ciudadano.prueba23@example.com', 0, DATEADD(day, -10, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 17, 2, 1, 3, DATEADD(day, -9, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -9, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 17', DATEADD(year, -2, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -9, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -9, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -9, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/23/1.pdf', 'application/pdf', 1024, DATEADD(day, -9, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/23/2.pdf', 'application/pdf', 1024, DATEADD(day, -9, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/23/3.pdf', 'application/pdf', 1024, DATEADD(day, -9, SYSUTCDATETIME()));

-- Registro 24: Camila Suarez Gonzalez - Etapa Inventario
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-24', 1, '900500024', 'Valledupar', 2,
         'Camila', 'Suarez Gonzalez', 1, 11, 11,
         DATEADD(year, -51, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 33 # 28-43', NULL, '3001000023', 'ciudadano.prueba24@example.com', 0, DATEADD(day, -11, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 17, 2, 1, 4, DATEADD(day, -10, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -10, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 17', DATEADD(year, -2, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -10, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -10, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -10, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 4, 'seed-script', 'Aprobada', DATEADD(day, -10, DATEADD(hour, 3, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/24/1.pdf', 'application/pdf', 1024, DATEADD(day, -10, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/24/2.pdf', 'application/pdf', 1024, DATEADD(day, -10, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/24/3.pdf', 'application/pdf', 1024, DATEADD(day, -10, SYSUTCDATETIME()));

-- Registro 25: Diego Vargas Rojas - Etapa1
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-25', 1, '900500025', 'Valledupar', 1,
         'Diego', 'Vargas Rojas', 1, 11, 11,
         DATEADD(year, -40, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 34 # 29-44', NULL, '3001000024', 'ciudadano.prueba25@example.com', 0, DATEADD(day, -12, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 12, 2, 1, 1, DATEADD(day, -11, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -11, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 12', DATEADD(year, -1, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -11, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/25/1.pdf', 'application/pdf', 1024, DATEADD(day, -11, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/25/2.pdf', 'application/pdf', 1024, DATEADD(day, -11, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/25/3.pdf', 'application/pdf', 1024, DATEADD(day, -11, SYSUTCDATETIME()));

-- Registro 26: Isabella Molina Garcia - Etapa2
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-26', 1, '900500026', 'Valledupar', 2,
         'Isabella', 'Molina Garcia', 1, 11, 11,
         DATEADD(year, -41, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 35 # 30-45', NULL, '3001000025', 'ciudadano.prueba26@example.com', 0, DATEADD(day, -13, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 15, 2, 1, 2, DATEADD(day, -12, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -12, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 15', DATEADD(year, -6, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -12, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -12, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/26/1.pdf', 'application/pdf', 1024, DATEADD(day, -12, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/26/2.pdf', 'application/pdf', 1024, DATEADD(day, -12, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/26/3.pdf', 'application/pdf', 1024, DATEADD(day, -12, SYSUTCDATETIME()));

-- Registro 27: Luis Ramirez Castro - Etapa3
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-27', 1, '900500027', 'Valledupar', 1,
         'Luis', 'Ramirez Castro', 1, 11, 11,
         DATEADD(year, -53, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 36 # 31-46', NULL, '3001000026', 'ciudadano.prueba27@example.com', 0, DATEADD(day, -1, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 7, 2, 1, 3, DATEADD(day, -0, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -0, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 7', DATEADD(year, -1, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -0, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -0, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -0, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/27/1.pdf', 'application/pdf', 1024, DATEADD(day, -0, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/27/2.pdf', 'application/pdf', 1024, DATEADD(day, -0, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/27/3.pdf', 'application/pdf', 1024, DATEADD(day, -0, SYSUTCDATETIME()));

-- Registro 28: Gabriela Reyes Molina - Etapa Inventario
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-28', 1, '900500028', 'Valledupar', 2,
         'Gabriela', 'Reyes Molina', 1, 11, 11,
         DATEADD(year, -35, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 37 # 32-47', NULL, '3001000027', 'ciudadano.prueba28@example.com', 0, DATEADD(day, -2, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 9, 2, 1, 4, DATEADD(day, -1, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -1, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 9', DATEADD(year, -2, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -1, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -1, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -1, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 4, 'seed-script', 'Aprobada', DATEADD(day, -1, DATEADD(hour, 3, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/28/1.pdf', 'application/pdf', 1024, DATEADD(day, -1, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/28/2.pdf', 'application/pdf', 1024, DATEADD(day, -1, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/28/3.pdf', 'application/pdf', 1024, DATEADD(day, -1, SYSUTCDATETIME()));

-- Registro 29: Mateo Suarez Ramirez - Etapa1
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-29', 1, '900500029', 'Valledupar', 1,
         'Mateo', 'Suarez Ramirez', 1, 11, 11,
         DATEADD(year, -36, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 38 # 33-48', NULL, '3001000028', 'ciudadano.prueba29@example.com', 0, DATEADD(day, -3, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 18, 2, 1, 1, DATEADD(day, -2, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -2, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 18', DATEADD(year, -3, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -2, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/29/1.pdf', 'application/pdf', 1024, DATEADD(day, -2, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/29/2.pdf', 'application/pdf', 1024, DATEADD(day, -2, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/29/3.pdf', 'application/pdf', 1024, DATEADD(day, -2, SYSUTCDATETIME()));

-- Registro 30: Andrea Medina Gonzalez - Etapa2
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-30', 1, '900500030', 'Valledupar', 2,
         'Andrea', 'Medina Gonzalez', 1, 11, 11,
         DATEADD(year, -24, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 39 # 34-49', NULL, '3001000029', 'ciudadano.prueba30@example.com', 0, DATEADD(day, -4, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 13, 2, 1, 2, DATEADD(day, -3, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -3, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 13', DATEADD(year, -6, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -3, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -3, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/30/1.pdf', 'application/pdf', 1024, DATEADD(day, -3, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/30/2.pdf', 'application/pdf', 1024, DATEADD(day, -3, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/30/3.pdf', 'application/pdf', 1024, DATEADD(day, -3, SYSUTCDATETIME()));

-- Registro 31: Santiago Gomez Garcia - Etapa3
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-31', 1, '900500031', 'Valledupar', 1,
         'Santiago', 'Gomez Garcia', 1, 11, 11,
         DATEADD(year, -47, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 40 # 35-50', NULL, '3001000030', 'ciudadano.prueba31@example.com', 0, DATEADD(day, -5, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 19, 2, 1, 3, DATEADD(day, -4, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -4, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 19', DATEADD(year, -5, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -4, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -4, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -4, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/31/1.pdf', 'application/pdf', 1024, DATEADD(day, -4, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/31/2.pdf', 'application/pdf', 1024, DATEADD(day, -4, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/31/3.pdf', 'application/pdf', 1024, DATEADD(day, -4, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 4, 'Tarjeta profesional.pdf', 'seed/31/4.pdf', 'application/pdf', 1024, DATEADD(day, -4, SYSUTCDATETIME()));

-- Registro 32: Valentina Rodriguez Perez - Etapa Inventario
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-32', 1, '900500032', 'Valledupar', 2,
         'Valentina', 'Rodriguez Perez', 1, 11, 11,
         DATEADD(year, -29, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 41 # 36-51', NULL, '3001000031', 'ciudadano.prueba32@example.com', 0, DATEADD(day, -6, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 19, 2, 1, 4, DATEADD(day, -5, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -5, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 19', DATEADD(year, -2, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -5, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -5, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -5, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 4, 'seed-script', 'Aprobada', DATEADD(day, -5, DATEADD(hour, 3, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/32/1.pdf', 'application/pdf', 1024, DATEADD(day, -5, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/32/2.pdf', 'application/pdf', 1024, DATEADD(day, -5, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/32/3.pdf', 'application/pdf', 1024, DATEADD(day, -5, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 4, 'Tarjeta profesional.pdf', 'seed/32/4.pdf', 'application/pdf', 1024, DATEADD(day, -5, SYSUTCDATETIME()));

-- Registro 33: Daniel Rincon Guerrero - Etapa1
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-33', 1, '900500033', 'Valledupar', 1,
         'Daniel', 'Rincon Guerrero', 1, 11, 11,
         DATEADD(year, -32, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 42 # 37-52', NULL, '3001000032', 'ciudadano.prueba33@example.com', 0, DATEADD(day, -7, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 6, 2, 1, 1, DATEADD(day, -6, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -6, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 6', DATEADD(year, -3, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -6, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/33/1.pdf', 'application/pdf', 1024, DATEADD(day, -6, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/33/2.pdf', 'application/pdf', 1024, DATEADD(day, -6, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/33/3.pdf', 'application/pdf', 1024, DATEADD(day, -6, SYSUTCDATETIME()));

-- Registro 34: Isabella Rincon Moreno - Etapa2
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-34', 1, '900500034', 'Valledupar', 2,
         'Isabella', 'Rincon Moreno', 1, 11, 11,
         DATEADD(year, -40, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 43 # 38-53', NULL, '3001000033', 'ciudadano.prueba34@example.com', 0, DATEADD(day, -8, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 7, 2, 1, 2, DATEADD(day, -7, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -7, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 7', DATEADD(year, -7, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -7, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -7, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/34/1.pdf', 'application/pdf', 1024, DATEADD(day, -7, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/34/2.pdf', 'application/pdf', 1024, DATEADD(day, -7, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/34/3.pdf', 'application/pdf', 1024, DATEADD(day, -7, SYSUTCDATETIME()));

-- Registro 35: Andres Cardenas Moreno - Etapa3
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-35', 1, '900500035', 'Valledupar', 1,
         'Andres', 'Cardenas Moreno', 1, 11, 11,
         DATEADD(year, -36, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 44 # 39-54', NULL, '3001000034', 'ciudadano.prueba35@example.com', 0, DATEADD(day, -9, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 17, 2, 1, 3, DATEADD(day, -8, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -8, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 17', DATEADD(year, -5, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -8, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -8, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -8, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/35/1.pdf', 'application/pdf', 1024, DATEADD(day, -8, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/35/2.pdf', 'application/pdf', 1024, DATEADD(day, -8, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/35/3.pdf', 'application/pdf', 1024, DATEADD(day, -8, SYSUTCDATETIME()));

-- Registro 36: Paula Romero Suarez - Etapa Inventario
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-36', 1, '900500036', 'Valledupar', 2,
         'Paula', 'Romero Suarez', 1, 11, 11,
         DATEADD(year, -52, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 45 # 40-55', NULL, '3001000035', 'ciudadano.prueba36@example.com', 0, DATEADD(day, -10, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 10, 2, 1, 4, DATEADD(day, -9, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -9, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 10', DATEADD(year, -8, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -9, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -9, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -9, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 4, 'seed-script', 'Aprobada', DATEADD(day, -9, DATEADD(hour, 3, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/36/1.pdf', 'application/pdf', 1024, DATEADD(day, -9, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/36/2.pdf', 'application/pdf', 1024, DATEADD(day, -9, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/36/3.pdf', 'application/pdf', 1024, DATEADD(day, -9, SYSUTCDATETIME()));

-- Registro 37: Luis Perez Perez - Etapa1
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-37', 1, '900500037', 'Valledupar', 1,
         'Luis', 'Perez Perez', 1, 11, 11,
         DATEADD(year, -45, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 46 # 41-56', NULL, '3001000036', 'ciudadano.prueba37@example.com', 0, DATEADD(day, -11, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 6, 2, 1, 1, DATEADD(day, -10, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -10, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 6', DATEADD(year, -1, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -10, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/37/1.pdf', 'application/pdf', 1024, DATEADD(day, -10, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/37/2.pdf', 'application/pdf', 1024, DATEADD(day, -10, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/37/3.pdf', 'application/pdf', 1024, DATEADD(day, -10, SYSUTCDATETIME()));

-- Registro 38: Sofia Moreno Perez - Etapa2
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-38', 1, '900500038', 'Valledupar', 2,
         'Sofia', 'Moreno Perez', 1, 11, 11,
         DATEADD(year, -38, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 47 # 42-57', NULL, '3001000037', 'ciudadano.prueba38@example.com', 0, DATEADD(day, -12, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 14, 2, 1, 2, DATEADD(day, -11, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -11, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 14', DATEADD(year, -1, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -11, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -11, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/38/1.pdf', 'application/pdf', 1024, DATEADD(day, -11, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/38/2.pdf', 'application/pdf', 1024, DATEADD(day, -11, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/38/3.pdf', 'application/pdf', 1024, DATEADD(day, -11, SYSUTCDATETIME()));

-- Registro 39: Luis Alvarez Suarez - Etapa3
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-39', 1, '900500039', 'Valledupar', 1,
         'Luis', 'Alvarez Suarez', 1, 11, 11,
         DATEADD(year, -38, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 48 # 43-58', NULL, '3001000038', 'ciudadano.prueba39@example.com', 0, DATEADD(day, -13, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 5, 2, 1, 3, DATEADD(day, -12, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -12, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 5', DATEADD(year, -2, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -12, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -12, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -12, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/39/1.pdf', 'application/pdf', 1024, DATEADD(day, -12, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/39/2.pdf', 'application/pdf', 1024, DATEADD(day, -12, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/39/3.pdf', 'application/pdf', 1024, DATEADD(day, -12, SYSUTCDATETIME()));

-- Registro 40: Alejandra Rodriguez Pena - Etapa Inventario
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-40', 1, '900500040', 'Valledupar', 2,
         'Alejandra', 'Rodriguez Pena', 1, 11, 11,
         DATEADD(year, -28, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 49 # 44-59', NULL, '3001000039', 'ciudadano.prueba40@example.com', 0, DATEADD(day, -1, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 10, 2, 1, 4, DATEADD(day, -0, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -0, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 10', DATEADD(year, -4, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -0, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -0, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -0, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 4, 'seed-script', 'Aprobada', DATEADD(day, -0, DATEADD(hour, 3, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/40/1.pdf', 'application/pdf', 1024, DATEADD(day, -0, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/40/2.pdf', 'application/pdf', 1024, DATEADD(day, -0, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/40/3.pdf', 'application/pdf', 1024, DATEADD(day, -0, SYSUTCDATETIME()));

-- Registro 41: Juan Romero Rincon - Etapa1
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-41', 1, '900500041', 'Valledupar', 1,
         'Juan', 'Romero Rincon', 1, 11, 11,
         DATEADD(year, -32, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 50 # 45-60', NULL, '3001000040', 'ciudadano.prueba41@example.com', 0, DATEADD(day, -2, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 8, 2, 1, 1, DATEADD(day, -1, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -1, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 8', DATEADD(year, -8, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -1, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/41/1.pdf', 'application/pdf', 1024, DATEADD(day, -1, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/41/2.pdf', 'application/pdf', 1024, DATEADD(day, -1, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/41/3.pdf', 'application/pdf', 1024, DATEADD(day, -1, SYSUTCDATETIME()));

-- Registro 42: Valentina Cortes Rincon - Etapa2
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-42', 1, '900500042', 'Valledupar', 2,
         'Valentina', 'Cortes Rincon', 1, 11, 11,
         DATEADD(year, -50, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 51 # 46-61', NULL, '3001000041', 'ciudadano.prueba42@example.com', 0, DATEADD(day, -3, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 17, 2, 1, 2, DATEADD(day, -2, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -2, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 17', DATEADD(year, -4, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -2, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -2, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/42/1.pdf', 'application/pdf', 1024, DATEADD(day, -2, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/42/2.pdf', 'application/pdf', 1024, DATEADD(day, -2, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/42/3.pdf', 'application/pdf', 1024, DATEADD(day, -2, SYSUTCDATETIME()));

-- Registro 43: Luis Garcia Romero - Etapa3
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-43', 1, '900500043', 'Valledupar', 1,
         'Luis', 'Garcia Romero', 1, 11, 11,
         DATEADD(year, -46, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 52 # 47-62', NULL, '3001000042', 'ciudadano.prueba43@example.com', 0, DATEADD(day, -4, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 11, 2, 1, 3, DATEADD(day, -3, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -3, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 11', DATEADD(year, -7, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -3, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -3, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -3, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/43/1.pdf', 'application/pdf', 1024, DATEADD(day, -3, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/43/2.pdf', 'application/pdf', 1024, DATEADD(day, -3, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/43/3.pdf', 'application/pdf', 1024, DATEADD(day, -3, SYSUTCDATETIME()));

-- Registro 44: Paula Ortiz Pena - Etapa Inventario
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-44', 1, '900500044', 'Valledupar', 2,
         'Paula', 'Ortiz Pena', 1, 11, 11,
         DATEADD(year, -27, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 53 # 48-63', NULL, '3001000043', 'ciudadano.prueba44@example.com', 0, DATEADD(day, -5, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 16, 2, 1, 4, DATEADD(day, -4, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -4, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 16', DATEADD(year, -2, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -4, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -4, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -4, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 4, 'seed-script', 'Aprobada', DATEADD(day, -4, DATEADD(hour, 3, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/44/1.pdf', 'application/pdf', 1024, DATEADD(day, -4, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/44/2.pdf', 'application/pdf', 1024, DATEADD(day, -4, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/44/3.pdf', 'application/pdf', 1024, DATEADD(day, -4, SYSUTCDATETIME()));

-- Registro 45: Jose Vargas Molina - Etapa1
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-45', 1, '900500045', 'Valledupar', 1,
         'Jose', 'Vargas Molina', 1, 11, 11,
         DATEADD(year, -30, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 54 # 49-64', NULL, '3001000044', 'ciudadano.prueba45@example.com', 0, DATEADD(day, -6, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 10, 2, 1, 1, DATEADD(day, -5, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -5, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 10', DATEADD(year, -4, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -5, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/45/1.pdf', 'application/pdf', 1024, DATEADD(day, -5, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/45/2.pdf', 'application/pdf', 1024, DATEADD(day, -5, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/45/3.pdf', 'application/pdf', 1024, DATEADD(day, -5, SYSUTCDATETIME()));

-- Registro 46: Valentina Hernandez Moreno - Etapa2
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-46', 1, '900500046', 'Valledupar', 2,
         'Valentina', 'Hernandez Moreno', 1, 11, 11,
         DATEADD(year, -32, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 55 # 50-65', NULL, '3001000045', 'ciudadano.prueba46@example.com', 0, DATEADD(day, -7, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 12, 2, 1, 2, DATEADD(day, -6, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -6, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 12', DATEADD(year, -7, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -6, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -6, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/46/1.pdf', 'application/pdf', 1024, DATEADD(day, -6, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/46/2.pdf', 'application/pdf', 1024, DATEADD(day, -6, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/46/3.pdf', 'application/pdf', 1024, DATEADD(day, -6, SYSUTCDATETIME()));

-- Registro 47: Carlos Sanchez Ortiz - Etapa3
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-47', 1, '900500047', 'Valledupar', 1,
         'Carlos', 'Sanchez Ortiz', 1, 11, 11,
         DATEADD(year, -28, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 56 # 51-66', NULL, '3001000046', 'ciudadano.prueba47@example.com', 0, DATEADD(day, -8, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 8, 2, 1, 3, DATEADD(day, -7, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -7, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 8', DATEADD(year, -8, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -7, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -7, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -7, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/47/1.pdf', 'application/pdf', 1024, DATEADD(day, -7, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/47/2.pdf', 'application/pdf', 1024, DATEADD(day, -7, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/47/3.pdf', 'application/pdf', 1024, DATEADD(day, -7, SYSUTCDATETIME()));

-- Registro 48: Mariana Pena Pena - Etapa Inventario
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-48', 1, '900500048', 'Valledupar', 2,
         'Mariana', 'Pena Pena', 1, 11, 11,
         DATEADD(year, -30, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 57 # 52-67', NULL, '3001000047', 'ciudadano.prueba48@example.com', 0, DATEADD(day, -9, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 13, 2, 1, 4, DATEADD(day, -8, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -8, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 13', DATEADD(year, -1, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -8, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -8, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -8, DATEADD(hour, 2, SYSUTCDATETIME())));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 4, 'seed-script', 'Aprobada', DATEADD(day, -8, DATEADD(hour, 3, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/48/1.pdf', 'application/pdf', 1024, DATEADD(day, -8, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/48/2.pdf', 'application/pdf', 1024, DATEADD(day, -8, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/48/3.pdf', 'application/pdf', 1024, DATEADD(day, -8, SYSUTCDATETIME()));

-- Registro 49: Alejandro Moreno Guerrero - Etapa1
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-49', 1, '900500049', 'Valledupar', 1,
         'Alejandro', 'Moreno Guerrero', 1, 11, 11,
         DATEADD(year, -29, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 58 # 53-68', NULL, '3001000048', 'ciudadano.prueba49@example.com', 0, DATEADD(day, -10, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 5, 2, 1, 1, DATEADD(day, -9, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -9, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 5', DATEADD(year, -4, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -9, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/49/1.pdf', 'application/pdf', 1024, DATEADD(day, -9, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/49/2.pdf', 'application/pdf', 1024, DATEADD(day, -9, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/49/3.pdf', 'application/pdf', 1024, DATEADD(day, -9, SYSUTCDATETIME()));

-- Registro 50: Camila Castro Rincon - Etapa2
INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-50', 1, '900500050', 'Valledupar', 2,
         'Camila', 'Castro Rincon', 1, 11, 11,
         DATEADD(year, -37, SYSUTCDATETIME()), 1, 11, 11,
         'Calle 59 # 54-69', NULL, '3001000049', 'ciudadano.prueba50@example.com', 0, DATEADD(day, -11, SYSUTCDATETIME()));
SET @sid = SCOPE_IDENTITY();
INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, 12, 2, 1, 2, DATEADD(day, -10, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -10, SYSUTCDATETIME()));
SET @solid = SCOPE_IDENTITY();
INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa 12', DATEADD(year, -7, SYSUTCDATETIME()), NULL, NULL, NULL);
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -10, SYSUTCDATETIME()));
INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -10, DATEADD(hour, 1, SYSUTCDATETIME())));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 1, 'Cedula ampliada al 150%.pdf', 'seed/50/1.pdf', 'application/pdf', 1024, DATEADD(day, -10, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 2, 'Diploma de grado.pdf', 'seed/50/2.pdf', 'application/pdf', 1024, DATEADD(day, -10, SYSUTCDATETIME()));
INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, 3, 'Acta de grado.pdf', 'seed/50/3.pdf', 'application/pdf', 1024, DATEADD(day, -10, SYSUTCDATETIME()));

COMMIT TRANSACTION;
SELECT COUNT(*) AS TotalCreadas FROM Solicitantes WHERE ApplicationUserId LIKE 'seed-%';