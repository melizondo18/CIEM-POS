/* Contiene la lógica de negocio relacionada con la
 * administración de las evaluaciones físicas del sistema.
 */

using CIEMPOS.Models;
using CIEMPOS.Repos;
using Microsoft.AspNetCore.Http;

namespace CIEMPOS.Services
{
    public class EvaluacionService
    {
        // Repositorios
        private readonly IEvaluacionRepo _evaluacionRepo;
        private readonly IPacienteRepo _pacienteRepo;
        private readonly IUsuarioRepo _usuarioRepo;

        // Acceso a la sesión del usuario
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Constructor con Dependency Injection
        public EvaluacionService(
            IEvaluacionRepo evaluacionRepo,
            IPacienteRepo pacienteRepo,
            IUsuarioRepo usuarioRepo,
            IHttpContextAccessor httpContextAccessor)
        {
            _evaluacionRepo = evaluacionRepo;
            _pacienteRepo = pacienteRepo;
            _usuarioRepo = usuarioRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        // Obtiene las evaluaciones según los filtros indicados
        public IEnumerable<TbEvaluacionFisica> GetAll(
            int? idPaciente = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null)
        {
            IEnumerable<TbEvaluacionFisica> evaluaciones =
                _evaluacionRepo.GetAll(idPaciente, fechaInicio, fechaFin);

            // Si no se indicó un rango de fechas,
            // muestra únicamente las últimas seis evaluaciones.
            if (!fechaInicio.HasValue && !fechaFin.HasValue)
                return evaluaciones.Take(6);

            return evaluaciones;
        }

        // Obtiene una evaluación por su Id
        public TbEvaluacionFisica? GetById(int id)
        {
            return _evaluacionRepo.GetById(id);
        }

        // Registra una nueva evaluación física
        public bool Create(TbEvaluacionFisica evaluacion)
        {
            // Verifica que el paciente exista
            TbPaciente? paciente = _pacienteRepo.GetById(evaluacion.IdPaciente);

            if (paciente == null)
                throw new Exception("El paciente seleccionado no existe.");

            // Obtiene el usuario autenticado desde la sesión
            int? idUsuario = _httpContextAccessor.HttpContext?
                .Session.GetInt32("IdUsuario");

            if (idUsuario == null)
                throw new Exception("No fue posible identificar el usuario que realiza la evaluación.");

            evaluacion.IdUsuario = idUsuario.Value;

            // Verifica que el usuario exista
            TbUsuario? usuario = _usuarioRepo.GetById(evaluacion.IdUsuario);

            if (usuario == null)
                throw new Exception("No fue posible identificar el usuario que realiza la evaluación.");

            // Valida el peso
            if (evaluacion.Peso <= 0)
                throw new Exception("El peso debe ser mayor que cero.");

            // Valida la estatura
            if (evaluacion.Estatura <= 0)
                throw new Exception("La estatura debe ser mayor que cero.");

            // Valida el porcentaje de grasa
            if (evaluacion.PorcentajeGrasa < 0)
                throw new Exception("El porcentaje de grasa no puede ser negativo.");

            // Valida la masa muscular
            if (evaluacion.MasaMuscular < 0)
                throw new Exception("La masa muscular no puede ser negativa.");

            // Asigna la fecha actual
            evaluacion.FechaEvaluacion = DateTime.Now;

            // Calcula el IMC
            evaluacion.Imc = Math.Round(
                evaluacion.Peso /
                (evaluacion.Estatura * evaluacion.Estatura),
                2);

            // Guarda la evaluación
            return _evaluacionRepo.Create(evaluacion);
        }

        // Actualiza una evaluación física existente
        public bool Update(TbEvaluacionFisica evaluacion)
        {
            // Verifica que la evaluación exista
            TbEvaluacionFisica? evaluacionActual =
                _evaluacionRepo.GetById(evaluacion.IdEvaluacion);

            if (evaluacionActual == null)
                throw new Exception("La evaluación no existe.");

            // Valida el peso
            if (evaluacion.Peso <= 0)
                throw new Exception("El peso debe ser mayor que cero.");

            // Valida la estatura
            if (evaluacion.Estatura <= 0)
                throw new Exception("La estatura debe ser mayor que cero.");

            // Valida el porcentaje de grasa
            if (evaluacion.PorcentajeGrasa < 0)
                throw new Exception("El porcentaje de grasa no puede ser negativo.");

            // Valida la masa muscular
            if (evaluacion.MasaMuscular < 0)
                throw new Exception("La masa muscular no puede ser negativa.");

            // Recalcula el IMC
            evaluacion.Imc = Math.Round(
                evaluacion.Peso /
                (evaluacion.Estatura * evaluacion.Estatura),
                2);

            // Actualiza la evaluación
            return _evaluacionRepo.Update(evaluacion);
        }
    }
}