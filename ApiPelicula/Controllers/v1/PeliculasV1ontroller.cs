using ApiPelicula.Model;
using ApiPelicula.Model.Dtos;
using ApiPelicula.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XAct.Security;

namespace ApiPelicula.Controllers.v1
{
    [Route("api/Peliculas")]
    [ApiController]
    public class PeliculasV1ontroller : ControllerBase
    {
        //Inyeccion de dependencia
        private readonly IPeliculaRepositorio _pelRepo;
        private readonly IMapper _mapper;
        public PeliculasV1ontroller(IPeliculaRepositorio pelRepo, IMapper mapper)
        {
            _pelRepo = pelRepo;
            _mapper = mapper;

        }




        #region Obtener Peliculas
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)] //El cliente no posee los permisos necesarios para cierto contenido
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPeliculas()
        {
            var ListaDePeliculas = _pelRepo.GetPeliculas(); //instanciamos una variable con la lista de categoria
            var ListaCategoriaDto = new List<PeliculaDTO>(); //instanciamos una variable con una lista de CategoriaDto

            //mapeamos la lista de categoria con ListaCategoriaDto
            foreach (var lista in ListaDePeliculas)
            {
                ListaCategoriaDto.Add(_mapper.Map<PeliculaDTO>(lista)); //agrega en categoriaDto la lista
            }
            //retorna la lista
            return Ok(ListaDePeliculas);
        }

        #endregion

        #region Obtener una sola Pelicula
        [HttpGet("{peliculaId:int}", Name = "GetPelicula")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)] //debera de producir un status code 403
        [ProducesResponseType(StatusCodes.Status200OK)]//debera de producir un status code 200OK
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//debera de producir un 400BadRequest
        [ProducesResponseType(StatusCodes.Status404NotFound)]//debera de producir un status code 404notfound
        public IActionResult GetPelicula(int peliculaId)
        {

            var ItemPelicula = _pelRepo.GetPelicula(peliculaId); //Instanciamos una variable que valida si hay una categoria con el categoriaId
            //validamos si es nulo
            if (ItemPelicula == null)
            {
                return NotFound(); //Statuscode 404
            }
            //instanciamos una variable para que el resultado sea mapeado con CategoriaDTO
            var ItemPeliculaDTO = _mapper.Map<PeliculaDTO>(ItemPelicula);
            //devolvemos el resultado
            return Ok(ItemPeliculaDTO);
        }
        #endregion

        #region Agregar una Pelicula

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(201, Type = typeof(PeliculaDTO))] //debera de producir un status code 201creado
        [ProducesResponseType(StatusCodes.Status201Created)]//debera de producir un status code 201creado
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//debera de producir un 400BadRequest
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//debera de producir un status code 500 que es un internal error
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]//debera de producir un status code 401 no autorizado(es con la autorizacion)
        public IActionResult CrearPelicula([FromBody] CrearPeliculaDTO peldto)
        {
            //Validamos si el modelo es valido, "si el modelo no es valido retorna modelstate"
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //si es nulo devuelve un badrequest con el model state
            if (peldto == null)
            {
                return BadRequest(ModelState);
            }
            //busca en el repo el metodo para saber si existe
            if (_pelRepo.ExistePelicula(peldto.Nombre))
            {
                ModelState.AddModelError("", $"La pelicula existe"); //si el modelo existe retorna un status code (404 y el model state    )
                return StatusCode(404, ModelState);
            }
            //instanciamos una variable que mapea categoria con el parametro ccdto 
            var pelicula = _mapper.Map<Pelicula>(peldto);
            //valiamos si se pudo guardar
            if (!_pelRepo.CrearPelicula(pelicula))
            {
                //si no se pudo guardar agregamos un model error 
                ModelState.AddModelError("", $"Algo salio mal guardando el registro{pelicula.Nombre}");
                return BadRequest(ModelState);
            }
            //Creamos y retornamos al ruta
            return CreatedAtRoute("GetPelicula", new { peliculaId = pelicula.Id }, pelicula);

        }

        #endregion

        #region Actualizar pelicula
        [Authorize(Roles = "Admin")]

        [HttpPatch("{peliculaId:int}", Name = "ActualizarPatchPelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] //debera de producir un status code 201creado
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//debera de producir un 400BadRequest
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]//debera de producir un status code 401 no autorizado(es con la autorizacion)
        public IActionResult ActualizarPatchPelicula(int peliculaId, [FromBody] PeliculaDTO peldto)
        {
            //Validamos si el modelo es valido, "si el modelo no es valido retorna modelstate"
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //si es nulo devuelve un badrequest con el model state o si no coincide con el id de la tabla
            if (peldto == null || peliculaId != peldto.Id)
            {
                return BadRequest(ModelState);
            }

            //instanciamos una variable que mapea categoria con el parametro ccdto 
            var pelicula = _mapper.Map<Pelicula>(peldto);

            //valiamos si se pudo actualizar
            if (!_pelRepo.ActualizarPelicula(pelicula))
            {
                //si no se pudo guardar agregamos un model error 
                ModelState.AddModelError("", $"Algo salio mal al actualizar el registro{pelicula.Nombre}");
                return BadRequest(ModelState);
            }


            //Retornamos un NoContent porque es Patch
            return NoContent();
        }
        #endregion
        #region Borra categoria
        [Authorize(Roles = "Admin")]

        [HttpDelete("{peliculaId:int}", Name = "BorrarPelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] //debera de producir un status code 201creado
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//debera de producir un 400BadRequest
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]//debera de producir un status code 401 no autorizado(es con la autorizacion)
        [ProducesResponseType(StatusCodes.Status404NotFound)]//debera de producir un Status404NotFound
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//debera de producir un SStatus500InternalServerError
        public IActionResult BorrarPelicula(int peliculaId)
        {
            //Validamos si el modelo es valido, "si el modelo no es valido retorna modelstate"
            if (!_pelRepo.ExistePelicula(peliculaId))
            {
                return NotFound();
            }

            var peliculaExistente = _pelRepo.GetPelicula(peliculaId);


            //valiamos si se pudo actualizar
            if (!_pelRepo.BorrarPelicula(peliculaExistente))
            {
                //si no se pudo guardar agregamos un model error 
                ModelState.AddModelError("", $"Algo salio mal al Borrar el registro{peliculaExistente.Nombre}");
                return StatusCode(500, ModelState);
            }
            //Retornamos un NoContent porque es Patch
            return NoContent();
        }
        #endregion

        [HttpGet("GetPeliculasEnCategoria/{categoriaId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetPeliculasEnCategoria(int categoriaId)
        {
            var listaPeliculas = _pelRepo.GetPeliculasEnCategoria(categoriaId);

            if (listaPeliculas == null)
            {
                return NotFound();
            }

            var itemPeliculas = new List<PeliculaDTO>();
            foreach (var item in listaPeliculas)
            {
                itemPeliculas.Add(_mapper.Map<PeliculaDTO>(item));
            }


            return Ok(itemPeliculas);
        }



        [HttpGet("Buscar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Buscar(string nombrePelicula)
        {
            try
            {
                var resultado = _pelRepo.BuscarPelicula(nombrePelicula);
                if (resultado.Any())
                {
                    return Ok(resultado);
                }
                return NotFound();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error al buscar");
            }

        }
    }

}


