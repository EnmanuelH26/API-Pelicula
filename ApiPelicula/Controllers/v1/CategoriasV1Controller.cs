using ApiPelicula.Data;
using ApiPelicula.Model;
using ApiPelicula.Model.Dtos;
using ApiPelicula.Repositorio.IRepositorio;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XAct.Security;

namespace ApiPelicula.Controllers.v1
{
    //[Authorize(Roles = "Admin")] //para protejer el controlador
    //[Route("api/[controller]")] //opcion estatica
    [Route("api/v{version:Apiversion}/Categoria")] //opcion estatica
    //[ResponseCache(Duration = 20)] nivel de controlador
    [ApiVersion("1.0")]
    [ApiController]
    public class CategoriasV1Controller : ControllerBase
    {
        //Inyeccion de dependencia
        private readonly ICategoriaRepositorio _ctRepo;
        private readonly IMapper _mapper;
        public CategoriasV1Controller(ICategoriaRepositorio ctRepo, IMapper mapper)
        {
            _ctRepo = ctRepo;
            _mapper = mapper;

        }
        #region Obtener Categoria
        [HttpGet]
        //[MapToApiVersion("1.0")]
        [ResponseCache(Duration = 20)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)] //El cliente no posee los permisos necesarios para cierto contenido
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategorias()
        {
            var ListaDeCategoria = _ctRepo.GetCategorias(); //instanciamos una variable con la lista de categoria
            var ListaCategoriaDto = new List<CategoriaDTO>(); //instanciamos una variable con una lista de CategoriaDto

            //mapeamos la lista de categoria con ListaCategoriaDto
            foreach (var lista in ListaDeCategoria)
            {
                ListaCategoriaDto.Add(_mapper.Map<CategoriaDTO>(lista)); //agrega en categoriaDto la lista
            }
            //retorna la lista
            return Ok(ListaDeCategoria);
        }

        #endregion

        #region Obtener una sola categoria
        [HttpGet("{categoriaId:int}", Name = "GetCategoria")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)] //para que no se guarde el cache ni en el servidor ni en el cliente
        [ProducesResponseType(StatusCodes.Status403Forbidden)] //debera de producir un status code 403
        [ProducesResponseType(StatusCodes.Status200OK)]//debera de producir un status code 200OK
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//debera de producir un 400BadRequest
        [ProducesResponseType(StatusCodes.Status404NotFound)]//debera de producir un status code 404notfound
        public IActionResult GetCategoria(int categoriaId)
        {

            var ItemCategoria = _ctRepo.GetCategoria(categoriaId); //Instanciamos una variable que valida si hay una categoria con el categoriaId
            //validamos si es nulo
            if (ItemCategoria == null)
            {
                return NotFound(); //Statuscode 404
            }
            //instanciamos una variable para que el resultado sea mapeado con CategoriaDTO
            var ItemCategoriaDto = _mapper.Map<CategoriaDTO>(ItemCategoria);
            //devolvemos el resultado
            return Ok(ItemCategoriaDto);
        }
        #endregion

        #region Agregar una categoria
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)] //debera de producir un status code 201creado
        [ProducesResponseType(StatusCodes.Status200OK)]//debera de producir un status code 200OK
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//debera de producir un 400BadRequest
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//debera de producir un status code 500 que es un internal error
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]//debera de producir un status code 401 no autorizado(es con la autorizacion)
        public IActionResult CrearCategoria([FromBody] CrearCategoriaDTO ccdto)
        {
            //Validamos si el modelo es valido, "si el modelo no es valido retorna modelstate"
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //si es nulo devuelve un badrequest con el model state
            if (ccdto == null)
            {
                return BadRequest(ModelState);
            }
            //busca en el repo el metodo para saber si existe
            if (_ctRepo.ExisteCategoria(ccdto.NombreCategoria))
            {
                ModelState.AddModelError("", $"La categoria existe"); //si el modelo existe retorna un status code (404 y el model state    )
                return StatusCode(404, ModelState);
            }
            //instanciamos una variable que mapea categoria con el parametro ccdto 
            var categoria = _mapper.Map<Categoria>(ccdto);
            //valiamos si se pudo guardar
            if (!_ctRepo.CrearCategoria(categoria))
            {
                //si no se pudo guardar agregamos un model error 
                ModelState.AddModelError("", $"Algo salio mal guardando el registro{categoria.NombreCategoria}");
                return BadRequest(ModelState);
            }
            //Creamos y retornamos al ruta
            return CreatedAtRoute("GetCategoria", new { categoriaId = categoria.Id }, categoria);
        }
        #endregion


        #region Actualizar una sola categoria por Id

        [Authorize(Roles = "Admin")]
        [HttpPatch("{CategoriaId:int}", Name = "ActualizarPatchCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] //debera de producir un status code 201creado
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//debera de producir un 400BadRequest
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]//debera de producir un status code 401 no autorizado(es con la autorizacion)
        public IActionResult ActualizarPatchCategoria(int CategoriaId, [FromBody] CategoriaDTO ccdto)
        {
            //Validamos si el modelo es valido, "si el modelo no es valido retorna modelstate"
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //si es nulo devuelve un badrequest con el model state o si no coincide con el id de la tabla
            if (ccdto == null || CategoriaId != ccdto.Id)
            {
                return BadRequest(ModelState);
            }



            //instanciamos una variable que mapea categoria con el parametro ccdto 
            var categoria = _mapper.Map<Categoria>(ccdto);

            //valiamos si se pudo actualizar
            if (!_ctRepo.ActualizarCategoria(categoria))
            {
                //si no se pudo guardar agregamos un model error 
                ModelState.AddModelError("", $"Algo salio mal al actualizar el registro{categoria.NombreCategoria}");
                return BadRequest(ModelState);
            }


            //Retornamos un NoContent porque es Patch
            return NoContent();
        }
        #endregion


        #region Actualizar categoria

        [Authorize(Roles = "Admin")]
        [HttpPut("{CategoriaId:int}", Name = "ActualizarPutCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] //debera de producir un status code 201creado
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//debera de producir un 400BadRequest
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]//debera de producir un status code 401 no autorizado(es con la autorizacion)
        public IActionResult ActualizarPutCategoria(int CategoriaId, [FromBody] CategoriaDTO ccdto)
        {
            //Validamos si el modelo es valido, "si el modelo no es valido retorna modelstate"
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //si es nulo devuelve un badrequest con el model state o si no coincide con el id de la tabla
            if (ccdto == null || CategoriaId != ccdto.Id)
            {
                return BadRequest(ModelState);
            }
            var categoriaExistente = _ctRepo.GetCategoria(CategoriaId);

            if (categoriaExistente == null)
            {
                return NotFound($"No se encontro la categoria con Id: {CategoriaId}");
            }

            //instanciamos una variable que mapea categoria con el parametro ccdto 
            var categoria = _mapper.Map<Categoria>(ccdto);

            //valiamos si se pudo actualizar
            if (!_ctRepo.ActualizarCategoria(categoria))
            {
                //si no se pudo guardar agregamos un model error 
                ModelState.AddModelError("", $"Algo salio mal al actualizar el registro{categoria.NombreCategoria}");
                return BadRequest(ModelState);
            }


            //Retornamos un NoContent porque es Patch
            return NoContent();
        }

        #endregion

        #region Borra categoria
        [Authorize(Roles = "Admin")]
        [HttpDelete("{CategoriaId:int}", Name = "BorrarCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] //debera de producir un status code 201creado
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//debera de producir un 400BadRequest
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]//debera de producir un status code 401 no autorizado(es con la autorizacion)
        [ProducesResponseType(StatusCodes.Status404NotFound)]//debera de producir un Status404NotFound
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//debera de producir un SStatus500InternalServerError
        public IActionResult BorrarCategoria(int CategoriaId)
        {
            //Validamos si el modelo es valido, "si el modelo no es valido retorna modelstate"
            if (!_ctRepo.ExisteCategoria(CategoriaId))
            {
                return NotFound();
            }

            var categoriaExistente = _ctRepo.GetCategoria(CategoriaId);


            //valiamos si se pudo actualizar
            if (!_ctRepo.BorrarCategoria(categoriaExistente))
            {
                //si no se pudo guardar agregamos un model error 
                ModelState.AddModelError("", $"Algo salio mal al Borrar el registro{categoriaExistente.NombreCategoria}");
                return StatusCode(500, ModelState);
            }


            //Retornamos un NoContent porque es Patch
            return NoContent();
        }
        #endregion
    }
}