/****************************************************************************
 * HelpController.cs
 * Controlador encargado de mostrar la ayuda del sistema.
 ***************************************************************************/

using Microsoft.AspNetCore.Mvc;

namespace CIEMPOS.Controllers
{
    public class HelpController : Controller
    {
        // GET: Help
        public IActionResult Index()
        {
            return View();
        }
    }
}