using System.Collections.Concurrent;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using RethusSalud.Application;
using RethusSalud.Infrastructure;
using RethusSalud.Infrastructure.Identity;
using RethusSalud.Infrastructure.Persistence;
using RethusSalud.Infrastructure.Persistence.Seed;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/rethus-.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddScoped<RethusSalud.Web.Services.FirmantesService>();

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

    builder.Services.AddRateLimiter(options =>
    {
        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

        var ventana = TimeSpan.FromMinutes(1);
        var inicioVentanaPorCliente = new ConcurrentDictionary<string, DateTimeOffset>();

        options.OnRejected = (contexto, _) =>
        {
            var clave = $"{contexto.HttpContext.Request.Path}:{ClienteId(contexto.HttpContext)}";
            var ahora = DateTimeOffset.UtcNow;

            var inicio = inicioVentanaPorCliente.AddOrUpdate(
                clave,
                ahora,
                (_, inicioExistente) => ahora - inicioExistente >= ventana ? ahora : inicioExistente);

            var restante = ventana - (ahora - inicio);
            var segundos = Math.Max(1, (int)Math.Ceiling(restante.TotalSeconds));
            contexto.HttpContext.Items["RetryAfterSeconds"] = segundos;

            return ValueTask.CompletedTask;
        };

        static string ClienteId(HttpContext contexto) =>
            contexto.Connection.RemoteIpAddress?.ToString() ?? "desconocido";

        options.AddPolicy("consulta-publica", contexto => RateLimitPartition.GetFixedWindowLimiter(
            ClienteId(contexto),
            _ => new FixedWindowRateLimiterOptions
            {
                Window = TimeSpan.FromMinutes(1),
                PermitLimit = 10,
                QueueLimit = 0
            }));

        options.AddPolicy("auth", contexto => RateLimitPartition.GetFixedWindowLimiter(
            ClienteId(contexto),
            _ => new FixedWindowRateLimiterOptions
            {
                Window = TimeSpan.FromMinutes(1),
                PermitLimit = 10,
                QueueLimit = 0
            }));
    });

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        await IdentitySeeder.SeedAsync(roleManager, userManager, app.Configuration, app.Environment.IsDevelopment());

        await CatalogoSeeder.SeedAsync(dbContext);
    }

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseStatusCodePagesWithReExecute("/Error/{0}");

    app.UseRouting();

    app.UseRateLimiter();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "La aplicacion no pudo iniciar.");
}
finally
{
    Log.CloseAndFlush();
}
