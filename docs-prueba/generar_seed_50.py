# -*- coding: utf-8 -*-
"""Genera un script SQL con 50 solicitudes de prueba distribuidas en las 4 etapas."""

import random

random.seed(42)

NOMBRES_M = ["Jose", "Luis", "Carlos", "Andres", "Juan", "Miguel", "Diego", "Santiago",
             "Camilo", "Sebastian", "Alejandro", "Daniel", "Felipe", "Mateo", "Nicolas"]
NOMBRES_F = ["Maria", "Laura", "Camila", "Valentina", "Daniela", "Andrea", "Paula",
             "Natalia", "Juliana", "Sofia", "Isabella", "Gabriela", "Mariana", "Carolina", "Alejandra"]
APELLIDOS = ["Gomez", "Rodriguez", "Martinez", "Garcia", "Lopez", "Gonzalez", "Hernandez",
             "Perez", "Sanchez", "Ramirez", "Torres", "Florez", "Vargas", "Castro", "Ortiz",
             "Rincon", "Jimenez", "Moreno", "Munoz", "Rojas", "Suarez", "Romero", "Alvarez",
             "Molina", "Medina", "Cortes", "Guerrero", "Pena", "Reyes", "Cardenas"]

PROFESIONES = list(range(5, 20))  # Ids 5..19
PSICOLOGIA_ID = 19

ETAPAS = [1, 2, 3, 4]  # Etapa1, Etapa2, Etapa3, Inventario

lines = []
lines.append("SET NOCOUNT ON;")
lines.append("BEGIN TRANSACTION;")
lines.append("DECLARE @sid INT, @solid INT;")

for i in range(50):
    es_hombre = i % 2 == 0
    nombre = random.choice(NOMBRES_M if es_hombre else NOMBRES_F)
    apellido1 = random.choice(APELLIDOS)
    apellido2 = random.choice(APELLIDOS)
    genero = 1 if es_hombre else 2
    cedula = 900500001 + i
    celular = 3001000000 + i
    correo = f"ciudadano.prueba{i+1}@example.com"
    etapa = ETAPAS[i % 4]
    profesion_id = random.choice(PROFESIONES)
    dias_atras = i % 13  # 0..12 dias, variedad para "esperando X dias"
    edad_anios = random.randint(24, 55)

    lines.append(f"-- Registro {i+1}: {nombre} {apellido1} {apellido2} - Etapa{etapa if etapa < 4 else ' Inventario'}")
    lines.append(f"""INSERT INTO Solicitantes
        (ApplicationUserId, TipoIdentificacion, NumeroIdentificacion, LugarExpedicion, Genero,
         Nombres, Apellidos, PaisNacimientoId, DepartamentoNacimientoId, MunicipioNacimientoId,
         FechaNacimiento, PaisResidenciaId, DepartamentoResidenciaId, MunicipioResidenciaId,
         DireccionDomicilio, TelefonoFijo, Celular, CorreoElectronico, GrupoEtnico, FechaAceptacionTerminos)
    VALUES
        ('seed-{i+1}', 1, '{cedula}', 'Valledupar', {genero},
         '{nombre}', '{apellido1} {apellido2}', 1, 11, 11,
         DATEADD(year, -{edad_anios}, SYSUTCDATETIME()), 1, 11, 11,
         'Calle {10+i} # {5+i}-{20+i}', NULL, '{celular}', '{correo}', 0, DATEADD(day, -{dias_atras+1}, SYSUTCDATETIME()));""")
    lines.append("SET @sid = SCOPE_IDENTITY();")

    lines.append(f"""INSERT INTO Solicitudes
        (SolicitanteId, ProfesionId, TipoTramite, Estado, EtapaActual, FechaCreacion, FechaActualizacion, FechaRadicacion)
    VALUES
        (@sid, {profesion_id}, 2, 1, {etapa}, DATEADD(day, -{dias_atras}, SYSUTCDATETIME()), SYSUTCDATETIME(), DATEADD(day, -{dias_atras}, SYSUTCDATETIME()));""")
    lines.append("SET @solid = SCOPE_IDENTITY();")

    lines.append(f"""INSERT INTO DatosAcademicos
        (SolicitudId, OrigenTitulo, TipoInstitucion, TipoPrograma, PaisInstitucionId, DepartamentoInstitucionId, MunicipioInstitucionId,
         NombreInstitucion, NombrePrograma, FechaGrado, NumeroConvalidacion, FechaConvalidacion, TituloEquivalente)
    VALUES
        (@solid, 1, 1, 'Universitario', 1, 11, 11,
         'Universidad Popular del Cesar', 'Programa {profesion_id}', DATEADD(year, -{random.randint(1,8)}, SYSUTCDATETIME()), NULL, NULL, NULL);""")

    lines.append(f"""INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 1, 'seed-script', 'Solicitud radicada', DATEADD(day, -{dias_atras}, SYSUTCDATETIME()));""")
    if etapa >= 2:
        lines.append(f"""INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 2, 'seed-script', 'Aprobada', DATEADD(day, -{dias_atras}, DATEADD(hour, 1, SYSUTCDATETIME())));""")
    if etapa >= 3:
        lines.append(f"""INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 3, 'seed-script', 'Aprobada', DATEADD(day, -{dias_atras}, DATEADD(hour, 2, SYSUTCDATETIME())));""")
    if etapa >= 4:
        lines.append(f"""INSERT INTO HistorialEstados (SolicitudId, EstadoResultante, EtapaResultante, UsuarioId, Motivo, Fecha)
    VALUES (@solid, 1, 4, 'seed-script', 'Aprobada', DATEADD(day, -{dias_atras}, DATEADD(hour, 3, SYSUTCDATETIME())));""")

    documentos = [(1, "Cedula ampliada al 150%.pdf"), (2, "Diploma de grado.pdf"), (3, "Acta de grado.pdf")]
    if profesion_id == PSICOLOGIA_ID:
        documentos.append((4, "Tarjeta profesional.pdf"))

    for tipo_doc, nombre_archivo in documentos:
        lines.append(f"""INSERT INTO ArchivosAdjuntos (SolicitudId, TipoDocumento, NombreArchivo, RutaAlmacenamiento, ContentType, TamanoBytes, FechaCarga)
    VALUES (@solid, {tipo_doc}, '{nombre_archivo}', 'seed/{{0}}/{tipo_doc}.pdf', 'application/pdf', 1024, DATEADD(day, -{dias_atras}, SYSUTCDATETIME()));""".replace("{0}", str(i+1)))

    lines.append("")

lines.append("COMMIT TRANSACTION;")
lines.append("SELECT COUNT(*) AS TotalCreadas FROM Solicitantes WHERE ApplicationUserId LIKE 'seed-%';")

with open("seed_50.sql", "w", encoding="utf-8") as f:
    f.write("\n".join(lines))

print("Generado seed_50.sql con", lines.count("COMMIT TRANSACTION;"), "transaccion(es)")
