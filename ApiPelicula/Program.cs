using ApiPelicula.Data;
using ApiPelicula.PeliculasMaper;
using ApiPelicula.Repositorio;
using ApiPelicula.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Inyeccion de dependencias
builder.Services.AddDbContext<AplicationDbContext>(opciones =>
                     opciones.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql")));

//Agregamos los repositorios
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();

//Agregamos el automapper
builder.Services.AddAutoMapper(typeof(PeliculasMaper));

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
