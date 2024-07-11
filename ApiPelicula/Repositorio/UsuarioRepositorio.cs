using ApiPelicula.Data;
using ApiPelicula.Model;
using ApiPelicula.Model.Dtos;
using ApiPelicula.Repositorio.IRepositorio;
using XSystem.Security.Cryptography;

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

        public async Task<Usuario> Registro(UsuarioRegistroDTO usuarioRegistroDTO)
        {
            var passwordEncriptado = obtenermd5(usuarioRegistroDTO.Password);

            Usuario usuario = new()
            {
                NombreUsuario = usuarioRegistroDTO.NombreUsuario,
                Password = passwordEncriptado,
                Nombre = usuarioRegistroDTO.Nombre,
                Role = usuarioRegistroDTO.Role
            };
            _db.Add(usuario);   
            await _db.SaveChangesAsync();
            usuario.Password = passwordEncriptado; //para que lo devuelva encriptado
            return usuario;

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

        public static string obtenermd5(string valor)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
            {
                resp += data[i].ToString("2").ToLower();
                
            }
            return resp;
        }
    }
    
}
