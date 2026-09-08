using System.Data;
using Microsoft.EntityFrameworkCore;
using RethusSalud.Application.Interfaces;
using RethusSalud.Domain.Enums;
using RethusSalud.Infrastructure.Persistence;

namespace RethusSalud.Infrastructure.Services;

public class SqlConsecutivoGenerator : IConsecutivoGenerator
{
    private readonly ApplicationDbContext _context;

    public SqlConsecutivoGenerator(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerarSiguienteAsync(TipoTramite tipoTramite)
    {
        var connection = _context.Database.GetDbConnection();
        var abrioAqui = connection.State != ConnectionState.Open;
        if (abrioAqui)
        {
            await connection.OpenAsync();
        }

        try
        {
            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT NEXT VALUE FOR ConsecutivoSeq";
            var resultado = await command.ExecuteScalarAsync();
            var siguiente = Convert.ToInt32(resultado);

            return $"RTH-{DateTime.UtcNow.Year}-{siguiente:D6}";
        }
        finally
        {
            if (abrioAqui)
            {
                await connection.CloseAsync();
            }
        }
    }
}
