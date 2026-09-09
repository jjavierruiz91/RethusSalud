using Microsoft.EntityFrameworkCore;
using RethusSalud.Domain.Entities;
using RethusSalud.Domain.Enums;

namespace RethusSalud.Infrastructure.Persistence.Seed;

public static class CatalogoSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await SeedGeografiaAsync(context);
        await SeedProfesionesAsync(context);
    }

    private static async Task SeedGeografiaAsync(ApplicationDbContext context)
    {
        if (await context.Paises.AnyAsync())
        {
            return;
        }

        var colombia = new Pais { Nombre = "Colombia" };
        context.Paises.Add(colombia);
        await context.SaveChangesAsync();

        // Municipios completos para Cesar (departamento donde opera esta Secretaria de Salud);
        // el resto de departamentos solo trae su capital como punto de partida.
        // La lista nacional completa de municipios (DANE) queda pendiente como tarea de datos, no de logica.
        (string Nombre, string[] Municipios)[] departamentos =
        {
            ("Amazonas", new[] { "Leticia" }),
            ("Antioquia", new[] { "Medellin" }),
            ("Arauca", new[] { "Arauca" }),
            ("Atlantico", new[] { "Barranquilla" }),
            ("Bolivar", new[] { "Cartagena de Indias" }),
            ("Boyaca", new[] { "Tunja" }),
            ("Caldas", new[] { "Manizales" }),
            ("Caqueta", new[] { "Florencia" }),
            ("Casanare", new[] { "Yopal" }),
            ("Cauca", new[] { "Popayan" }),
            ("Cesar", new[]
            {
                "Valledupar", "Aguachica", "Agustin Codazzi", "Astrea", "Becerril", "Bosconia",
                "Chimichagua", "Chiriguana", "Curumani", "El Copey", "El Paso", "Gamarra",
                "Gonzalez", "La Gloria", "La Jagua de Ibirico", "Manaure Balcon del Cesar",
                "Pailitas", "Pelaya", "Pueblo Bello", "Rio de Oro", "La Paz", "San Alberto",
                "San Diego", "San Martin", "Tamalameque"
            }),
            ("Choco", new[] { "Quibdo" }),
            ("Cordoba", new[] { "Monteria" }),
            ("Cundinamarca", new[] { "Zipaquira" }),
            ("Guainia", new[] { "Inirida" }),
            ("Guaviare", new[] { "San Jose del Guaviare" }),
            ("Huila", new[] { "Neiva" }),
            ("La Guajira", new[] { "Riohacha" }),
            ("Magdalena", new[] { "Santa Marta" }),
            ("Meta", new[] { "Villavicencio" }),
            ("Narino", new[] { "Pasto" }),
            ("Norte de Santander", new[] { "Cucuta" }),
            ("Putumayo", new[] { "Mocoa" }),
            ("Quindio", new[] { "Armenia" }),
            ("Risaralda", new[] { "Pereira" }),
            ("San Andres y Providencia", new[] { "San Andres" }),
            ("Santander", new[] { "Bucaramanga" }),
            ("Sucre", new[] { "Sincelejo" }),
            ("Tolima", new[] { "Ibague" }),
            ("Valle del Cauca", new[] { "Cali" }),
            ("Vaupes", new[] { "Mitu" }),
            ("Vichada", new[] { "Puerto Carreno" }),
            ("Bogota D.C.", new[] { "Bogota D.C." })
        };

        foreach (var (nombreDepto, municipios) in departamentos)
        {
            var departamento = new Departamento { Nombre = nombreDepto, PaisId = colombia.Id };
            context.Departamentos.Add(departamento);
            await context.SaveChangesAsync();

            context.Municipios.AddRange(municipios.Select(nombreMunicipio => new Municipio
            {
                Nombre = nombreMunicipio,
                DepartamentoId = departamento.Id
            }));
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedProfesionesAsync(ApplicationDbContext context)
    {
        if (await context.Profesiones.AnyAsync())
        {
            return;
        }

        var profesiones = new List<Profesion>
        {
            new() { Nombre = "Auxiliar administrativo en salud", TipoTramite = TipoTramite.Rethus, NivelFormacion = NivelFormacion.Auxiliar },
            new() { Nombre = "Auxiliar en enfermeria", TipoTramite = TipoTramite.Rethus, NivelFormacion = NivelFormacion.Auxiliar },
            new() { Nombre = "Auxiliar en salud oral", TipoTramite = TipoTramite.Rethus, NivelFormacion = NivelFormacion.Auxiliar },
            new() { Nombre = "Auxiliar en salud publica", TipoTramite = TipoTramite.Rethus, NivelFormacion = NivelFormacion.Auxiliar },
            new() { Nombre = "Auxiliar en servicios farmaceuticos", TipoTramite = TipoTramite.Rethus, NivelFormacion = NivelFormacion.Auxiliar },

            new() { Nombre = "Tecnico profesional en atencion prehospitalaria", TipoTramite = TipoTramite.Rethus, NivelFormacion = NivelFormacion.TecnicoProfesional },
            new() { Nombre = "Tecnico profesional en citologia", TipoTramite = TipoTramite.Rethus, NivelFormacion = NivelFormacion.TecnicoProfesional },

            new() { Nombre = "Tecnologia en atencion prehospitalaria", TipoTramite = TipoTramite.Rethus, NivelFormacion = NivelFormacion.Tecnologo },
            new() { Nombre = "Tecnologia en citohistologia", TipoTramite = TipoTramite.Rethus, NivelFormacion = NivelFormacion.Tecnologo },
            new() { Nombre = "Tecnologia en regencia en farmacia", TipoTramite = TipoTramite.Rethus, NivelFormacion = NivelFormacion.Tecnologo },
            new() { Nombre = "Tecnologia en manejo de fuentes abiertas de uso diagnostico y terapeutico", TipoTramite = TipoTramite.Rethus, NivelFormacion = NivelFormacion.Tecnologo },
            new() { Nombre = "Tecnologia en radiodiagnostico y radioterapia", TipoTramite = TipoTramite.Rethus, NivelFormacion = NivelFormacion.Tecnologo },
            new() { Nombre = "Tecnologia en radiologia e imagenes diagnosticas", TipoTramite = TipoTramite.Rethus, NivelFormacion = NivelFormacion.Tecnologo },
            new() { Nombre = "Tecnologia en radioterapia", TipoTramite = TipoTramite.Rethus, NivelFormacion = NivelFormacion.Tecnologo },

            new() { Nombre = "Psicologia", TipoTramite = TipoTramite.Rethus, NivelFormacion = NivelFormacion.Profesional }
        };

        context.Profesiones.AddRange(profesiones);
        await context.SaveChangesAsync();
    }
}
