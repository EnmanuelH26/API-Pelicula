using ApiPelicula.Model;

namespace ApiPelicula.Repositorio.IRepositorio
{
   //Nota: Aqui solo se definen los metodos
    public interface ICategoriaRepositorio
    {
        //metodo ICollecion para que nos traiga la lista
        ICollection<Categoria> GetCategorias(); //Todas las categorias
        //Metodo para obtener una sola categoria
        Categoria GetCategoria(int categoriaId); //una sola categoria
        
        //si existe la categoria por id
        bool ExisteCategoria(int categoriaId);
        //si existe la categoria por nombre
        bool ExisteCategoria(string nombre);
        bool CrearCategoria(Categoria categoria);
        bool ActualizarCategoria(Categoria categoria);
        bool BorrarCategoria(Categoria categoria);
        bool GuardarCategoria();
    }
}
