/* Controlador encargado de gestionar el proceso de
 * autenticación de los usuarios, incluyendo el inicio de sesión,
 * el cambio de contraseña en el primer ingreso y el cierre de sesión.
 */

using CIEMPOS.Models;
using CIEMPOS.Services;
using Microsoft.AspNetCore.Mvc;

namespace CIEMPOS.Controllers
{
    public class LogInController : Controller
    {
        // Servicio de autenticación
        private readonly LogInService _logInService;

        // Constructor con Dependency Injection
        public LogInController(LogInService logInService)
        {
            _logInService = logInService;
        }

        // Muestra la pantalla de inicio de sesión
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Procesa las credenciales ingresadas por el usuario
        [HttpPost]
        public IActionResult Index(string identificacion, string password)
        {
            try
            {
                // Autentica al usuario
                TbUsuario usuario = _logInService.Authenticate(identificacion, password);

                // Guarda la información del usuario en la sesión
                HttpContext.Session.SetInt32("IdUsuario", usuario.IdUsuario);
                HttpContext.Session.SetInt32("IdRol", usuario.IdRol);

                HttpContext.Session.SetString(
                    "NombreRol",
                    usuario.IdRolNavigation.Nombre);

                HttpContext.Session.SetString(
                    "NombreCompleto",
                    $"{usuario.IdPersonaNavigation.Nombre} {usuario.IdPersonaNavigation.Apellido}");

                // Si es el primer ingreso, redirige al cambio de contraseña
                if (_logInService.MustChangePassword(usuario))
                {
                    return RedirectToAction("CambiarContrasena");
                }

                // Muestra un mensaje de bienvenida
                TempData["Success"] = "Bienvenido al sistema.";

                // Redirige al menú principal
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // Muestra la pantalla para cambiar la contraseña
        [HttpGet]
        public IActionResult CambiarContrasena()
        {
            return View();
        }

        // Procesa el cambio de contraseña
        [HttpPost]
        public IActionResult CambiarContrasena(string nuevaContrasena,
                                               string confirmarContrasena)
        {
            try
            {
                // Obtiene el usuario autenticado desde la sesión
                int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");

                // Verifica que exista una sesión activa
                if (idUsuario == null)
                    throw new Exception("La sesión ha expirado. Inicie sesión nuevamente.");

                // Actualiza la contraseña
                _logInService.ChangePassword(
                    idUsuario.Value,
                    nuevaContrasena,
                    confirmarContrasena);

                // Muestra un mensaje de éxito
                TempData["Success"] = "La contraseña se actualizó correctamente. Bienvenido al sistema.";

                // Redirige al menú principal
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // Cierra la sesión del usuario
        [HttpGet]
        public IActionResult LogOut()
        {
            // Elimina la información almacenada en la sesión
            HttpContext.Session.Clear();

            // Muestra un mensaje informativo
            TempData["Success"] = "La sesión se cerró correctamente.";

            // Redirige a la página principal
            return RedirectToAction("Index", "Home");
        }
    }
}