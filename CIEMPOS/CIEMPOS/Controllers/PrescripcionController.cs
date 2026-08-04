/* Controlador encargado de administrar las operaciones
 * relacionadas con las prescripciones del sistema.
 */

using CIEMPOS.Helpers;
using CIEMPOS.Models;
using CIEMPOS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CIEMPOS.Controllers
{
    public class PrescripcionController : Controller
    {
        // Servicios
        private readonly PrescripcionService _prescripcionService;
        private readonly PacienteService _pacienteService;

        // Constructor con Dependency Injection
        public PrescripcionController(
            PrescripcionService prescripcionService,
            PacienteService pacienteService)
        {
            _prescripcionService = prescripcionService;
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

        // Muestra el listado de prescripciones
        public IActionResult Index(
            int? idPaciente,
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            // Verifica que exista una sesión activa
            if (!Helper.SesionActiva(HttpContext.Session.GetInt32("IdUsuario")))
            {
                TempData["Error"] =
                    "La sesión ha expirado por inactividad. Inicie sesión nuevamente.";

                return RedirectToAction("Index", "LogIn");
            }

            // Verifica que el usuario tenga acceso al módulo
            if (!Helper.TieneAccesoPrescripciones(IdRol))
            {
                TempData["Error"] =
                    "No tiene permisos para acceder a este módulo.";

                return RedirectToAction("Index", "Home");
            }

            IEnumerable<TbPrescripcion> prescripciones =
                _prescripcionService.GetAll(
                    idPaciente,
                    fechaInicio,
                    fechaFin);

            CargarListas(idPaciente);

            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = fechaFin;

            return View(prescripciones);
        }

        // Muestra el formulario para registrar una prescripción
        [HttpGet]
        public IActionResult Create()
        {
            // Verifica que exista una sesión activa
            if (!Helper.SesionActiva(HttpContext.Session.GetInt32("IdUsuario")))
            {
                TempData["Error"] =
                    "La sesión ha expirado por inactividad. Inicie sesión nuevamente.";

                return RedirectToAction("Index", "LogIn");
            }

            // Verifica que el usuario tenga acceso al módulo
            if (!Helper.TieneAccesoPrescripciones(IdRol))
            {
                TempData["Error"] =
                    "No tiene permisos para acceder a este módulo.";

                return RedirectToAction("Index", "Home");
            }

            CargarListas();

            return View();
        }

        // Registra una nueva prescripción
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TbPrescripcion prescripcion)
        {
            // Verifica que exista una sesión activa
            if (!Helper.SesionActiva(HttpContext.Session.GetInt32("IdUsuario")))
            {
                TempData["Error"] =
                    "La sesión ha expirado por inactividad. Inicie sesión nuevamente.";

                return RedirectToAction("Index", "LogIn");
            }

            // Verifica que el usuario tenga acceso al módulo
            if (!Helper.TieneAccesoPrescripciones(IdRol))
            {
                TempData["Error"] =
                    "No tiene permisos para acceder a este módulo.";

                return RedirectToAction("Index", "Home");
            }

            try
            {
                // Estas propiedades son cargadas por Entity Framework
                ModelState.Remove(nameof(TbPrescripcion.IdPacienteNavigation));
                ModelState.Remove(nameof(TbPrescripcion.IdUsuarioNavigation));

                if (!ModelState.IsValid)
                {
                    CargarListas();
                    return View(prescripcion);
                }

                _prescripcionService.Create(prescripcion);

                TempData["Success"] =
                    "La prescripción fue registrada correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

                CargarListas();

                return View(prescripcion);
            }
        }

        // Muestra el formulario para editar una prescripción
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Verifica que exista una sesión activa
            if (!Helper.SesionActiva(HttpContext.Session.GetInt32("IdUsuario")))
            {
                TempData["Error"] =
                    "La sesión ha expirado por inactividad. Inicie sesión nuevamente.";

                return RedirectToAction("Index", "LogIn");
            }

            // Verifica que el usuario tenga acceso al módulo
            if (!Helper.TieneAccesoPrescripciones(IdRol))
            {
                TempData["Error"] =
                    "No tiene permisos para acceder a este módulo.";

                return RedirectToAction("Index", "Home");
            }

            TbPrescripcion? prescripcion =
                _prescripcionService.GetById(id);

            if (prescripcion == null)
                return NotFound();

            CargarListas();

            return View(prescripcion);
        }


        // Actualiza una prescripción
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TbPrescripcion prescripcion)
        {
            // Verifica que exista una sesión activa
            if (!Helper.SesionActiva(HttpContext.Session.GetInt32("IdUsuario")))
            {
                TempData["Error"] =
                    "La sesión ha expirado por inactividad. Inicie sesión nuevamente.";

                return RedirectToAction("Index", "LogIn");
            }

            // Verifica que el usuario tenga acceso al módulo
            if (!Helper.TieneAccesoPrescripciones(IdRol))
            {
                TempData["Error"] =
                    "No tiene permisos para acceder a este módulo.";

                return RedirectToAction("Index", "Home");
            }

            try
            {
                // Estas propiedades son cargadas por Entity Framework
                ModelState.Remove(nameof(TbPrescripcion.IdPacienteNavigation));
                ModelState.Remove(nameof(TbPrescripcion.IdUsuarioNavigation));

                if (!ModelState.IsValid)
                {
                    CargarListas(prescripcion.IdPaciente);
                    return View(prescripcion);
                }

                _prescripcionService.Update(prescripcion);

                TempData["Success"] =
                    "La prescripción fue actualizada correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

                CargarListas(prescripcion.IdPaciente);

                return View(prescripcion);
            }
        }

        // Muestra el detalle de una prescripción
        [HttpGet]
        public IActionResult Ver(int id)
        {
            // Verifica que exista una sesión activa
            if (!Helper.SesionActiva(HttpContext.Session.GetInt32("IdUsuario")))
            {
                TempData["Error"] =
                    "La sesión ha expirado por inactividad. Inicie sesión nuevamente.";

                return RedirectToAction("Index", "LogIn");
            }

            // Verifica que el usuario tenga acceso al módulo
            if (!Helper.TieneAccesoPrescripciones(IdRol))
            {
                TempData["Error"] =
                    "No tiene permisos para acceder a este módulo.";

                return RedirectToAction("Index", "Home");
            }

            TbPrescripcion? prescripcion =
                _prescripcionService.GetById(id);

            if (prescripcion == null)
                return NotFound();

            return View(prescripcion);
        }


        // Carga las listas utilizadas por las vistas
        private void CargarListas(int? idPaciente = null)
        {
            // Solo muestra pacientes con una evaluación
            // realizada durante los últimos 30 días
            DateTime fechaLimite = DateTime.Now.AddDays(-30);

            var pacientes = _pacienteService.GetAll();

            var pacientesFiltrados = pacientes
                .Where(p => p.TbEvaluacionFisicas.Any(e =>
                    e.FechaEvaluacion >= fechaLimite))
                .Select(p => new
                {
                    p.IdPaciente,
                    NombreCompleto =
                        $"{p.IdPersonaNavigation.Nombre} {p.IdPersonaNavigation.Apellido}"
                })
                .OrderBy(p => p.NombreCompleto)
                .ToList();

            ViewBag.Pacientes = new SelectList(
                pacientesFiltrados,
                "IdPaciente",
                "NombreCompleto",
                idPaciente);
        }
    }
    }