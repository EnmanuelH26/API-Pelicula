using ApiPelicula.Data;
using ApiPelicula.Model;
using ApiPelicula.Model.Dtos;
using ApiPelicula.Repositorio.IRepositorio;

namespace ApiPelicula.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly AplicationDbContext _db; //instancia del contexto

        public UsuarioRepositorio(AplicationDbContext db)
        {
            _db = db;
        }
        public Usuario GetUsuario(int UsuarioId)
        {
            return _db.Usuario.FirstOrDefault(u => u.Id == UsuarioId);  
        }

        public ICollection<Usuario> GetUsuarios()
        {
            return _db.Usuario.OrderBy(u => u.Id).ToList();
        }

        public Task<UsuarioLoginRespuestaDTO> Login(UsuarioLoginDTO usuarioLoginDTO)
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioDatosDTO> Registro(UsuarioRegistroDTO usuarioRegistroDTO)
        {
            throw new NotImplementedException();
        }

        public bool UnicoUsuario(string usuario)
        {
            var usuarioBd = _db.Usuario.FirstOrDefault(u => u.NombreUsuario == usuario);

            if (usuarioBd == null)
            {
                return true;
            }
            return false;
        }
    }
}
