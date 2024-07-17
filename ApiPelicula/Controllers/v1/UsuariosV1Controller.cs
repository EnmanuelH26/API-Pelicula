using ApiPelicula.Model;
using ApiPelicula.Model.Dtos;
using ApiPelicula.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiPelicula.Controllers.v1
{
    [Route("api/Usuarios")]
    [ApiController]
    [ResponseCache(CacheProfileName = "Defecto20Seg")]
    public class UsuariosV1Controller : ControllerBase
    {
        //Inyeccion de dependencia
        private readonly IUsuarioRepositorio _usRepo;
        protected RespuestaAPI _respuestaAPI;
        private readonly IMapper _mapper;
        public UsuariosV1Controller(IUsuarioRepositorio usRepo, IMapper mapper)
        {
            _usRepo = usRepo;
            _mapper = mapper;
            _respuestaAPI = new();

        }

        #region Obtener Usuarios
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)] //El cliente no posee los permisos necesarios para cierto contenido
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUsuario()
        {
            var ListaDeUsuarios = _usRepo.GetUsuarios(); //instanciamos una variable con la lista de categoria
            var ListaDeUsuariosDto = new List<UsuarioDTO>(); //instanciamos una variable con una lista de CategoriaDto

            //mapeamos la lista de categoria con ListaCategoriaDto
            foreach (var lista in ListaDeUsuarios)
            {
                ListaDeUsuariosDto.Add(_mapper.Map<UsuarioDTO>(lista)); //agrega en categoriaDto la lista
            }
            //retorna la lista
            return Ok(ListaDeUsuarios);
        }

        #endregion

        #region Obtener un solo usuario
        [HttpGet("{usuarioId:int}", Name = "GetUsuario")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)] //debera de producir un status code 403
        [ProducesResponseType(StatusCodes.Status200OK)]//debera de producir un status code 200OK
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//debera de producir un 400BadRequest
        [ProducesResponseType(StatusCodes.Status404NotFound)]//debera de producir un status code 404notfound
        public IActionResult GetUsuario(int usuarioId)
        {

            var ItemUsuario = _usRepo.GetUsuario(usuarioId); //Instanciamos una variable que valida si hay una categoria con el categoriaId
            //validamos si es nulo
            if (ItemUsuario == null)
            {
                return NotFound(); //Statuscode 404
            }
            //instanciamos una variable para que el resultado sea mapeado con CategoriaDTO
            var ItemUsuarioDto = _mapper.Map<UsuarioDTO>(ItemUsuario);
            //devolvemos el resultado
            return Ok(ItemUsuarioDto);
        }
        #endregion

        #region Postear un solo usuario
        [HttpPost("registro")]
        [ProducesResponseType(StatusCodes.Status201Created)]//debera de producir un status code 201created
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//debera de producir un 400BadRequest
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//debera de producir un status code 404notfound
        public async Task<IActionResult> Registro([FromBody] UsuarioRegistroDTO usuarioRegistroDTO)
        {

            bool validarNombreDeUsuario = _usRepo.UnicoUsuario(usuarioRegistroDTO.NombreUsuario);

            if (!validarNombreDeUsuario)
            {
                _respuestaAPI.statusCode = HttpStatusCode.BadRequest;
                _respuestaAPI.isSuccess = false;
                _respuestaAPI.ErrorMessages.Add("El nombre de usuario ya existe");
                return BadRequest(_respuestaAPI);
            }
            var usuario = await _usRepo.Registro(usuarioRegistroDTO);

            if (usuario == null)
            {
                _respuestaAPI.statusCode = HttpStatusCode.BadRequest;
                _respuestaAPI.isSuccess = false;
                _respuestaAPI.ErrorMessages.Add("Error al registrars");
                return BadRequest(_respuestaAPI);

            }
            _respuestaAPI.statusCode = HttpStatusCode.OK;
            _respuestaAPI.isSuccess = true;
            return Ok(_respuestaAPI);
        }
        #endregion

        #region Postear un solo usuario
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status201Created)]//debera de producir un status code 201created
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//debera de producir un 400BadRequest
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//debera de producir un status code 404notfound
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDTO UsuarioLoginDTO)
        {
            var respuestaLogin = await _usRepo.Login(UsuarioLoginDTO);

            if (respuestaLogin.Usuario == null || string.IsNullOrEmpty(respuestaLogin.Token))
            {
                _respuestaAPI.statusCode = HttpStatusCode.BadRequest;
                _respuestaAPI.isSuccess = false;
                _respuestaAPI.ErrorMessages.Add("El nombre de usuario o paswword esta mal");
                return BadRequest(_respuestaAPI);
            }
            _respuestaAPI.statusCode = HttpStatusCode.OK;
            _respuestaAPI.isSuccess = true;
            _respuestaAPI.Result = respuestaLogin;
            return Ok(_respuestaAPI);
        }
        #endregion

    }
}
