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

namespace ApiPelicula.Controllers.v2
{
    //[Authorize(Roles = "Admin")] //para protejer el controlador
    //[Route("api/[controller]")] //opcion estatica
    [Route("api/v{version:Apiversion}/Categoria")] //opcion estatica
    //[ResponseCache(Duration = 20)] nivel de controlador
    [ApiVersion("2.0")]
    [ApiController]
    public class CategoriasV2Controller : ControllerBase
    {
        //Inyeccion de dependencia
        private readonly ICategoriaRepositorio _ctRepo;
        private readonly IMapper _mapper;
        public CategoriasV2Controller(ICategoriaRepositorio ctRepo, IMapper mapper)
        {
            _ctRepo = ctRepo;
            _mapper = mapper;

        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "valor1", "valor2", "valor3" };
        }
    }
}