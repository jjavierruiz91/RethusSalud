using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RethusSalud.Domain.Constants;

namespace RethusSalud.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        bool seedDemoStaff)
    {
        foreach (var role in Roles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        await SeedSuperAdminAsync(userManager, configuration);

        if (seedDemoStaff)
        {
            await SeedFuncionariosDemoAsync(userManager);
        }
    }

    private static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        var adminEmail = configuration["SeedAdmin:Email"];
        var adminPassword = configuration["SeedAdmin:Password"];

        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
        {
            return;
        }

        if (await userManager.FindByEmailAsync(adminEmail) is not null)
        {
            return;
        }

        var admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            NombreCompleto = "Sistemas administrador",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(admin, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, Roles.SuperAdmin);
        }
    }

    private static async Task SeedFuncionariosDemoAsync(UserManager<ApplicationUser> userManager)
    {
        var funcionariosDemo = new (string Email, string Rol, string Nombre)[]
        {
            ("etapa1@rethus.local", Roles.FuncionarioEtapa1, "Funcionario etapa 1 (demo)"),
            ("etapa2@rethus.local", Roles.FuncionarioEtapa2, "Funcionario etapa 2 (demo)"),
            ("etapa3@rethus.local", Roles.FuncionarioEtapa3, "Funcionario etapa 3 (demo)"),
            ("inventario@rethus.local", Roles.Inventario, "Funcionario inventario (demo)")
        };

        foreach (var (email, rol, nombre) in funcionariosDemo)
        {
            if (await userManager.FindByEmailAsync(email) is not null)
            {
                continue;
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                NombreCompleto = nombre,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, "Demo123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, rol);
            }
        }
    }
}
