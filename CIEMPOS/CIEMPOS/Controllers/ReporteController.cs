using CIEMPOS.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CIEMPOS.Controllers
{
    public class ReporteController : Controller
    {
        // Obtiene el rol del usuario autenticado
        private int? IdRol
        {
            get
            {
                return HttpContext.Session.GetInt32("IdRol");
            }
        }

        // Muestra el módulo de Reportes
        public IActionResult Index()
        {
            // Verifica que exista una sesión activa
            if (!Helper.SesionActiva(HttpContext.Session.GetInt32("IdUsuario")))
            {
                TempData["Error"] = "La sesión ha expirado por inactividad. Inicie sesión nuevamente.";
                return RedirectToAction("Index", "LogIn");
            }

            // Verifica que el usuario tenga acceso al módulo
            if (!Helper.TieneAccesoReportes(IdRol))
            {
                TempData["Error"] = "No tiene permisos para acceder a este módulo.";
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}