// Este controlador administra las solicitudes relacionadas con las personas.
// Recibe las peticiones del usuario, utiliza el servicio de personas para
// ejecutar la lógica de negocio y devuelve las vistas correspondientes.

using CIEMPOS.Models;
using CIEMPOS.Services;
using Microsoft.AspNetCore.Mvc;

namespace CIEMPOS.Controllers
{
    public class PersonaController : Controller
    {
        // Servicio de personas
        private readonly PersonaService _personaService;

        // Constructor con Dependency Injection
        public PersonaController(PersonaService personaService)
        {
            _personaService = personaService;
        }

        // Muestra la lista de personas
        public IActionResult Index(bool mostrarInactivos = false)
        {
            // Obtiene la lista de personas
            var personas = _personaService.GetAll(mostrarInactivos);

            // Envía la información a la vista
            return View(personas);
        }

        // Muestra el formulario para registrar una persona
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Registra una nueva persona
        [HttpPost]
        public IActionResult Create(TbPersona persona)
        {
            // Valida los datos enviados desde el formulario
            if (!ModelState.IsValid)
                return View(persona);

            try
            {
                // Intenta registrar la persona
                if (_personaService.Create(persona))
                    return RedirectToAction(nameof(Index));

                ViewBag.Error = "No fue posible registrar la persona.";
            }
            catch (Exception ex)
            {
                // Muestra el mensaje generado por la lógica de negocio
                ViewBag.Error = ex.Message;
            }

            return View(persona);
        }

        // Muestra el formulario para editar una persona
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Obtiene la persona por su Id
            TbPersona? persona = _personaService.GetById(id);

            // Valida que la persona exista
            if (persona == null)
                return NotFound();

            // Envía la persona a la vista
            return View(persona);
        }

        // Guarda los cambios realizados a la persona
        [HttpPost]
        public IActionResult Edit(TbPersona persona)
        {
            // Valida los datos enviados desde el formulario
            if (!ModelState.IsValid)
                return View(persona);

            try
            {
                // Intenta actualizar la persona
                if (_personaService.Update(persona))
                    return RedirectToAction(nameof(Index));

                ViewBag.Error = "No fue posible actualizar la persona.";
            }
            catch (Exception ex)
            {
                // Muestra el mensaje generado por la lógica de negocio
                ViewBag.Error = ex.Message;
            }

            return View(persona);
        }
    }
}