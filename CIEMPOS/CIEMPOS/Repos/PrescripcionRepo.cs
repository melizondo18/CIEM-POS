/*Implementa las operaciones de acceso a datos
 * relacionadas con las prescripciones del sistema.
 */

using CIEMPOS.Data;
using CIEMPOS.Models;
using Microsoft.EntityFrameworkCore;

namespace CIEMPOS.Repos
{
    public class PrescripcionRepo : IPrescripcionRepo
    {
        // Contexto de la base de datos
        private readonly ApplicationDbContext _context;

        // Constructor con Dependency Injection
        public PrescripcionRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtiene todas las prescripciones
        public IEnumerable<TbPrescripcion> GetAll(
            int? idPaciente = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null)
        {
            IQueryable<TbPrescripcion> query =
                _context.TbPrescripcions
                    .Include(p => p.IdPacienteNavigation)
                        .ThenInclude(p => p.IdPersonaNavigation)
                    .Include(p => p.IdUsuarioNavigation);

            if (idPaciente.HasValue)
                query = query.Where(p => p.IdPaciente == idPaciente.Value);

            if (fechaInicio.HasValue)
                query = query.Where(p =>
                    p.FechaPrescripcion.Date >= fechaInicio.Value.Date);

            if (fechaFin.HasValue)
                query = query.Where(p =>
                    p.FechaPrescripcion.Date <= fechaFin.Value.Date);

            return query
                .OrderByDescending(p => p.FechaPrescripcion)
                .ToList();
        }

        // Obtiene una prescripción por su Id
        public TbPrescripcion? GetById(int id)
        {
            return _context.TbPrescripcions
                .Include(p => p.IdPacienteNavigation)
                    .ThenInclude(p => p.IdPersonaNavigation)
                .Include(p => p.IdUsuarioNavigation)
                .FirstOrDefault(p => p.IdPrescripcion == id);
        }

        // Obtiene las prescripciones de un paciente
        public IEnumerable<TbPrescripcion> GetByPaciente(int idPaciente)
        {
            return _context.TbPrescripcions
                .Include(p => p.IdUsuarioNavigation)
                .Where(p => p.IdPaciente == idPaciente)
                .OrderByDescending(p => p.FechaPrescripcion)
                .ToList();
        }

        // Registra una nueva prescripción
        public bool Create(TbPrescripcion prescripcion)
        {
            _context.TbPrescripcions.Add(prescripcion);

            return _context.SaveChanges() > 0;
        }

        // Actualiza una prescripción existente
        public bool Update(TbPrescripcion prescripcion)
        {
            _context.TbPrescripcions.Update(prescripcion);

            return _context.SaveChanges() > 0;
        }
    }
}