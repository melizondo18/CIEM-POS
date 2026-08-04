/* Contiene la lógica de negocio relacionada con la
 * administración de las prescripciones del sistema.
 */

using CIEMPOS.Models;
using CIEMPOS.Repos;
using Microsoft.AspNetCore.Http;

namespace CIEMPOS.Services
{
    public class PrescripcionService
    {
        // Repositorios
        private readonly IPrescripcionRepo _prescripcionRepo;
        private readonly IPacienteRepo _pacienteRepo;
        private readonly IUsuarioRepo _usuarioRepo;
        private readonly IEvaluacionRepo _evaluacionRepo;

        // Permite acceder a la sesión del usuario autenticado
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Constructor con Dependency Injection
        public PrescripcionService(
            IPrescripcionRepo prescripcionRepo,
            IPacienteRepo pacienteRepo,
            IUsuarioRepo usuarioRepo,
            IEvaluacionRepo evaluacionRepo,
            IHttpContextAccessor httpContextAccessor)
        {
            _prescripcionRepo = prescripcionRepo;
            _pacienteRepo = pacienteRepo;
            _usuarioRepo = usuarioRepo;
            _evaluacionRepo = evaluacionRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        // Obtiene las prescripciones según los filtros indicados
        public IEnumerable<TbPrescripcion> GetAll(
            int? idPaciente = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null)
        {
            IEnumerable<TbPrescripcion> prescripciones =
                _prescripcionRepo.GetAll(
                    idPaciente,
                    fechaInicio,
                    fechaFin);

            // Si no se indicó un rango de fechas,
            // muestra únicamente las últimas seis prescripciones.
            if (!fechaInicio.HasValue && !fechaFin.HasValue)
                return prescripciones.Take(6);

            return prescripciones;
        }

        // Obtiene una prescripción por su Id
        public TbPrescripcion? GetById(int id)
        {
            return _prescripcionRepo.GetById(id);
        }

        // Registra una nueva prescripción
        public bool Create(TbPrescripcion prescripcion)
        {
            // Verifica que el paciente exista
            TbPaciente? paciente =
                _pacienteRepo.GetById(prescripcion.IdPaciente);

            if (paciente == null)
                throw new Exception(
                    "El paciente seleccionado no existe.");

            // Verifica que el paciente tenga una evaluación
            // realizada durante los últimos 30 días
            DateTime fechaLimite = DateTime.Now.AddDays(-30);

            if (!_evaluacionRepo.ExisteEvaluacionReciente(
                    prescripcion.IdPaciente,
                    fechaLimite,
                    DateTime.Now))
            {
                throw new Exception(
                    "El paciente debe contar con una evaluación física realizada durante los últimos 30 días antes de registrar una prescripción.");
            }

            // Obtiene el usuario autenticado desde la sesión
            int? idUsuario =
                _httpContextAccessor.HttpContext?
                .Session.GetInt32("IdUsuario");

            if (idUsuario == null)
                throw new Exception(
                    "La sesión ha expirado. Inicie sesión nuevamente.");

            prescripcion.IdUsuario = idUsuario.Value;

            // Verifica que el usuario exista
            TbUsuario? usuario =
                _usuarioRepo.GetById(prescripcion.IdUsuario);

            if (usuario == null)
                throw new Exception(
                    "No fue posible identificar el usuario que registra la prescripción.");

            // Valida que exista al menos una rutina
            if (string.IsNullOrWhiteSpace(prescripcion.Cardio) &&
                string.IsNullOrWhiteSpace(prescripcion.Fuerza) &&
                string.IsNullOrWhiteSpace(prescripcion.Estiramiento))
            {
                throw new Exception(
                    "Debe registrar al menos una rutina de ejercicio.");
            }

            // Asigna la fecha actual
            prescripcion.FechaPrescripcion = DateTime.Now;

            // Toda prescripción nueva se registra como activa
            prescripcion.Estado = true;

            // Guarda la prescripción
            return _prescripcionRepo.Create(prescripcion);
        }

        // Actualiza una prescripción existente
        public bool Update(TbPrescripcion prescripcion)
        {
            // Verifica que la prescripción exista
            TbPrescripcion? prescripcionActual =
                _prescripcionRepo.GetById(
                    prescripcion.IdPrescripcion);

            if (prescripcionActual == null)
                throw new Exception(
                    "La prescripción no existe.");

            // Valida que exista al menos una rutina
            if (string.IsNullOrWhiteSpace(prescripcion.Cardio) &&
                string.IsNullOrWhiteSpace(prescripcion.Fuerza) &&
                string.IsNullOrWhiteSpace(prescripcion.Estiramiento))
            {
                throw new Exception(
                    "Debe registrar al menos una rutina de ejercicio.");
            }

            // Conserva la fecha y el usuario originales
            prescripcion.FechaPrescripcion =
                prescripcionActual.FechaPrescripcion;

            prescripcion.IdUsuario =
                prescripcionActual.IdUsuario;

            prescripcion.Estado =
                prescripcionActual.Estado;

            // Actualiza la prescripción
            return _prescripcionRepo.Update(prescripcion);
        }
    }
}