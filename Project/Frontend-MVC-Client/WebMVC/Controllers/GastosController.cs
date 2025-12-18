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
    public class GastosController : Controller
    {
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        private readonly string URLApiGastos;

        public GastosController(IConfiguration configuration)
        {
            URLApiGastos = configuration.GetSection("ConnectionStrings:URLApiGastos").Value;
        }
        // GET: GastosController
        public ActionResult Index()
        {
            IEnumerable<GastoDTO> gastos = new List<GastoDTO>();
            try
            {
                string token = HttpContext.Session.GetString("token");
                HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(URLApiGastos, "GET", null, token);
                string body = AuxiliarClienteHttp.ObtenerBody(respuesta);

                if (respuesta.IsSuccessStatusCode) // Serie 200
                {
                    gastos = JsonConvert.DeserializeObject<IEnumerable<GastoDTO>>(body); // en el body hay JSON
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

            return View(gastos);
        }

        // GET: GastosController/Details/5
        public ActionResult Details(int id)
        {
            GastoDTO gasto = null;

            try
            {
                string token = HttpContext.Session.GetString("token");
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var tarea1 = cliente.GetAsync(URLApiGastos + "/" + id);
                tarea1.Wait();

                var tarea2 = tarea1.Result.Content.ReadAsStringAsync();
                tarea2.Wait();

                if (tarea1.Result.IsSuccessStatusCode)
                {
                    gasto = JsonConvert.DeserializeObject<GastoDTO>(tarea2.Result);
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


            return View(gasto);
        }

        // GET: GastosController/Create
        public ActionResult Create()           
        {
           return View();
        }

        // POST: GastosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GastoDTO nuevo)
        {
            string token = HttpContext.Session.GetString("token");
            if (ModelState.IsValid)
            {
                string json = JsonConvert.SerializeObject(nuevo);
                HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(URLApiGastos, "POST", json, token);

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

        // GET: GastosController/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            GastoDTO gasto = null;
            string token = HttpContext.Session.GetString("token");
            HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(URLApiGastos + "/" + id, "GET", null, token);
            string body = AuxiliarClienteHttp.ObtenerBody(respuesta);

            if (respuesta.IsSuccessStatusCode)
            {
                gasto = JsonConvert.DeserializeObject<GastoDTO>(body);
            }
            else
            {
                ViewBag.Mensaje = body;
            }
            return View(gasto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, GastoDTO gastoEditado)
        {
            string token = HttpContext.Session.GetString("token");
            string json = JsonConvert.SerializeObject(gastoEditado);
            HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(URLApiGastos + "/" + id, "PUT", json, token);

            if (respuesta.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Mensaje = AuxiliarClienteHttp.ObtenerBody(respuesta);
            }
            return View(gastoEditado);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            GastoDTO gasto = null;

            string token = HttpContext.Session.GetString("token");
            HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(URLApiGastos + "/" + id, "GET", null, token);

            string body = AuxiliarClienteHttp.ObtenerBody(respuesta);

            if (respuesta.IsSuccessStatusCode) // Serie 200 
            {
                gasto = JsonConvert.DeserializeObject<GastoDTO>(body); // en el body hay JSON
            }
            else // Serie 400 o 500
            {
                ViewBag.Mensaje = body; // en el body vino el mensaje del error
            }

            return View(gasto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, GastoDTO gasto)
        {
            HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(URLApiGastos + "/" + id, "DELETE");

            if (respuesta.IsSuccessStatusCode) // Serie 200
            {
                return RedirectToAction(nameof(Index)); 
            }
            else
            {
                ViewBag.Mesaje = AuxiliarClienteHttp.ObtenerBody(respuesta);
            }

            return View(gasto);
        }



       

  


    }
}
