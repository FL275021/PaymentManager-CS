using LogicaAccesoDatos;
using LogicaNegocio.EntidadesDominio;
using Newtonsoft.Json;
using WebMVC.Auxiliares;
using WebMVC.DTOs;
using LogicaAplicacion.InterfacesCasosUso;
using LogicaAplicacion.Mapeadores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.InterfacesRepositorios;
using WebMVC.Models.Pagos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Net.Http.Headers;

namespace WebMVC.Controllers
{
    public class PagosController : Controller
    {
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        private readonly string URLApiPagos;

        public PagosController(IConfiguration configuration)
        {
            URLApiPagos = configuration.GetSection("ConnectionStrings:URLApiPagos").Value;
        }
        // GET: PagosController
        public ActionResult Index()
        {
            string rol = HttpContext.Session.GetString("loggedRol");
            int? id = HttpContext.Session.GetInt32("loggedId");

            if (rol != "EMPLEADO" && rol != "GERENTE")
            {
                ViewBag.Mensaje = "No tiene permisos para esta función.";
                return RedirectToAction("Login", "Usuarios");
            }

            if (id == null || id == 0)
            {
                ViewBag.Mensaje = "Usuario no válido.";
                return RedirectToAction("Login", "Usuarios");
            }

            return RedirectToAction(nameof(PagosDeUsuario), new { usuarioId = id.Value });
        }

        ////RF2 - ob2 "Deberá controlar que el usuario utilizado para el filtro sea el usuario que envía la solicitud." ????
        public IActionResult PagosDeUsuario(Usuario usuario)
        {
            int? loggedId = HttpContext.Session.GetInt32("loggedId");
            string rol = HttpContext.Session.GetString("loggedRol");
            if ((rol == "EMPLEADO" || rol == "GERENTE") && usuario.Id == loggedId)
            {
                string token = HttpContext.Session.GetString("token");
                HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(
                    URLApiPagos + "/usuario/" + usuario.Id, "GET", null, token);

                string body = AuxiliarClienteHttp.ObtenerBody(respuesta);
                List<PagoDTO> pagos = new List<PagoDTO>();

                if (respuesta.IsSuccessStatusCode)
                {
                    pagos = JsonConvert.DeserializeObject<List<PagoDTO>>(body);
                    if (pagos == null || pagos.Count == 0)
                        ViewBag.Mensaje = "No se encontraron pagos para este usuario.";
                }
                else
                {
                    ViewBag.Mensaje = body;
                }

                return View(pagos);
            }
            else
            {
                ViewBag.Mensaje = "No tiene permisos para ver los pagos de este usuario.";
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: PagosController/Details/5
        public ActionResult Details(int id)
        {
            PagoDTO pago = null;

            try
            {
                string token = HttpContext.Session.GetString("token");
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var tarea1 = cliente.GetAsync(URLApiPagos + "/" + id);
                tarea1.Wait();

                var tarea2 = tarea1.Result.Content.ReadAsStringAsync();
                tarea2.Wait();

                if (tarea1.Result.IsSuccessStatusCode)
                {
                    pago = JsonConvert.DeserializeObject<PagoDTO>(tarea2.Result);
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


            return View(pago);
        }

        // GET: PagosController/Create
        public ActionResult Create()
        {
/*             PagoDTO nuevo = new PagoDTO();

            nuevo.Usuarios = ListadoUsuariosCU.ObtenerListado();
            nuevo.Gastos = ListadoGastos.ObtenerListado();
            Console.WriteLine($"Usuarios count: {nuevo.Usuarios?.Count() ?? 0}");
            Console.WriteLine($"Gastos count: {nuevo.Gastos?.Count() ?? 0}");
 */
            return View();
        }

        // POST: PagosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PagoDTO nuevo)
        {
            string token = HttpContext.Session.GetString("token");
            if (ModelState.IsValid)
            {
                string json = JsonConvert.SerializeObject(nuevo);
                HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(
                    URLApiPagos, "POST", json, token);

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
            // borrar en prod
          /*   nuevo.Usuarios = ListadoUsuariosCU.ObtenerListado();
            nuevo.Gastos = ListadoGastos.ObtenerListado(); */
            return View(nuevo);
        }


        // GET: PagosController/Edit/5
        public ActionResult Edit(int id)
        {
            PagoDTO pago = null;
            string token = HttpContext.Session.GetString("token");
            HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(URLApiPagos + "/" + id, "GET", null, token);
            string body = AuxiliarClienteHttp.ObtenerBody(respuesta);

            if (respuesta.IsSuccessStatusCode)
            {
                pago = JsonConvert.DeserializeObject<PagoDTO>(body);
            }
            else
            {
                ViewBag.Mensaje = body;
            }
            return View(pago);
        }

        // POST: PagosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PagoDTO pagoEditado)
        {
            string token = HttpContext.Session.GetString("token");
            string json = JsonConvert.SerializeObject(pagoEditado);
            HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(URLApiPagos + "/" + id, "PUT", json, token);

            if (respuesta.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Mensaje = AuxiliarClienteHttp.ObtenerBody(respuesta);
            }
            return View(pagoEditado);
        }

        // GET: PagosController/Delete/5
        public ActionResult Delete(int id)
        {
            PagoDTO pago = null;

            string token = HttpContext.Session.GetString("token");
            HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(URLApiPagos + "/" + id, "GET", null, token);

            string body = AuxiliarClienteHttp.ObtenerBody(respuesta);

            if (respuesta.IsSuccessStatusCode) // Serie 200 
            {
                pago = JsonConvert.DeserializeObject<PagoDTO>(body); // en el body hay JSON
            }
            else // Serie 400 o 500
            {
                ViewBag.Mensaje = body; // en el body vino el mensaje del error
            }

            return View(pago);
        }

        // POST: PagosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, PagoDTO pago)
        {
            HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(URLApiPagos + "/" + id, "DELETE");

            if (respuesta.IsSuccessStatusCode) // Serie 200
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Mesaje = AuxiliarClienteHttp.ObtenerBody(respuesta);
            }

            return View(pago);
        }

        //RF4 - ob1
        public ActionResult ListadoMensual(int mes, int año)
        {
            List<PagoDTO> pagos = new List<PagoDTO>();
            string token = HttpContext.Session.GetString("token");

            HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(
                URLApiPagos + $"/mensual?mes={mes}&año={año}", "GET", null, token);

            string body = AuxiliarClienteHttp.ObtenerBody(respuesta);

            if (respuesta.IsSuccessStatusCode)
            {
                pagos = JsonConvert.DeserializeObject<List<PagoDTO>>(body);
                if (pagos == null || pagos.Count == 0)
                    ViewBag.Mensaje = "No existen pagos para el mes y año seleccionados.";
            }
            else
            {
                ViewBag.Mensaje = body;
            }

            return View(pagos);
        }




        private string GetUsuarioActual()
        {
            return HttpContext.Session.GetString("usuario") ?? "Desconocido";
        }

    }
}
