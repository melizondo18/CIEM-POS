/****************************************************************************
 * Controlador encargado de mostrar la información
 * general del sistema.
 ***************************************************************************/

using Microsoft.AspNetCore.Mvc;

namespace CIEMPOS.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        public IActionResult Index()
        {
            return View();
        }
    }
}