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
    public class PagoController : ControllerBase
  {
    //interfaz(definicion) - implementacion(guarda el objeto a usar)
        public IAltaPago CUaltaPago;
        public IModificarPago CUmodificarPago;
        public IListadoPagos CUlistadoPagos;
        public IBuscarPagoPorId CUbuscarPagoporid;
        public IBajaPago CUbajaPago;
        public IPagosUsuarioDado CUpagosUsuarioDado;
        public IResetearPassword CUresetearPassword;

        public PagoController(
            IListadoPagos listadoPagos,
            IBuscarPagoPorId buscarPagoPorId,
            IAltaPago altaPago,
            IModificarPago modificarPago,
            IBajaPago bajaPago,
            IPagosUsuarioDado pagosUsuarioDado,
            IResetearPassword resetearPassword)
        {
        //  campo privado = objeto que llega por parametro
            CUlistadoPagos = listadoPagos;
            CUbuscarPagoporid = buscarPagoPorId;
            CUaltaPago = altaPago;
            CUmodificarPago = modificarPago;
            CUbajaPago = bajaPago; 
            CUpagosUsuarioDado = pagosUsuarioDado;
            CUresetearPassword = resetearPassword;
        }

    /// <summary>
    /// Permite obtener los datos de un pago en base al id que recibe por parámetro
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagoDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<PagoDTO> Get(int id)
    {
      try
      {
        var pago = CUbuscarPagoporid.EjecutarBusqueda(id);

        if (pago == null)
        {
          return NotFound($"No se encontró el pago con ID: {id}");
        }

        return Ok(pago);
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError,
            "Error al obtener los datos del pago");
      }
    }

    /// <summary>
    /// Permite obtener todos los pagos
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet()]
    public ActionResult<IEnumerable<PagoDTO>> Get()
    {
      try
      {
        IEnumerable<PagoDTO> pagos = CUlistadoPagos.ObtenerListado();
        return Ok(pagos);
      }
      catch (Exception ex)
      {
        return StatusCode(500, "Error");
      }
    }


    //RF5 - OB2
    /// <summary>
    /// Crea un nuevo pago.
    /// Si datos invalidos devuelve un mensaje de error
    /// </summary>
    /// <returns>el envio creado o 400 por datos o 500 por server</returns>
    [HttpPost]
    [ProducesResponseType(typeof(PagoDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [HttpPost]
    public ActionResult Post([FromBody] PagoDTO pago)
    {
      try
      {
        if (pago == null) 
        {
          return BadRequest("Datos incorrectos");
        }
        CUaltaPago.EjecutarAlta(pago);
        return CreatedAtAction(nameof(Post), new { id = pago.Id }, pago);
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
    /// Modifica un pago existente dada su id.
    /// Si los datos son inválidos, devuelve un mensaje de error.
    /// </summary>
    /// <returns>el envio modificado o un 400</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PagoDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public ActionResult Put(int id, [FromBody] PagoDTO pDTO)
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
        CUmodificarPago.EjecutarModificacion(pDTO);
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
    /// Elimina un pago existente dada su id
    /// </summary>
    /// <returns>204 no content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(PagoDTO), StatusCodes.Status200OK)]
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
            CUbajaPago.EjecutarBaja(id);
            return NoContent();

        }
        catch(DatosInvalidosException ex)
        {
            return BadRequest(ex.Message);
        }catch(Exception ex)
        {
            return StatusCode(500, "Error");
        }
    }


    [HttpPost("reset-password/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<string> ResetPassword(int id) //RF3 - OB2
    {
      try
      {
          string rol = HttpContext.Session.GetString("loggedRol");
          if (rol != "ADMIN")
              return BadRequest("No tiene permisos para realizar esta acción");
          string passwNueva = CUresetearPassword.Ejecutar(id);
          return Ok(passwNueva);
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

    [HttpPost("pagosusuariodado/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult PagosDeUsuarioDado(UsuarioDTO usuario) //RF2 - OB2
        {
            try
            {
                string rol = HttpContext.Session.GetString("loggedRol");
                if (rol != "EMPLEADO" && rol != "GERENTE")
                return BadRequest("No tiene permisos para realizar esta acción");
                List<PagoDTO> pagosUsuario = CUpagosUsuarioDado.EjecutarPagosUsuarioDado(usuario.Id);
                return Ok(pagosUsuario);
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

    }
}