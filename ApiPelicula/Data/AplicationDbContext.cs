using ApiPelicula.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiPelicula.Data
{
    //para la conexion a la base de datos debe de extender de DbContext
    public class AplicationDbContext : DbContext
    {
        //contructor de la clase
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) 
            : base(options) //metodo de la clase, hereda de base, sirve para recibir las entidades del db context
        {
        }
        //Aqui pasar todos los modelos o entidades
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Pelicula> Pelicula { get; set; }



    }
}
