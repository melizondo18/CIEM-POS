/*
 * Nombre del archivo: UsuarioController.cs
 * Descripción: Controlador encargado de administrar las operaciones
 * relacionadas con los usuarios del sistema.
 */

using CIEMPOS.Helpers;
using CIEMPOS.Models;
using CIEMPOS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CIEMPOS.Controllers
{
    public class UsuarioController : Controller
    {
        // Servicios
        private readonly UsuarioService _usuarioService;
        private readonly PersonaService _personaService;
        private readonly RolService _rolService;

        // Constructor con Dependency Injection
        public UsuarioController(
            UsuarioService usuarioService,
            PersonaService personaService,
            RolService rolService)
        {
            _usuarioService = usuarioService;
            _personaService = personaService;
            _rolService = rolService;
        }

        // Obtiene el rol del usuario autenticado
        private int? IdRol
        {
            get
            {
                return HttpContext.Session.GetInt32("IdRol");
            }
        }

        // Muestra el listado de usuarios
        public IActionResult Index(bool mostrarInactivos = false)
        {
            // Verifica que el usuario tenga acceso al módulo
            if (!Helper.TieneAccesoUsuarios(IdRol))
                return RedirectToAction("Index", "Home");

            IEnumerable<TbUsuario> usuarios =
                _usuarioService.GetAll(mostrarInactivos);

            ViewBag.MostrarInactivos = mostrarInactivos;

            return View(usuarios);
        }

        // Muestra el formulario para registrar un usuario
        [HttpGet]
        public IActionResult Create()
        {
            // Verifica que el usuario tenga acceso al módulo
            if (!Helper.TieneAccesoUsuarios(IdRol))
                return RedirectToAction("Index", "Home");

            CargarListas();

            return View();
        }

        // Registra un nuevo usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TbUsuario usuario)
        {
            // Verifica que el usuario tenga acceso al módulo
            if (!Helper.TieneAccesoUsuarios(IdRol))
                return RedirectToAction("Index", "Home");

            try
            {
                if (!ModelState.IsValid)
                {
                    CargarListas();
                    return View(usuario);
                }

                _usuarioService.Create(usuario);

                TempData["Success"] =
                    "El usuario fue registrado correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

                CargarListas();

                return View(usuario);
            }
        }

        // Muestra el formulario para editar un usuario
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Verifica que el usuario tenga acceso al módulo
            if (!Helper.TieneAccesoUsuarios(IdRol))
                return RedirectToAction("Index", "Home");

            TbUsuario? usuario = _usuarioService.GetById(id);

            if (usuario == null)
                return NotFound();

            CargarListas();

            return View(usuario);
        }

        // Actualiza un usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TbUsuario usuario)
        {
            // Verifica que el usuario tenga acceso al módulo
            if (!Helper.TieneAccesoUsuarios(IdRol))
                return RedirectToAction("Index", "Home");

            try
            {
                // Estos campos no se editan desde esta vista
                ModelState.Remove(nameof(TbUsuario.Contrasena));
                ModelState.Remove(nameof(TbUsuario.ConfirmarContrasena));

                if (!ModelState.IsValid)
                {
                    CargarListas();
                    return View(usuario);
                }

                _usuarioService.Update(usuario);

                TempData["Success"] =
                    "El usuario fue actualizado correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

                CargarListas();

                return View(usuario);
            }
        }

        // Restablece la contraseña de un usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(int id)
        {
            // Verifica que el usuario tenga acceso al módulo
            if (!Helper.TieneAccesoUsuarios(IdRol))
                return RedirectToAction("Index", "Home");

            try
            {
                // Restablece la contraseña
                _usuarioService.ResetPassword(id);

                // Muestra un mensaje informativo
                TempData["Success"] =
                    $"La contraseña fue restablecida correctamente. La nueva contraseña temporal es: {Helper.PASSWORD_TEMPORAL}";

                // Regresa al listado de usuarios
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction(nameof(Index));
            }
        }

        // Carga las listas utilizadas por las vistas
        private void CargarListas()
        {
            ViewBag.Personas = new SelectList(
                _personaService.GetAll(),
                "IdPersona",
                "Nombre");

            ViewBag.Roles = new SelectList(
                _rolService.GetAll(),
                "IdRol",
                "Nombre");
        }
    }
}