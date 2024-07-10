using System.ComponentModel.DataAnnotations;

namespace ApiPelicula.Model.Dtos
{
    public class CrearCategoriaDTO
    {
        [Required(ErrorMessage = "El Nombre es obligatorio")] //Es un dato requerido y muestra un error si el nombre no se ha puesto
        [MaxLength(100, ErrorMessage = "El maximo de caracteres es 100")]
        public string NombreCategoria { get; set; }
    }
}
