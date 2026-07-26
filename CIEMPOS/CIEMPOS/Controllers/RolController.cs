// Este controlador administra las solicitudes relacionadas con los roles.
// Recibe las peticiones del usuario, utiliza el servicio de roles para
// ejecutar la lógica de negocio y devuelve las vistas correspondientes.

using CIEMPOS.Models;
using CIEMPOS.Services;
using Microsoft.AspNetCore.Mvc;

namespace CIEMPOS.Controllers
{
    public class RolController : Controller
    {
        // Servicio de roles
        private readonly RolService _rolService;

        // Constructor con Dependency Injection
        public RolController(RolService rolService)
        {
            _rolService = rolService;
        }

        // Muestra la lista de roles
        public IActionResult Index(bool mostrarInactivos = false)
        {
            // Obtiene la lista de roles
            var roles = _rolService.GetAll(mostrarInactivos);

            // Envía la información a la vista
            return View(roles);
        }

        // Muestra el formulario para registrar un rol
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Registra un nuevo rol
        [HttpPost]

        public IActionResult Create(TbRol rol)
        {
            // Valida los datos enviados desde el formulario
            if (!ModelState.IsValid)
                return View(rol);

            // Intenta registrar el rol
            if (_rolService.Create(rol))
                return RedirectToAction(nameof(Index));

            // Si ocurre un error, vuelve a mostrar el formulario
            ViewBag.Error = "No fue posible registrar el rol.";

            return View(rol);
        }

        // Muestra el formulario para editar un rol
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Obtiene el rol por su Id
            TbRol? rol = _rolService.GetById(id);

            // Valida que el rol exista
            if (rol == null)
                return NotFound();

            // Envía el rol a la vista
            return View(rol);
        }

        // Guarda los cambios realizados al rol
        [HttpPost]
        public IActionResult Edit(TbRol rol)
        {
            // Valida los datos enviados desde el formulario
            if (!ModelState.IsValid)
                return View(rol);

            // Intenta actualizar el rol
            if (_rolService.Update(rol))
                return RedirectToAction(nameof(Index));

            // Si ocurre un error, vuelve a mostrar el formulario
            ViewBag.Error = "No fue posible actualizar el rol.";

            return View(rol);
        }
    }
}