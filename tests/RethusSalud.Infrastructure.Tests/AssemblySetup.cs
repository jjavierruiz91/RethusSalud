using System.Runtime.CompilerServices;
using QuestPDF.Infrastructure;

namespace RethusSalud.Infrastructure.Tests;

internal static class AssemblySetup
{
    [ModuleInitializer]
    internal static void Inicializar()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }
}
