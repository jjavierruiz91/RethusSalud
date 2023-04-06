using Microsoft.EntityFrameworkCore;
using rethus_backend.Data;
using rethus_backend.Repository.IRepository;
using rethus_backend.Repository;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors();

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
  option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
