using System.ComponentModel.DataAnnotations;

namespace ApiPelicula.Model.Dtos
{
    public class UsuarioDTO
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "El Nombre es obligatorio")]

        public string Nombre { get; set; }
        [Required(ErrorMessage = "El Passsword es obligatorio")]

        public string Password { get; set; }
    }
}
