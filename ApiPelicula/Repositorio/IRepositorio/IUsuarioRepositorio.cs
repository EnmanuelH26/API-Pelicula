using ApiPelicula.Model;
using ApiPelicula.Model.Dtos;

namespace ApiPelicula.Repositorio.IRepositorio
{
   //Nota: Aqui solo se definen los metodos
    public interface IUsuarioRepositorio
    {
        //metodo ICollecion para que nos traiga la lista
        ICollection<Usuario> GetUsuarios(); //Todas las categorias
        //Metodo para obtener una sola categoria
        Usuario GetUsuario(int UsuarioId); //una sola categoria
        
        //si existe la categoria por id
        bool UnicoUsuario(string usuario);
        Task<UsuarioLoginRespuestaDTO> Login(UsuarioLoginDTO usuarioLoginDTO);
        Task<UsuarioDatosDTO> Registro(UsuarioRegistroDTO usuarioRegistroDTO);
    }
}
