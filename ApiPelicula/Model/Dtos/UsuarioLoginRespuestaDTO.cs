﻿namespace ApiPelicula.Model.Dtos
{
    public class UsuarioLoginRespuestaDTO
    {
        public UsuarioDatosDTO Usuario { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
