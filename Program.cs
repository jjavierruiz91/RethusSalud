using Microsoft.EntityFrameworkCore;
using rethus_backend.Data;
using rethus_backend.Repository.IRepository;
using rethus_backend.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using rethus_backend;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(
        $"appsettings.{builder.Environment.EnvironmentName}.json",
        optional: false,
        reloadOnChange: true
    )
    .AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

string JWT_SECRET = builder.Configuration.GetValue<string>("ApiSettings:Secret");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            SaveSigninToken = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWT_SECRET)),
            ValidateIssuer = false,
            ValidateAudience = false,
            // ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter()
        );
    });
builder.Services.AddAuthorization();

// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy(
//   	Policies.User ,
// 		policy => policy.RequireAuthenticatedUser().RequireClaim("role", Policies.User)
// 	);
//     //  options.AddPolicy(Policies.Admin, Policies.AdminPolicy());
//     //  options.AddPolicy(Policies.User, Policies.UserPolicy());


//     // options.AddPolicy(Policies.User, policy => policy.RequireRole(Policies.User));
//     // options.AddPolicy("AdminPolicy", policy => policy.RequireRole(UserRoles.Admin.ToString()));
//     // options.AddPolicy("SuperAdminPolicy", policy => policy.RequireRole(UserRoles.SuperAdmin.ToString()));
//     // options.AddPolicy("OfficialPolicy", policy => policy.RequireRole(UserRoles.FuncionarioEtapa1.ToString()));
//     // options.AddPolicy("PublicPolicy", policy => policy.RequireAssertion(context => true));

// });
builder.Services.AddEndpointsApiExplorer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();

// builder.Services.AddSingleton<ConverPdfService>();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description =
                "JWT Authorization header using the Bearer scheme. \r\n\r\n "
                + "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n"
                + "Example: \"Bearer 12345abcdef\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Scheme = "Bearer"
        }
    );
    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        }
    );
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "Open",
        policy =>
        {
            policy.WithOrigins();
        }
    );
});

var app = builder.Build();
app.UseCors("Open");

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHsts();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        var result = new
        {
            Error = "Ha ocurrido un error interno en el servidor.",
            Details = exception?.Message
        };

        // Registrar el error
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(exception, "Error procesando la solicitud.");

        await context.Response.WriteAsJsonAsync(result);
    });
});
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
