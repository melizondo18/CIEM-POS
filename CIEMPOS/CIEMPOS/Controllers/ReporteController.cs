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
            // Verifica que el usuario tenga acceso al módulo
            if (!Helper.TieneAccesoReportes(IdRol))
                return RedirectToAction("Index", "Home");

            return View();
        }
    }
}