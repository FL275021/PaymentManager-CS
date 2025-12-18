using DTOs;
using LogicaAplicacion.InterfacesCasosUso;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExcepcionesPropias;
using LogicaNegocio.EntidadesDominio;


namespace WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class GastoController : ControllerBase
  {
    //interfaz(definicion) - implementacion(guarda el objeto a usar)
    public IAltaGasto CUaltaGasto;
    public IModificarGasto CUmodificarGasto;
    public IListadoGastos CUlistadoGastos;
    public IBuscarGastoPorId CUbuscarGastoporid;
    public IBajaGasto CUbajaGasto;
    public IAuditoriaPorGasto CUAuditoriaPorGasto;

    public GastoController(
        IListadoGastos listadoGastos,
        IBuscarGastoPorId buscarGastoPorId,
        IAltaGasto altaGasto,
        IModificarGasto modificarGasto,
        IBajaGasto bajaGasto,
        IAuditoriaPorGasto auditoriaPorGasto)
    {
      //  campo privado = objeto que llega por parametro
      CUlistadoGastos = listadoGastos;
      CUbuscarGastoporid = buscarGastoPorId;
      CUaltaGasto = altaGasto;
      CUmodificarGasto = modificarGasto;
      CUbajaGasto = bajaGasto;
      CUAuditoriaPorGasto = auditoriaPorGasto;
    }

    /// <summary>
    /// Permite obtener los datos de un gasto en base al id que recibe por parámetro
    /// </summary>
    /// <returns>el gasto o 404, 500</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GastoDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<GastoDTO> Get(int id)
    {
      try
      {
        GastoDTO gasto = CUbuscarGastoporid.EjecutarBusqueda(id);

        if (gasto == null)
        {
          return NotFound($"No se encontró el gasto con ID: {id}");
        }

        return Ok(gasto);
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError,
            "Error al obtener los datos del gasto");
      }
    }

    /// <summary>
    /// Permite obtener todos los gastos
    /// </summary>
    /// <returns>el listado o 500</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet()]
    public ActionResult<IEnumerable<GastoDTO>> Get()
    {
      try
      {
        IEnumerable<GastoDTO> gastos = CUlistadoGastos.ObtenerListado();
        return Ok(gastos);
      }
      catch (Exception ex)
      {
        return StatusCode(500, "Error");
      }
    }


    /// <summary>
    /// Crea un nuevo gasto.
    /// Si datos invalidos devuelve un mensaje de error
    /// </summary>
    /// <returns>el envio creado o 400 por datos o 500 por server</returns>
    [HttpPost]
    [ProducesResponseType(typeof(GastoDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [HttpPost]
    public ActionResult Post([FromBody] GastoDTO gasto)
    {
      try
      {
        if (gasto == null)
        {
          return BadRequest("Datos incorrectos");
        }

        CUaltaGasto.EjecutarAlta(gasto);
        return CreatedAtAction(nameof(Post), new { id = gasto.Id }, gasto);
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
    /// Modifica un gasto existente dada su id.
    /// Si los datos son inválidos, devuelve un mensaje de error.
    /// </summary>
    /// <returns>el envio modificado o un 400</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(GastoDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public ActionResult Put(int id, [FromBody] GastoDTO pDTO)
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
        CUmodificarGasto.EjecutarModificacion(pDTO);
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
    /// Elimina un gasto existente dada su id
    /// </summary>
    /// <returns>204 no content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(GastoDTO), StatusCodes.Status200OK)]
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
        CUbajaGasto.EjecutarBaja(id);
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
 
    [HttpGet("{id}/auditoria")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<AuditoriaDTO>> GetAuditoria(int id)
    {
      IEnumerable<AuditoriaDTO> auditorias = CUAuditoriaPorGasto.AuditoriaPorGasto(id);
      if (auditorias == null || !auditorias.Any())
        return NotFound("No hay auditoría para este gasto.");
      return Ok(auditorias); 
    }
  }
}