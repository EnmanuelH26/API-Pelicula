using ApiPelicula.Model;

namespace ApiPelicula.Repositorio.IRepositorio
{
   //Nota: Aqui solo se definen los metodos
    public interface IPeliculaRepositorio
    {
        //metodo ICollecion para que nos traiga la lista
        ICollection<Pelicula> GetPeliculas(); //Todas las Peliculas
        ICollection<Pelicula> GetPeliculasEnCategoria(int categoriaId); //Todas las Peliculas en una categoria

        IEnumerable<Pelicula> BuscarPelicula(string buscarPelicula);

        //Metodo para obtener una sola Pelicula
        Pelicula GetPelicula(int peliculaId); //una sola Pelicula
        
        //si existe la Pelicula por id
        bool ExistePelicula(int peliculaId);
        //si existe la Pelicula por nombre
        bool ExistePelicula(string nombre);
        bool CrearPelicula(Pelicula pelicula);
        bool ActualizarPelicula(Pelicula pelicula);
        bool BorrarPelicula(Pelicula pelicula);
        bool GuardarPelicula();
    }
}
