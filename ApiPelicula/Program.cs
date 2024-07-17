using ApiPelicula.Data;
using ApiPelicula.PeliculasMaper;
using ApiPelicula.Repositorio;
using ApiPelicula.Repositorio.IRepositorio;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Inyeccion de dependencias
builder.Services.AddDbContext<AplicationDbContext>(opciones =>
                     opciones.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql")));

//soporte para cache

var apiVersioning = builder.Services.AddApiVersioning(opcion =>
{
    opcion.AssumeDefaultVersionWhenUnspecified = true;
    opcion.DefaultApiVersion = new ApiVersion(1, 0);
    opcion.ReportApiVersions = true;
    //opcion.ApiVersionReader = ApiVersionReader.Combine(
    //    new QueryStringApiVersionReader("api-version")
    //    );
});

apiVersioning.AddApiExplorer(
    opciones =>
    {
        opciones.GroupNameFormat = "'v'VVV";
        opciones.SubstituteApiVersionInUrl = true;
    }
    );


//Agregamos los repositorios
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IPeliculaRepositorio, PeliculaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

var key = builder.Configuration.GetValue<string>("ApiSettings:Secreta");

//Soporte para versionamiento
builder.Services.AddApiVersioning();

//Agregamos el automapper
builder.Services.AddAutoMapper(typeof(PeliculasMaper));

//se configura autenticacion
builder.Services.AddAuthentication(
        //autenticacion por defecto
        x => {
            //agregar y configurar los servicios de autenticacion
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }


    ).AddJwtBearer(x =>
    { //agrega el jwtbearer
        x.RequireHttpsMetadata = false; //que no se requiere https para los metadatos(en produccion se pone en true)
        x.SaveToken = true; //se guarda el token
        x.TokenValidationParameters = new TokenValidationParameters //para validar los parametros del token
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false
    };

    });

//anadir a los controllers

builder.Services.AddControllers(opcion =>
{
    // anadir perfil global de cache
    opcion.CacheProfiles.Add("Defecto20Seg", new CacheProfile() { Duration = 30 });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();

//configurar al swagger en una aplicacion asp .net core usando el esquema bearer
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
        "Ponga bearer seguido de un espacio y despues su codigo o token",
        Name = "Authorization",                     
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    //agrega un requisito
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0",
        Title = "Peliculas Api V1",
        Description = "Api de Peliculas Versión 1",
        TermsOfService = new Uri("https://ejemplo.com"),
        Contact = new OpenApiContact
        {
            Name = "render2web",
            Url = new Uri("https://ejemplo.com")
        },
        License = new OpenApiLicense
        {
            Name = "Licencia Personal",
            Url = new Uri("https://ejemplo.com")
        },
    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2.0",
        Title = "Peliculas Api V2",
        Description = "Api de Peliculas Versión 2",
        TermsOfService = new Uri("https://ejemplo.com"),
        Contact = new OpenApiContact
        {
            Name = "render2web",
            Url = new Uri("https://ejemplo.com")
        },
        License = new OpenApiLicense
        {
            Name = "Licencia Personal",
            Url = new Uri("https://ejemplo.com")
        },
    });
}    
);

//soporte dominio
//se pueden habilitar 1-un dominio  2-multiples dominios, 3- cualquier dominio
//usamos de ejemplo el dominio: https://localhost:3223
//usamos (*) para que todos lo puedan usar

builder.Services.AddCors(p => p.AddPolicy("PoliticaCors", build =>
{
    //para que dominio quiero que tenga accesso mi dominio
    build.WithOrigins("https://localhost:3223").AllowAnyMethod().AllowAnyHeader();
}));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opciones =>
    {
        opciones.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiPeliculaV1");
        opciones.SwaggerEndpoint("/swagger/v2/swagger.json", "ApiPeliculaV2");
    });
}


app.UseHttpsRedirection();

//soporte para cors

app.UseCors("PoliticaCors"); //le pasamos el nombre de la politica

app.UseAuthentication(); //soporte para autenticacion
app.UseAuthorization();
app.MapControllers();
app.Run();
