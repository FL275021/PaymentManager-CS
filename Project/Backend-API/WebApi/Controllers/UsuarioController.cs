using DTOs;
using LogicaAplicacion.InterfacesCasosUso;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ExcepcionesPropias; 



namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
  {
    //interfaz(definicion) - implementacion(guarda el objeto a usar)
        public IAltaUsuario AltaUsuario;
        public IModificarUsuario ModificarUsuario;
        public IListadoUsuarios ListadoUsuarios;
        public IBuscarUsuarioPorId BuscarUsuarioPorId; 
        public IBajaUsuario BajaUsuario;
        public IUsuariosQueSuperanMontoDado UsuariosQueSuperanMontoDado;
        public IResetearPassword ResetearPassword; 
        public ILogin Login;

        public UsuarioController(
            IListadoUsuarios listadoUsuarios,
            IBuscarUsuarioPorId buscarUsuarioPorId,
            IAltaUsuario altaUsuario,
            IModificarUsuario modificarUsuario,
            IBajaUsuario bajaUsuario,
            IUsuariosQueSuperanMontoDado usuariosQueSuperanMontoDado,
            ILogin login,
            IResetearPassword resetearPassword)
        {
        //  campo privado = objeto que llega por parametro
            ListadoUsuarios = listadoUsuarios;
            BuscarUsuarioPorId = buscarUsuarioPorId;
            AltaUsuario = altaUsuario;
            ModificarUsuario = modificarUsuario;
            BajaUsuario = bajaUsuario;
            UsuariosQueSuperanMontoDado = usuariosQueSuperanMontoDado;
            ResetearPassword = resetearPassword;
            Login = login;
        }

    /// <summary>
    /// Permite obtener los datos de un usuario en base al id que recibe por parámetro
    /// </summary>
    /// <returns>el usuario o 404, 500</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<UsuarioDTO> Get(int id)
    {
      try
      {
        UsuarioDTO usuario = BuscarUsuarioPorId.EjecutarBusqueda(id);

        if (usuario == null)
        {
          return NotFound($"No se encontró el usuario con ID: {id}");
        }

        return Ok(usuario);
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError,
            "Error al obtener los datos del usuario");
      }
    }

    /// <summary>
    /// Permite obtener todos los usuarios
    /// </summary>
    /// <returns>el listado o 500</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet()]
    public ActionResult<IEnumerable<UsuarioDTO>> Get()
    {
      try
      {
        IEnumerable<UsuarioDTO> usuarios = ListadoUsuarios.ObtenerListado();
        return Ok(usuarios);
      }
      catch (Exception ex)
      {
        return StatusCode(500, "Error");
      }
    }


    /// <summary>
    /// Crea un nuevo usuario.
    /// Si datos invalidos devuelve un mensaje de error
    /// </summary>
    /// <returns>el envio creado o 400 por datos o 500 por server</returns>
    [HttpPost]
    [ProducesResponseType(typeof(UsuarioDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [HttpPost]
    public ActionResult Post([FromBody] UsuarioDTO usuario)
    {
      try
      {
        if (usuario == null)
        {
          return BadRequest("Datos incorrectos");
        }

        AltaUsuario.EjecutarAlta(usuario);
        return CreatedAtAction(nameof(Post), new { id = usuario.Id }, usuario);
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
    /// Modifica un usuario existente dada su id.
    /// Si los datos son inválidos, devuelve un mensaje de error.
    /// </summary> 
    /// <returns>el envio modificado o un 400</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UsuarioDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public ActionResult Put(int id, [FromBody] UsuarioDTO pDTO)
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
        ModificarUsuario.EjecutarModificacion(pDTO);
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
    /// Elimina un usuario existente dada su id
    /// </summary>
    /// <returns>204 no content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(UsuarioDTO), StatusCodes.Status200OK)]
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
        BajaUsuario.EjecutarBaja(id);
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
    ///permite a un usuario ingresar al sistema 
    /// </summary>
    /// <returns></returns>
    [HttpPost("login")] 
    [ProducesResponseType(StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)] 
    public ActionResult<UsuarioDTO> Ingresar([FromBody] LoginDTO login) 
    {
        try 
        {
          ManejadorJwt.CargarDatos(); 
          UsuarioDTO usr = ManejadorJwt.ObtenerUsuario(login.Email);
          if (usr == null || usr.Password != login.Password)
            return Unauthorized("Credenciales inválidas. Reintente");


          UsuarioDTO logueado = this.Login.EjecutarLogin(login.Email, login.Password);
          if (logueado == null){return Unauthorized("Credenciales inválidas. Reintente");}
          
          HttpContext.Session.SetString("loggedEmail", logueado.Email);
          HttpContext.Session.SetInt32("loggedId", logueado.Id);
        //  HttpContext.Session.SetString("loggedRol", logueado.Rol.Nombre); 
          Console.WriteLine($"usuario logueado: {logueado.Email}, {logueado.Id}");

          string token = ManejadorJwt.GenerarToken(usr);
          return Ok(new { Token = token, Usuario = usr});
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
    /// permite a un usuario cerrar su sesión
    /// </summary>
    /// <returns></returns>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
    

    /// <summary>
    /// Permite resetear la contraseña de un usuario dado si id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost("reset-password/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<string> ResetPassword(int id)
    {
      try 
      { 
          string rol = HttpContext.Session.GetString("loggedRol");
          if (rol != "ADMIN")
              return BadRequest("No tiene permisos para realizar esta acción");
          string passwNueva = ResetearPassword.Ejecutar(id);
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
  }
}