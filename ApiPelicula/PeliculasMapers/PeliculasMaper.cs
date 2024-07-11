using ApiPelicula.Model;
using ApiPelicula.Model.Dtos;
using AutoMapper;
using System.Runtime;

namespace ApiPelicula.PeliculasMaper
{
    public class PeliculasMaper : Profile //extiende de una clase de auto maper
    {
        public PeliculasMaper()
        {
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();   //crea el mapeo que Categoria se conecta con CategoriaDTO
                                                                 //Tambien le ponemos reverse map para que vaya de CategoriaDTO a Categoria
            CreateMap<Categoria, CrearCategoriaDTO>().ReverseMap(); //Lo mismo para el modelo de CrearCategoria DTO

            CreateMap<Pelicula, PeliculaDTO>().ReverseMap();   //crea el mapeo que Categoria se conecta con CategoriaDTO
                                                                 //Tambien le ponemos reverse map para que vaya de CategoriaDTO a Categoria
            CreateMap<Pelicula, CrearPeliculaDTO>().ReverseMap(); //Lo mismo para el modelo de CrearCategoria DTO
        }
    }
    
}
