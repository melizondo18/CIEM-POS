// Esta clase se encarga de realizar las operaciones de acceso a los datos
// de la tabla TB_EvaluacionFisica. Permite consultar, registrar y actualizar
// la información de las evaluaciones utilizando Entity Framework.

using CIEMPOS.Data;
using CIEMPOS.Models;
using Microsoft.EntityFrameworkCore;

namespace CIEMPOS.Repos
{
    // Acceso a datos de TB_EvaluacionFisica
    public class EvaluacionRepo : IEvaluacionRepo
    {
        // Contexto de Entity Framework
        private readonly ApplicationDbContext _context;

        // Constructor con Dependency Injection
        public EvaluacionRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtiene las evaluaciones físicas según los filtros indicados
        public IEnumerable<TbEvaluacionFisica> GetAll(
            int? idPaciente = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null)
        {
            IQueryable<TbEvaluacionFisica> query = _context.TbEvaluacionFisicas
                                                           .Include(e => e.IdPacienteNavigation)
                                                               .ThenInclude(p => p.IdPersonaNavigation)
                                                           .Include(e => e.IdUsuarioNavigation)
                                                               .ThenInclude(u => u.IdPersonaNavigation);

            // Filtra por paciente
            if (idPaciente.HasValue)
                query = query.Where(e => e.IdPaciente == idPaciente.Value);

            // Filtra por fecha inicial
            if (fechaInicio.HasValue)
                query = query.Where(e => e.FechaEvaluacion >= fechaInicio.Value);

            // Filtra por fecha final
            if (fechaFin.HasValue)
                query = query.Where(e => e.FechaEvaluacion <= fechaFin.Value);

            // Ordena de la más reciente a la más antigua
            return query.OrderByDescending(e => e.FechaEvaluacion)
                        .ToList();
        }

        // Busca una evaluación por su identificador
        public TbEvaluacionFisica? GetById(int id)
        {
            return _context.TbEvaluacionFisicas
                           .Include(e => e.IdPacienteNavigation)
                               .ThenInclude(p => p.IdPersonaNavigation)
                           .Include(e => e.IdUsuarioNavigation)
                               .ThenInclude(u => u.IdPersonaNavigation)
                           .FirstOrDefault(e => e.IdEvaluacion == id);
        }

        // Verifica si un paciente tiene una evaluación
        // realizada dentro del rango de fechas indicado
        public bool ExisteEvaluacionReciente(
            int idPaciente,
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            return _context.TbEvaluacionFisicas.Any(e =>
                e.IdPaciente == idPaciente &&
                e.FechaEvaluacion >= fechaInicio &&
                e.FechaEvaluacion <= fechaFin);
        }

        // Registra una nueva evaluación
        public bool Create(TbEvaluacionFisica evaluacion)
        {
            _context.TbEvaluacionFisicas.Add(evaluacion);

            return _context.SaveChanges() > 0;
        }

        // Actualiza una evaluación existente
        public bool Update(TbEvaluacionFisica evaluacion)
        {
            // Busca la evaluación en la base de datos
            TbEvaluacionFisica? evaluacionActual =
                _context.TbEvaluacionFisicas.Find(evaluacion.IdEvaluacion);

            // Verifica que exista
            if (evaluacionActual == null)
                return false;

            // Actualiza únicamente los campos permitidos
            evaluacionActual.Peso = evaluacion.Peso;
            evaluacionActual.Estatura = evaluacion.Estatura;
            evaluacionActual.Imc = evaluacion.Imc;
            evaluacionActual.PorcentajeGrasa = evaluacion.PorcentajeGrasa;
            evaluacionActual.MasaMuscular = evaluacion.MasaMuscular;
            evaluacionActual.Observaciones = evaluacion.Observaciones;

            // Guarda los cambios
            return _context.SaveChanges() > 0;
        }
    }
}