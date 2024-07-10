using System.ComponentModel.DataAnnotations;

namespace ApiPelicula.Model
{
    public class Categoria
    {
        //definimos el modelo
        //esto es una tabla en la base de datos
        [Key] //Para que sea una llave primaria y auto incremental
        public int Id { get; set; } //se recomienda que sea una llave primaria
        [Required] //Es un dato requerido
        public string NombreCategoria { get; set; }
        [Required]
        //[Display(Name = "Fecha de creacion")]  // se usa mas en web pero sirve para poner un nombre al modelo
        public DateTime FechaCreacion { get; set; }


    }
}
