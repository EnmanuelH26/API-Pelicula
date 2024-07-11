using System.ComponentModel.DataAnnotations;

namespace ApiPelicula.Model.Dtos
{
    public class UsuarioLoginDTO
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "El password es requerido")]
        public string Password { get; set; }
    }
}
