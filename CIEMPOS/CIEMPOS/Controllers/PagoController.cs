/*
 * Nombre del archivo: PagoController.cs
 * Descripción: Controlador encargado de administrar las operaciones
 * relacionadas con los pagos del sistema.
 */

using CIEMPOS.Helpers;
using CIEMPOS.Models;
using CIEMPOS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CIEMPOS.Controllers
{
    public class PagoController : Controller
    {
        // Servicios
        private readonly PagoService _pagoService;
        private readonly PacienteService _pacienteService;

        // Constructor con Dependency Injection
        public PagoController(
            PagoService pagoService,
            PacienteService pacienteService)
        {
            _pagoService = pagoService;
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

        // Muestra el listado de pagos
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
            if (!Helper.TieneAccesoPagos(IdRol))
            {
                TempData["Error"] =
                    "No tiene permisos para acceder a este módulo.";

                return RedirectToAction("Index", "Home");
            }

            IEnumerable<TbPago> pagos =
                _pagoService.GetAll(
                    idPaciente,
                    fechaInicio,
                    fechaFin);

            CargarListas(idPaciente);

            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = fechaFin;

            // Información de la mensualidad
            ViewBag.MontoBase = _pagoService.ObtenerMontoBase();
            ViewBag.Iva = _pagoService.ObtenerIva();
            ViewBag.Total = _pagoService.ObtenerTotal();

            // Rol del usuario autenticado
            ViewBag.IdRol = IdRol;

            return View(pagos);
        }

        // Muestra el formulario para registrar un pago
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
            if (!Helper.TieneAccesoPagos(IdRol))
            {
                TempData["Error"] =
                    "No tiene permisos para acceder a este módulo.";

                return RedirectToAction("Index", "Home");
            }

            CargarListas();

            ViewBag.MontoBase = _pagoService.ObtenerMontoBase();
            ViewBag.Iva = _pagoService.ObtenerIva();
            ViewBag.Total = _pagoService.ObtenerTotal();

            return View("~/Views/Pago/Create.cshtml");
        }

        // Registra un nuevo pago
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TbPago pago)
        {
            // Verifica que exista una sesión activa
            if (!Helper.SesionActiva(HttpContext.Session.GetInt32("IdUsuario")))
            {
                TempData["Error"] =
                    "La sesión ha expirado por inactividad. Inicie sesión nuevamente.";

                return RedirectToAction("Index", "LogIn");
            }

            // Verifica que el usuario tenga acceso al módulo
            if (!Helper.TieneAccesoPagos(IdRol))
            {
                TempData["Error"] =
                    "No tiene permisos para acceder a este módulo.";

                return RedirectToAction("Index", "Home");
            }

            try
            {
                // Estas propiedades son cargadas por Entity Framework
                ModelState.Remove(nameof(TbPago.IdPacienteNavigation));
                ModelState.Remove(nameof(TbPago.IdUsuarioNavigation));

                if (!ModelState.IsValid)
                {
                    CargarListas();

                    ViewBag.MontoBase = _pagoService.ObtenerMontoBase();
                    ViewBag.Iva = _pagoService.ObtenerIva();
                    ViewBag.Total = _pagoService.ObtenerTotal();

                    return View(pago);
                }

                _pagoService.Create(pago);

                TempData["Success"] =
                    "El pago fue registrado correctamente. El registro podrá consultarse posteriormente desde el módulo de Reportes.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

                CargarListas();

                ViewBag.MontoBase = _pagoService.ObtenerMontoBase();
                ViewBag.Iva = _pagoService.ObtenerIva();
                ViewBag.Total = _pagoService.ObtenerTotal();

                return View(pago);
            }
        }

        // Muestra el formulario para editar un pago
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
            if (!Helper.TieneAccesoPagos(IdRol))
            {
                TempData["Error"] =
                    "No tiene permisos para acceder a este módulo.";

                return RedirectToAction("Index", "Home");
            }

            TbPago? pago =
                _pagoService.GetById(id);

            if (pago == null)
                return NotFound();

            ViewBag.MontoBase = _pagoService.ObtenerMontoBase();
            ViewBag.Iva = _pagoService.ObtenerIva();
            ViewBag.Total = _pagoService.ObtenerTotal();

            return View(pago);
        }

        // Actualiza un pago
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TbPago pago)
        {
            // Verifica que exista una sesión activa
            if (!Helper.SesionActiva(HttpContext.Session.GetInt32("IdUsuario")))
            {
                TempData["Error"] =
                    "La sesión ha expirado por inactividad. Inicie sesión nuevamente.";

                return RedirectToAction("Index", "LogIn");
            }

            // Verifica que el usuario tenga acceso al módulo
            if (!Helper.TieneAccesoPagos(IdRol))
            {
                TempData["Error"] =
                    "No tiene permisos para acceder a este módulo.";

                return RedirectToAction("Index", "Home");
            }

            try
            {
                ModelState.Remove(nameof(TbPago.IdPacienteNavigation));
                ModelState.Remove(nameof(TbPago.IdUsuarioNavigation));

                if (!ModelState.IsValid)
                {
                    ViewBag.MontoBase = _pagoService.ObtenerMontoBase();
                    ViewBag.Iva = _pagoService.ObtenerIva();
                    ViewBag.Total = _pagoService.ObtenerTotal();

                    return View(pago);
                }

                _pagoService.Update(pago);

                TempData["Success"] =
                    "El número de autorización fue actualizado correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

                ViewBag.MontoBase = _pagoService.ObtenerMontoBase();
                ViewBag.Iva = _pagoService.ObtenerIva();
                ViewBag.Total = _pagoService.ObtenerTotal();

                return View(pago);
            }
        }

        // Muestra el detalle de un pago
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
            if (!Helper.TieneAccesoPagos(IdRol))
            {
                TempData["Error"] =
                    "No tiene permisos para acceder a este módulo.";

                return RedirectToAction("Index", "Home");
            }

            TbPago? pago =
                _pagoService.GetById(id);

            if (pago == null)
                return NotFound();

            ViewBag.MontoBase = _pagoService.ObtenerMontoBase();
            ViewBag.Iva = _pagoService.ObtenerIva();
            ViewBag.Total = _pagoService.ObtenerTotal();

            return View(pago);
        }

        // Carga las listas utilizadas por las vistas
        private void CargarListas(int? idPaciente = null)
        {
            ViewBag.Pacientes = new SelectList(
                _pacienteService.GetAll()
                    .Select(p => new
                    {
                        p.IdPaciente,
                        NombreCompleto =
                            $"{p.IdPersonaNavigation.Nombre} {p.IdPersonaNavigation.Apellido}"
                    })
                    .OrderBy(p => p.NombreCompleto),
                "IdPaciente",
                "NombreCompleto",
                idPaciente);
        }

        // Muestra el formulario para editar el monto de la mensualidad
        [HttpGet]
        public IActionResult EditarMonto()
        {
            // Verifica que exista una sesión activa
            if (!Helper.SesionActiva(HttpContext.Session.GetInt32("IdUsuario")))
            {
                TempData["Error"] =
                    "La sesión ha expirado por inactividad. Inicie sesión nuevamente.";

                return RedirectToAction("Index", "LogIn");
            }

            // Solo el administrador puede modificar el monto
            if (IdRol != Helper.ROL_ADMINISTRADOR)
            {
                TempData["Error"] =
                    "No tiene permisos para realizar esta acción.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.MontoBase = _pagoService.ObtenerMontoBase();
            ViewBag.Iva = _pagoService.ObtenerIva();
            ViewBag.Total = _pagoService.ObtenerTotal();

            return View();
        }

        // Actualiza el monto de la mensualidad
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarMonto(
            decimal montoBase,
            decimal iva)
        {
            // Verifica que exista una sesión activa
            if (!Helper.SesionActiva(HttpContext.Session.GetInt32("IdUsuario")))
            {
                TempData["Error"] =
                    "La sesión ha expirado por inactividad. Inicie sesión nuevamente.";

                return RedirectToAction("Index", "LogIn");
            }

            // Solo el administrador puede modificar el monto
            if (IdRol != Helper.ROL_ADMINISTRADOR)
            {
                TempData["Error"] =
                    "No tiene permisos para realizar esta acción.";

                return RedirectToAction(nameof(Index));
            }

            try
            {
                _pagoService.ActualizarMontos(
                    montoBase,
                    iva);

                TempData["Success"] =
                    "El monto de la mensualidad fue actualizado correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

                ViewBag.MontoBase = montoBase;
                ViewBag.Iva = iva;
                ViewBag.Total = montoBase + iva;

                return View();
            }
        }
    }
}