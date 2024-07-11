using System.ComponentModel.DataAnnotations;

namespace ApiPelicula.Model.Dtos
{
    public class UsuarioRegistroDTO
    {

        [Required(ErrorMessage = "El usuario es requerido")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]

        public string Nombre { get; set; }
        [Required(ErrorMessage = "El password es requerido")]

        public string Password { get; set; }
        public string Role { get; set; }
    }
}
