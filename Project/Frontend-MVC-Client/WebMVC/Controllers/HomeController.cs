using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Diagnostics;
using WebMVC.Models;
using Newtonsoft.Json;
using WebMVC.Auxiliares;
using WebMVC.DTOs;
using WebMVC.Filters;


namespace WebMVC.Controllers
{
	public class HomeController : Controller
	{
		public string URLApiLogin { get; set; }

		public HomeController(IConfiguration config)
		{
			URLApiLogin = config.GetValue<string>("ConnectionStrings:URLApiLogin");
		}

		/* 	public IActionResult Index()
			{
				return View(Login);
			} */

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Login(UsuarioDTO model)
		{
			try
			{
				LoginDTO logueado = new LoginDTO
				{
					Email = model.Email,
					Password = model.Password
				};
				HttpResponseMessage respuesta = AuxiliarClienteHttp.EnviarSolicitud(URLApiLogin, "POST", logueado);
				string body = AuxiliarClienteHttp.ObtenerBody(respuesta);
				if (respuesta.IsSuccessStatusCode)
				{
					UsuarioDTO usuario = JsonConvert.DeserializeObject<UsuarioDTO>(body);
					HttpContext.Session.SetString("loggedEmail", usuario.Email);
					HttpContext.Session.SetInt32("loggedId", usuario.Id);
					HttpContext.Session.SetString("loggedRol", usuario.Rol.Nombre);
					HttpContext.Session.SetString("token", usuario.Token);
					return RedirectToAction("Index", "Pago");
				}
				else
				{
					ViewBag.Mensaje = body;
					return View("Login", model);
				}
			}
			catch (Exception ex)
			{
				ViewBag.Error = "Sucedió un error inesperado: " + ex.Message;
				return View("Login", model);
			}
		}
	}
}
