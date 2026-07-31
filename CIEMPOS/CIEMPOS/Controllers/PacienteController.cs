/*
 * Nombre del archivo: PacienteController.cs
 * Descripción: Controlador encargado de administrar las operaciones
 * relacionadas con los pacientes del sistema.
 */

using CIEMPOS.Models;
using CIEMPOS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CIEMPOS.Controllers
{
    public class PacienteController : Controller
    {
        // Servicios
        private readonly PacienteService _pacienteService;
        private readonly PersonaService _personaService;

        // Constructor con Dependency Injection
        public PacienteController(
            PacienteService pacienteService,
            PersonaService personaService)
        {
            _pacienteService = pacienteService;
            _personaService = personaService;
        }

        // Muestra el listado de pacientes
        public IActionResult Index(bool mostrarInactivos = false)
        {
            IEnumerable<TbPaciente> pacientes =
                _pacienteService.GetAll(mostrarInactivos);

            ViewBag.MostrarInactivos = mostrarInactivos;

            return View(pacientes);
        }

        // Muestra el formulario para registrar un paciente
        [HttpGet]
        public IActionResult Create()
        {
            CargarListas();

            return View();
        }

        // Registra un nuevo paciente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TbPaciente paciente)
        {
            try
            {
                ModelState.Remove(nameof(TbPaciente.IdPersonaNavigation));

                if (!ModelState.IsValid)
                {
                    CargarListas();
                    return View(paciente);
                }

                _pacienteService.Create(paciente);

                TempData["Success"] =
                    "El paciente fue registrado correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

                CargarListas();

                return View(paciente);
            }
        }

        // Muestra el formulario para editar un paciente
        [HttpGet]
        public IActionResult Edit(int id)
        {
            TbPaciente? paciente = _pacienteService.GetById(id);

            if (paciente == null)
                return NotFound();

            CargarListas();

            return View(paciente);
        }

        // Actualiza un paciente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TbPaciente paciente)
        {
            try
            {
                // Entity Framework carga esta propiedad automáticamente
                ModelState.Remove(nameof(TbPaciente.IdPersonaNavigation));

                if (!ModelState.IsValid)
                {
                    CargarListas();
                    return View(paciente);
                }

                _pacienteService.Update(paciente);

                TempData["Success"] =
                    "El paciente fue actualizado correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

                CargarListas();

                return View(paciente);
            }
        }

        // Carga las listas utilizadas por las vistas
        private void CargarListas()
        {
            ViewBag.Personas = new SelectList(
                _personaService.GetDisponiblesParaPaciente(),
                "IdPersona",
                "Nombre");
        }
    }
}