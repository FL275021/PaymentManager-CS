using LogicaAccesoDatos;
using LogicaNegocio.EntidadesDominio;
using ExcepcionesPropias;
using Newtonsoft.Json;
using WebMVC.Auxiliares;
using WebMVC.DTOs;
using LogicaAplicacion.InterfacesCasosUso;
using LogicaAplicacion.Mapeadores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using NuGet.Protocol;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Net.Http.Headers;

namespace WebMVC.Controllers
{

    public class UsuariosController : Controller
    {
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        private readonly string urlApiUsuarios;

        public UsuariosController(IConfiguration configuration)
        {
            urlApiUsuarios = configuration.GetSection("ConnectionStrings:URLApiUsuarios").Value;
        }

        // GET: UsuariosController
        public ActionResult Index()
        {
            IEnumerable<UsuarioDTO> usuarios = new List<UsuarioDTO>();

            try
            {
                string token = HttpContext.Session.GetString("token");
                HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(urlApiUsuarios, "GET", null, token);
                string body = AuxiliarClienteHttp.ObtenerBody(respuesta);

                if (respuesta.IsSuccessStatusCode) // Serie 200
                {
                    usuarios = JsonConvert.DeserializeObject<IEnumerable<UsuarioDTO>>(body); // en el body hay JSON
                }
                else // Serie 400 o 500
                {
                    ViewBag.Mesaje = body; // en el body vino el mensaje del error
                }
            }
            catch (Exception)
            {
                ViewBag.Mensaje = "Ocurrió un error inesperado. Intente de nuevo más tarde.";
            }

            return View(usuarios);
        }

        // GET: UsuariosController/Details/5
        public ActionResult Details(int id)
        {
            UsuarioDTO usuario = null;

            try
            {
                string token = HttpContext.Session.GetString("token");
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var tarea1 = cliente.GetAsync(urlApiUsuarios + "/" + id);
                tarea1.Wait();

                var tarea2 = tarea1.Result.Content.ReadAsStringAsync();
                tarea2.Wait();

                if (tarea1.Result.IsSuccessStatusCode)
                {
                    usuario = JsonConvert.DeserializeObject<UsuarioDTO>(tarea2.Result);
                }
                else
                {
                    ViewBag.Mensaje = tarea2.Result;
                }
            }
            catch (Exception)
            {

                ViewBag.Mensaje = "Ocurrió un error inesperado";
            }


            return View(usuario);
        }

        // GET: UsuariosController/Create
        public ActionResult Create()           
        {
            return View();
        }

        // POST: UsuariosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsuarioDTO nuevo)
        {
            string token = HttpContext.Session.GetString("token");
            if (ModelState.IsValid)
            {
                string json = JsonConvert.SerializeObject(nuevo);
                HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(urlApiUsuarios, "POST", json, token);

                if (respuesta.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Mensaje = AuxiliarClienteHttp.ObtenerBody(respuesta);
                }
            }
            else
            {
                ViewBag.Mensaje = "Datos inválidos";
            }
            return View(nuevo);
        }

        // GET: UsuariosController/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            UsuarioDTO usuario = null;
            string token = HttpContext.Session.GetString("token");
            HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(urlApiUsuarios + "/" + id, "GET", null, token);
            string body = AuxiliarClienteHttp.ObtenerBody(respuesta);

            if (respuesta.IsSuccessStatusCode)
            {
                usuario = JsonConvert.DeserializeObject<UsuarioDTO>(body);
            }
            else
            {
                ViewBag.Mensaje = body;
            }
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UsuarioDTO usuarioEditado)
        {
            string token = HttpContext.Session.GetString("token");
            string json = JsonConvert.SerializeObject(usuarioEditado);
            HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(urlApiUsuarios + "/" + id, "PUT", json, token);

            if (respuesta.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Mensaje = AuxiliarClienteHttp.ObtenerBody(respuesta);
            }
            return View(usuarioEditado);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            UsuarioDTO usuario = null;

            string token = HttpContext.Session.GetString("token");
            HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(urlApiUsuarios + "/" + id, "GET", null, token);

            string body = AuxiliarClienteHttp.ObtenerBody(respuesta);

            if (respuesta.IsSuccessStatusCode) // Serie 200 
            {
                usuario = JsonConvert.DeserializeObject<UsuarioDTO>(body); // en el body hay JSON
            }
            else // Serie 400 o 500
            {
                ViewBag.Mensaje = body; // en el body vino el mensaje del error
            }

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, UsuarioDTO usuario)
        {
            HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(urlApiUsuarios + "/" + id, "DELETE");

            if (respuesta.IsSuccessStatusCode) // Serie 200
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Mesaje = AuxiliarClienteHttp.ObtenerBody(respuesta);
            }

            return View(usuario);
        }



        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }



        public ActionResult UsuariosQueSuperanMonto(int monto)
        {
            if (HttpContext.Session.GetString("loggedId") == null)
            {
                return RedirectToAction("Login", "Usuarios");
            }
            string rol = HttpContext.Session.GetString("loggedRol");
            if (rol != "ADMIN" && rol != "GERENTE")
            {
                ViewBag.Error = "No tiene permisos para acceder a esta función";
                return RedirectToAction("Index", "Home");
            }

            if (monto == 0)
            {

                return View(new List<UsuarioDTO>());
            }

            try
            {
                string token = HttpContext.Session.GetString("token");
                HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(
                    urlApiUsuarios + "/superan-monto/" + monto, "GET", null, token);

                string body = AuxiliarClienteHttp.ObtenerBody(respuesta);
                List<UsuarioDTO> usuarios = new List<UsuarioDTO>();

                if (respuesta.IsSuccessStatusCode)
                {
                    usuarios = JsonConvert.DeserializeObject<List<UsuarioDTO>>(body);
                    if (!usuarios.Any())
                        ViewBag.Mensaje = $"No se encontraron usuarios con pagos únicos mayores a ${monto}";
                }
                else
                {
                    ViewBag.Error = body;
                }

                ViewBag.Monto = monto;
                return View(usuarios);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<UsuarioDTO>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(int id)
        {
            try
            {
                string rol = HttpContext.Session.GetString("loggedRol");
                if (rol != "ADMIN")
                {
                    ViewBag.Mensaje = "No tiene permisos para realizar esta acción";
                    return RedirectToAction(nameof(Index));
                }

                string token = HttpContext.Session.GetString("token");
                HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(
                    urlApiUsuarios + "/reset-password/" + id, "POST", null, token);

                string body = AuxiliarClienteHttp.ObtenerBody(respuesta);

                if (respuesta.IsSuccessStatusCode)
                {
                    ViewBag.Mensaje = $"La nueva contraseña generada es: {body}";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Mensaje = body;
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Error inesperado: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
