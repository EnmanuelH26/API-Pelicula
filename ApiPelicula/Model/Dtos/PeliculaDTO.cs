using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPelicula.Model.Dtos
{
    public class PeliculaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Duracion { get; set; }
        public string RutaImg { get; set; }
        public enum CrearTipoClasificacion
        {
            Siete, Trece, Dieciseis, Dieciocho

        }
        public CrearTipoClasificacion Clasificacion { get; set; }
        public DateTime FechaCreacion { get; set; }

        //relacion con categoria
        public int categoriaId { get; set; }
    }
}
