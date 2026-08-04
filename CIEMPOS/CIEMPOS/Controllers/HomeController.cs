/* Controlador encargado de mostrar la página principal del sistema
 * y administrar la navegación inicial según el rol del usuario
 * autenticado.
 */

using System.Diagnostics;
using CIEMPOS.Models;
using Microsoft.AspNetCore.Mvc;
using CIEMPOS.Helpers;

namespace CIEMPOS.Controllers
{
    public class HomeController : Controller
    {
        // Logger de la aplicación
        private readonly ILogger<HomeController> _logger;

        // Constructor con Dependency Injection
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Obtiene el rol del usuario autenticado
        private int? IdRol
        {
            get
            {
                return HttpContext.Session.GetInt32("IdRol");
            }
        }

        // Obtiene el nombre completo del usuario autenticado
        private string? NombreCompleto
        {
            get
            {
                return HttpContext.Session.GetString("NombreCompleto");
            }
        }

        // Obtiene el nombre del rol del usuario autenticado
        private string? NombreRol
        {
            get
            {
                return HttpContext.Session.GetString("NombreRol");
            }
        }

        // Muestra la página principal del sistema
        public IActionResult Index()
        {
            if (!Helper.SesionActiva(HttpContext.Session.GetInt32("IdUsuario")))
            {
                TempData["Error"] = "La sesión ha expirado por inactividad. Inicie sesión nuevamente.";

                return RedirectToAction("Index", "LogIn");
            }

            ViewBag.IdRol = IdRol;
            ViewBag.NombreCompleto = NombreCompleto;
            ViewBag.NombreRol = NombreRol;

            return View();
        }

        // Muestra la política de privacidad
        public IActionResult Privacy()
        {
            return View();
        }

        // Muestra la página de error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}