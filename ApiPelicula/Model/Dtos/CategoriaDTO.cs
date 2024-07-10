using System.ComponentModel.DataAnnotations;

namespace ApiPelicula.Model.Dtos
{
    //Este es el que se va a exponer
    //aqui se ponen las validaciones
    public class CategoriaDTO
    {
        //definimos el modelo
        //esto es una tabla en la base de datos
        //[Key] //Para que sea una llave primaria y auto incremental
        public int Id { get; set; } //se recomienda que sea una llave primaria
        [Required(ErrorMessage = "El Nombre es obligatorio")] //Es un dato requerido y muestra un error si el nombre no se ha puesto
        [MaxLength(100, ErrorMessage = "El maximo de caracteres es 100")]
        public string NombreCategoria { get; set; }
        [Required]
        //[Display(Name = "Fecha de creacion")]  // se usa mas en web pero sirve para poner un nombre al modelo
        public DateTime FechaCreacion { get; set; }
    }
}
