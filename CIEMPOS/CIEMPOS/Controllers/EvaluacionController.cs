/* Controlador encargado de administrar las operaciones
 * relacionadas con las evaluaciones físicas del sistema.
 */

using CIEMPOS.Helpers;
using CIEMPOS.Models;
using CIEMPOS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CIEMPOS.Controllers
{
    public class EvaluacionController : Controller
    {
        // Servicios
        private readonly EvaluacionService _evaluacionService;
        private readonly PacienteService _pacienteService;

        // Constructor con Dependency Injection
        public EvaluacionController(
            EvaluacionService evaluacionService,
            PacienteService pacienteService)
        {
            _evaluacionService = evaluacionService;
            _pacienteService = pacienteService;
        }

        // Obtiene el rol del usuario autenticado
        private int? IdRol
        {
            get
            {
                return HttpContext.Session.GetInt32("IdRol");
            }
        }

        // Muestra el listado de evaluaciones
        public IActionResult Index(
            int? idPaciente,
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            // Verifica que el usuario tenga acceso al módulo
            if (!Helper.TieneAccesoEvaluaciones(IdRol))
                return RedirectToAction("Index", "Home");

            IEnumerable<TbEvaluacionFisica> evaluaciones =
                _evaluacionService.GetAll(
                    idPaciente,
                    fechaInicio,
                    fechaFin);

            CargarListas(idPaciente);

            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = fechaFin;

            return View(evaluaciones);
        }

        // Muestra el formulario para registrar una evaluación
        [HttpGet]
        public IActionResult Create()
        {
            // Verifica que el usuario tenga acceso al módulo
            if (!Helper.TieneAccesoEvaluaciones(IdRol))
                return RedirectToAction("Index", "Home");

            CargarListas();

            return View();
        }

        // Registra una nueva evaluación
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TbEvaluacionFisica evaluacion)
        {
            // Verifica que el usuario tenga acceso al módulo
            if (!Helper.TieneAccesoEvaluaciones(IdRol))
                return RedirectToAction("Index", "Home");

            try
            {
                // Estas propiedades son cargadas por Entity Framework
                ModelState.Remove(nameof(TbEvaluacionFisica.IdPacienteNavigation));
                ModelState.Remove(nameof(TbEvaluacionFisica.IdUsuarioNavigation));

                if (!ModelState.IsValid)
                {
                    CargarListas();
                    return View(evaluacion);
                }

                _evaluacionService.Create(evaluacion);

                TempData["Success"] =
                    "La evaluación fue registrada correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

                CargarListas();

                return View(evaluacion);
            }
        }

        // Muestra el formulario para editar una evaluación
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Verifica que el usuario tenga acceso al módulo
            if (!Helper.TieneAccesoEvaluaciones(IdRol))
                return RedirectToAction("Index", "Home");

            TbEvaluacionFisica? evaluacion =
                _evaluacionService.GetById(id);

            if (evaluacion == null)
                return NotFound();

            CargarListas();

            return View(evaluacion);
        }

        // Actualiza una evaluación
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TbEvaluacionFisica evaluacion)
        {
            // Verifica que el usuario tenga acceso al módulo
            if (!Helper.TieneAccesoEvaluaciones(IdRol))
                return RedirectToAction("Index", "Home");

            try
            {
                // Estas propiedades son cargadas por Entity Framework
                ModelState.Remove(nameof(TbEvaluacionFisica.IdPacienteNavigation));
                ModelState.Remove(nameof(TbEvaluacionFisica.IdUsuarioNavigation));

                if (!ModelState.IsValid)
                {
                    CargarListas();
                    return View(evaluacion);
                }

                _evaluacionService.Update(evaluacion);

                TempData["Success"] =
                    "La evaluación fue actualizada correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

                CargarListas();

                return View(evaluacion);
            }
        }

        // Muestra el detalle de una evaluación
        [HttpGet]
        public IActionResult Ver(int id)
        {
            // Verifica que el usuario tenga acceso al módulo
            if (!Helper.TieneAccesoEvaluaciones(IdRol))
                return RedirectToAction("Index", "Home");

            TbEvaluacionFisica? evaluacion =
                _evaluacionService.GetById(id);

            if (evaluacion == null)
                return NotFound();

            return View(evaluacion);
        }

        // Carga las listas utilizadas por las vistas
        private void CargarListas(int? idPaciente = null)
        {
            ViewBag.Pacientes = new SelectList(
                _pacienteService.GetAll()
                    .Select(p => new
                    {
                        p.IdPaciente,
                        NombreCompleto = $"{p.IdPersonaNavigation.Nombre} {p.IdPersonaNavigation.Apellido}"
                    }),
                "IdPaciente",
                "NombreCompleto",
                idPaciente);
        }
    }
}