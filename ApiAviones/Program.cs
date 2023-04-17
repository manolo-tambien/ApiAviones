using ApiAviones.AvionesMapper;
using ApiAviones.Data;
using ApiAviones.Repositorio;
using ApiAviones.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configuramos la conexion a SQL SERVER.
// Se obtiene la cadena de conexion llamada "ConnectionString" del archivo appsettings.json
builder.Services.AddDbContext<ApplicationDBContext>(opciones =>
{
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql"));
});

// Importante: Agregar los respositorios para poder acceder a ellos
builder.Services.AddScoped<IAvionRepositorio, AvionRepositorio>();

// Importante: Se tiene que instalar primero en el manejador de paquetes el paquete automapper.extensions.injection
builder.Services.AddAutoMapper(typeof(AvionesMapper));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
