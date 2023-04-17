using ApiAviones.Modelos;
using ApiAviones.Modelos.DTO;
using ApiAviones.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

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

        /// <summary>
        /// Crea un registro nuevo en la tabla Avion
        /// </summary>
        /// <param name="crearAvionDTO">Modelo requerido por este método para ser insertado en la tabla Avion</param>
        /// <returns></returns>
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

        /// <summary>
        /// Actualiza específicamente los valores que se desean
        /// </summary>
        /// <param name="crearAvionDTO"></param>
        /// <returns></returns>
        [HttpPatch("{avionId:int}", Name = "ActualizarPatchAvion")]
        [ProducesResponseType(201, Type = typeof(AvionDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]        
        public IActionResult ActualizarPatchAvion(int avionId, [FromBody] AvionDTO avionDTO) // Requiere "AvionDTO" porque si cuenta con el parametro avionId que es con el cual se va a actualizar en la base de datos.
        {
            // * Se valida el modelo (si no es valido)
            // ------------
            // * El ModelState controla que la clase CrearAvionDTO cumpla con los reuqerimientos
            // que se le especificaron con los decoradores [Required] y [MaxLength]
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Responde el método con un BadRequest si no es válido el modelo "AvionDTO" que está siendo enviado.
            }

            // Valida que no venga nulo el avion DTO y que solo tenga el avionId correcto
            if (avionDTO == null || avionId != avionDTO.Id )
            {
                return BadRequest(ModelState);
            }


            var avion = _mapper.Map<Avion>(avionDTO);

            // Si no se pudo crear la categoría 
            if (!_avionRepo.ActualizarAvion(avion))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro {avion.Nombre}.");
                return StatusCode(500, ModelState);
            }

            // Si sí se pudo crear se retorna el avion mandado
            return NoContent();
        }

        /// <summary>
        /// Elimina un avion de la tabla Avion
        /// </summary>
        /// <param name="avionId"></param>
        /// <param name="avionDTO"></param>
        /// <returns></returns>
        [HttpDelete("{avionId:int}", Name = "BorrarAvion")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // En caso de que no se encuentre el avión con ese ID 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult BorrarAvion(int avionId) // Requiere "AvionDTO" porque si cuenta con el parametro avionId que es con el cual se va a actualizar en la base de datos.
        {

            // * Si no existe no podemos hacer nada porque se está validando con "ExisteAvion"
            if (!_avionRepo.ExisteAvion(avionId))
            {
                return NotFound();
            }

            // Va y busca en la tabla Avion el avion por el atributo avionId
            var avion = _avionRepo.GetAvion(avionId);

            // Si no se pudo crear la categoría 
            if (!_avionRepo.BorrarAvion(avion))
            {
                ModelState.AddModelError("", $"Algo salió mal borrando el registro {avion.Nombre}.");
                return StatusCode(500, ModelState);
            }

            // Si sí se pudo crear se retorna el avion mandado
            return NoContent();
        }
    }
}
