/*
 * Nombre del archivo: LogInController.cs
 * Descripción: Controlador encargado del proceso de autenticación de los usuarios,
 * permitiendo el inicio de sesión mediante la validación de sus credenciales.
 */

using CIEMPOS.Models;
using CIEMPOS.Services;
using Microsoft.AspNetCore.Mvc;

namespace CIEMPOS.Controllers
{
    public class LogInController : Controller
    {
        // Servicio que contiene la lógica de autenticación
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

                // Verifica si debe cambiar la contraseña
                if (_logInService.MustChangePassword(usuario))
                {
                    return RedirectToAction("CambiarContrasena");
                }

                // Redirige al menú principal
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View();
            }
        }
    }
}