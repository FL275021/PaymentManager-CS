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
using System.Net.Http.Headers;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebMVC.Controllers
{
    public class EquiposController : Controller
    {
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        private readonly string URLApiEquipos;

        public EquiposController(IConfiguration configuration)
        {
            URLApiEquipos = configuration.GetSection("ConnectionStrings:URLApiEquipos").Value;
        }
        // GET: EquiposController
        public ActionResult Index()
        {
            IEnumerable<EquipoDTO> equipos = new List<EquipoDTO>();
            try
            {
                string token = HttpContext.Session.GetString("token");
                HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(URLApiEquipos, "GET", null, token);
                string body = AuxiliarClienteHttp.ObtenerBody(respuesta);

                if (respuesta.IsSuccessStatusCode) // Serie 200
                {
                    equipos = JsonConvert.DeserializeObject<IEnumerable<EquipoDTO>>(body); // en el body hay JSON
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

            return View(equipos);
        }

        // GET: EquiposController/Details/5
        public ActionResult Details(int id)
        {
            EquipoDTO equipo = null;

            try
            {
                string token = HttpContext.Session.GetString("token");
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var tarea1 = cliente.GetAsync(URLApiEquipos + "/" + id);
                tarea1.Wait();

                var tarea2 = tarea1.Result.Content.ReadAsStringAsync();
                tarea2.Wait();

                if (tarea1.Result.IsSuccessStatusCode)
                {
                    equipo = JsonConvert.DeserializeObject<EquipoDTO>(tarea2.Result);
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


            return View(equipo);
        }

        // GET: EquiposController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EquiposController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EquipoDTO nuevo)
        {
            string token = HttpContext.Session.GetString("token");
            if (ModelState.IsValid)
            {
                string json = JsonConvert.SerializeObject(nuevo);
                HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(URLApiEquipos, "POST", json, token);

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

        // GET: EquiposController/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            EquipoDTO equipo = null;
            string token = HttpContext.Session.GetString("token");
            HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(URLApiEquipos + "/" + id, "GET", null, token);
            string body = AuxiliarClienteHttp.ObtenerBody(respuesta);

            if (respuesta.IsSuccessStatusCode)
            {
                equipo = JsonConvert.DeserializeObject<EquipoDTO>(body);
            }
            else
            {
                ViewBag.Mensaje = body;
            }
            return View(equipo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EquipoDTO equipoEditado)
        {
            string token = HttpContext.Session.GetString("token");
            string json = JsonConvert.SerializeObject(equipoEditado);
            HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(URLApiEquipos + "/" + id, "PUT", json, token);

            if (respuesta.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Mensaje = AuxiliarClienteHttp.ObtenerBody(respuesta);
            }
            return View(equipoEditado);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            EquipoDTO equipo = null;

            string token = HttpContext.Session.GetString("token");
            HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(URLApiEquipos + "/" + id, "GET", null, token);

            string body = AuxiliarClienteHttp.ObtenerBody(respuesta);

            if (respuesta.IsSuccessStatusCode) // Serie 200 
            {
                equipo = JsonConvert.DeserializeObject<EquipoDTO>(body); // en el body hay JSON
            }
            else // Serie 400 o 500
            {
                ViewBag.Mensaje = body; // en el body vino el mensaje del error
            }

            return View(equipo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, EquipoDTO equipo)
        {
            HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(URLApiEquipos + "/" + id, "DELETE");

            if (respuesta.IsSuccessStatusCode) // Serie 200
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Mesaje = AuxiliarClienteHttp.ObtenerBody(respuesta);
            }

            return View(equipo);
        }








    }
}
