/*
 * Contiene las operaciones de acceso a datos relacionadas con
 * los pagos registrados en el sistema.
 */

using CIEMPOS.Data;
using CIEMPOS.Models;
using Microsoft.EntityFrameworkCore;

namespace CIEMPOS.Repos
{
    public class PagoRepo
    {
        // Contexto de Entity Framework
        private readonly ApplicationDbContext _context;

        // Constructor con Dependency Injection
        public PagoRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtiene los pagos según los filtros indicados
        public IEnumerable<TbPago> GetAll(
            int? idPaciente = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null)
        {
            IQueryable<TbPago> query = _context.TbPagos
                .Include(p => p.IdPacienteNavigation)
                    .ThenInclude(p => p.IdPersonaNavigation)
                .Include(p => p.IdUsuarioNavigation)
                    .ThenInclude(u => u.IdPersonaNavigation);

            // Filtra por paciente
            if (idPaciente.HasValue)
                query = query.Where(p =>
                    p.IdPaciente == idPaciente.Value);

            // Filtra por fecha inicial
            if (fechaInicio.HasValue)
                query = query.Where(p =>
                    p.FechaPago >= fechaInicio.Value);

            // Filtra por fecha final
            if (fechaFin.HasValue)
                query = query.Where(p =>
                    p.FechaPago <= fechaFin.Value);

            // Ordena del más reciente al más antiguo
            return query.OrderByDescending(p => p.FechaPago)
                        .ToList();
        }

        // Obtiene un pago por su identificador
        public TbPago? GetById(int id)
        {
            return _context.TbPagos
                .Include(p => p.IdPacienteNavigation)
                    .ThenInclude(p => p.IdPersonaNavigation)
                .Include(p => p.IdUsuarioNavigation)
                    .ThenInclude(u => u.IdPersonaNavigation)
                .FirstOrDefault(p => p.IdPago == id);
        }

        // Registra un nuevo pago
        public bool Create(TbPago pago)
        {
            _context.TbPagos.Add(pago);

            return _context.SaveChanges() > 0;
        }

        // Actualiza un pago existente
        public bool Update(TbPago pago)
        {
            // Busca el pago en la base de datos
            TbPago? pagoActual =
                _context.TbPagos.Find(pago.IdPago);

            // Verifica que exista
            if (pagoActual == null)
                return false;

            // Solo permite modificar el número de autorización
            pagoActual.NumeroAutorizacion =
                pago.NumeroAutorizacion;

            // Guarda los cambios
            return _context.SaveChanges() > 0;
        }
    }
}