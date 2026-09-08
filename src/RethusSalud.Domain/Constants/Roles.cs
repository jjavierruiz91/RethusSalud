namespace RethusSalud.Domain.Constants;

public static class Roles
{
    public const string SuperAdmin = "SuperAdmin";
    public const string FuncionarioEtapa1 = "FuncionarioEtapa1";
    public const string FuncionarioEtapa2 = "FuncionarioEtapa2";
    public const string FuncionarioEtapa3 = "FuncionarioEtapa3";
    public const string Inventario = "Inventario";
    public const string Ciudadano = "Ciudadano";

    public static readonly IReadOnlyList<string> All = new[]
    {
        SuperAdmin, FuncionarioEtapa1, FuncionarioEtapa2, FuncionarioEtapa3, Inventario, Ciudadano
    };
}
