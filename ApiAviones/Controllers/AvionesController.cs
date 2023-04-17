using ApiAviones.Modelos;
using ApiAviones.Modelos.DTO;
using ApiAviones.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiAviones.Controllers
{
    /// <summary>
    /// Para indicar que este es un controlador de una api hay que cambiar el nombre de la clase que hereda por "ControllerBase"
    /// además decorar la cabecera de la clase el valor de [ApiController] y tambien indicar cual es la ruta que va a tener el 
    /// controlador con el decorador [Route]
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AvionesController : ControllerBase
    {
        private readonly IAvionRepositorio _avionRepo;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor de la clase. Como parametros que recibe se encuntran _avionRepo y mapper
        /// </summary>
        /// <param name="_avionRepo">Este parámetro se utiliza para acceder a los métodos de consulta a la base de datos</param>
        /// <param name="mapper">Se usa para hacer la vinculación entre el DTO y el modelo (Avion)</param>
        public AvionesController(IAvionRepositorio avionRepo, IMapper mapper)
        {
            _avionRepo = avionRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene el listado de todos los aviones de la tabla Aviones
        /// </summary>
        /// <returns>Regresa el listado de todos los aviones de la tabla Aviones</returns>
        [HttpGet] // Se especifica el verbo, en este caso es el verbo 'Get'
        [ProducesResponseType(StatusCodes.Status200OK)] // Se le indica que podría responder con un status code 200 OK
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // Se le indica que podría responder con un status code 403 Forbidden
        public IActionResult GetAviones()
        {
            var listaAviones = _avionRepo.GetAviones();
            var listaAvionesDTO = new List<AvionDTO>();

            foreach (var lista in listaAviones)
            {
                listaAvionesDTO.Add(_mapper.Map<AvionDTO>(lista));
            }
            return Ok(listaAvionesDTO);
        }

        /// <summary>
        /// Obtiene un solo avión enviandole solamente el avionId que se desea buscar.
        /// </summary>
        /// <returns>Regresa un avion en base al avion buscado</returns>
        [HttpGet("{avionId:int}", Name = "GetAvion")] // Se especifica el verbo, en este caso es el verbo 'Get'
        [ProducesResponseType(StatusCodes.Status200OK)] // Se le indica que podría responder con un status code 200 OK
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // Se le indica que podría responder con un status code 403 Forbidden
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Se le indica con este decorador que podría responder con status code 400 Bad Request
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Se le indica con este decorador que podría responder con estatus code 404 Not Found
        public IActionResult GetAvion(int avionId)
        {
            var itemAvion = _avionRepo.GetAvion(avionId);
            if (itemAvion == null)
            {
                return NotFound();
            }
            var itemAvionDTO = _mapper.Map<AvionDTO>(itemAvion);
            return Ok(itemAvionDTO);
        }

        [HttpPost]
        [ProducesResponseType(201, Type= typeof(AvionDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearAvion([FromBody] CrearAvionDTO crearAvionDTO) // Se le especifica que del [FromBody] se va a obtener el modelo crearAvionDTO
        {
            // * Se valida el modelo (si no es valido)
            // ------------
            // * El ModelState controla que la clase CrearAvionDTO cumpla con los reuqerimientos
            // que se le especificaron con los decoradores [Required] y [MaxLength]
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState); // Responde el método con un BadRequest si no es válido el modelo "CrearAvionDTO" que está siendo enviado.
            }

            if (crearAvionDTO == null) 
            {
                return BadRequest(ModelState);
            }

            // * Se valida el parametro enviado "crearAvionDTO.Nombre" al método "ExisteAvion" para saber si ya hay ese 
            // nombre en la tabla de la base de datos.
            if (_avionRepo.ExisteAvion(crearAvionDTO.Nombre))
            {
                ModelState.AddModelError("", "El avión ya existe");
                return StatusCode(404, ModelState);
            }
            

            var avion = _mapper.Map<Avion>(crearAvionDTO);

            // Si no se pudo crear la categoría 
            if (!_avionRepo.CrearAvion(avion))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el registro {avion.Nombre}.");
                return StatusCode(500, ModelState);
            }

            // Si sí se pudo crear se retorna el avion mandado
            return CreatedAtRoute("GetAvion", new { avionId = avion.Id }, avion);
        }
    }
}
