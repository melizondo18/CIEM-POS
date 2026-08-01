using CIEMPOS.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CIEMPOS.Controllers
{
    public class PrescripcionController : Controller
    {
        // Obtiene el rol del usuario autenticado
        private int? IdRol
        {
            get
            {
                return HttpContext.Session.GetInt32("IdRol");
            }
        }

        // Muestra el módulo de Prescripciones
        public IActionResult Index()
        {
            // Verifica que el usuario tenga acceso al módulo
            if (!Helper.TieneAccesoPrescripciones(IdRol))
                return RedirectToAction("Index", "Home");

            return View();
        }
    }
}