using DTOs;
using LogicaAplicacion.InterfacesCasosUso;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExcepcionesPropias;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipoController : ControllerBase
  {
    //interfaz(definicion) - implementacion(guarda el objeto a usar)
        public IAltaEquipo CUaltaEquipo;
        public IModificarEquipo CUmodificarEquipo;
        public IListadoEquipos CUlistadoEquipos;
        public IBuscarEquipoPorId CUbuscarEquipoporid;
        public IBajaEquipo CUbajaEquipo;
        public IBuscarPorPagoMayorA CUbuscarPorPagoMayorA;

        public EquipoController(
            IListadoEquipos listadoEquipos,
            IBuscarEquipoPorId buscarEquipoPorId,
            IAltaEquipo altaEquipo,
            IModificarEquipo modificarEquipo,
            IBajaEquipo bajaEquipo,
            IBuscarPorPagoMayorA buscarPorPagoMayorA)
        {
        //  campo privado = objeto que llega por parametro
            CUlistadoEquipos = listadoEquipos;
            CUbuscarEquipoporid = buscarEquipoPorId;
            CUaltaEquipo = altaEquipo;
            CUmodificarEquipo = modificarEquipo;
            CUbajaEquipo = bajaEquipo;
            CUbuscarPorPagoMayorA = buscarPorPagoMayorA;
        }

    /// <summary>
    /// Permite obtener los datos de un equipo en base al id que recibe por parámetro
    /// </summary>
    /// <returns>el equipo o 404, 500</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EquipoDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<EquipoDTO> Get(int id)
    {
      try
      {
        EquipoDTO equipo = CUbuscarEquipoporid.EjecutarBusqueda(id);

        if (equipo == null)
        {
          return NotFound($"No se encontró el equipo con ID: {id}");
        }

        return Ok(equipo);
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError,
            "Error al obtener los datos del equipo");
      }
    }

    /// <summary>
    /// Permite obtener todos los equipos
    /// </summary>
    /// <returns>el listado o 500</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet()]
    public ActionResult<List<EquipoDTO>> Get()
    {
      try 
      {
        List<EquipoDTO> equipos = CUlistadoEquipos.ObtenerListado();
        return Ok(equipos);
      }
      catch (Exception ex)
      {
        return StatusCode(500, "Error");
      }
    }


    /// <summary>
    /// Crea un nuevo equipo.
    /// Si datos invalidos devuelve un mensaje de error
    /// </summary>
    /// <returns>el envio creado o 400 por datos o 500 por server</returns>
    [HttpPost]
    [ProducesResponseType(typeof(EquipoDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [HttpPost]
    public ActionResult Post([FromBody] EquipoDTO equipo)
    {
      try
      {
        if (equipo == null)
        {
          return BadRequest("Datos incorrectos");
        }

        CUaltaEquipo.EjecutarAlta(equipo);
        return CreatedAtAction(nameof(Post), new { id = equipo.Id }, equipo);
      }
      catch (DatosInvalidosException ex)
      {
        return BadRequest(ex.Message);
      }
      catch (Exception ex)
      {
        return StatusCode(500, "Ocurrió un error inesperado. Intente de nuevo más tarde");
      }
    }


    /// <summary>
    /// Modifica un equipo existente dada su id.
    /// Si los datos son inválidos, devuelve un mensaje de error.
    /// </summary>
    /// <returns>el envio modificado o un 400</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(EquipoDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public ActionResult Put(int id, [FromBody] EquipoDTO pDTO)
    {
      try
      {
        if (id <= 0)
        {
          return BadRequest("El id no es correcto");
        }
        if (pDTO == null)
        {
          return BadRequest("Los datos no son correctos");
        }
        if (id != pDTO.Id)
        {
          return BadRequest("Los ids no coinciden");
        }
        CUmodificarEquipo.EjecutarModificacion(pDTO);
        return Ok();

      }
      catch (DatosInvalidosException ex)
      {
        return BadRequest(ex.Message);
      }
      catch (Exception ex)
      {
        return StatusCode(500, "Error");
      }
    }


        /// <summary>
        /// Elimina un equipo existente dada su id
        /// </summary>
        /// <returns>204 no content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(EquipoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public ActionResult Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("El id no es correcto");

                }
                CUbajaEquipo.EjecutarBaja(id);
                return NoContent();

            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error");
            }
        }
    
        /// <summary>
        /// Permite obtener los equipos que hayan realizado pagos únicos mayores al monto indicado
        [HttpGet("pagosMayores/{monto}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<EquipoDTO>> GetEquiposPorPagoUnicoMayor(decimal monto)
        {
            try
            {
                List<EquipoDTO> equipos = CUbuscarPorPagoMayorA.EjecutarBusqueda(monto);
                return Ok(equipos);
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error");
            }
        }

    }
}