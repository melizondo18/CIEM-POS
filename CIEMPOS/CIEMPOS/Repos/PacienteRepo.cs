/* Esta clase se encarga de realizar las operaciones de acceso
 * a los datos de la tabla TB_Paciente utilizando Entity Framework.
 */

using CIEMPOS.Data;
using CIEMPOS.Models;
using Microsoft.EntityFrameworkCore;

namespace CIEMPOS.Repos
{
    // Acceso a datos de TB_Paciente
    public class PacienteRepo : IPacienteRepo
    {
        // Contexto de Entity Framework
        private readonly ApplicationDbContext _context;

        // Constructor con Dependency Injection
        public PacienteRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtiene la lista de pacientes según el filtro indicado
        public IEnumerable<TbPaciente> GetAll(bool mostrarInactivos = false)
        {
            IQueryable<TbPaciente> query = _context.TbPacientes
                                                   .Include(p => p.IdPersonaNavigation);

            if (!mostrarInactivos)
                query = query.Where(p => p.Estado);

            return query.ToList();
        }

        // Busca un paciente por su identificador
        public TbPaciente? GetById(int id)
        {
            return _context.TbPacientes
                           .Include(p => p.IdPersonaNavigation)
                           .FirstOrDefault(p => p.IdPaciente == id);
        }

        // Registra un nuevo paciente
        public bool Create(TbPaciente paciente)
        {
            // Agrega el paciente
            _context.TbPacientes.Add(paciente);

            // Guarda los cambios y devuelve true si fue exitoso
            return _context.SaveChanges() > 0;
        }

        // Actualiza la información de un paciente
        public bool Update(TbPaciente paciente)
        {
            // Busca el paciente en la base de datos
            TbPaciente? pacienteActual = _context.TbPacientes.Find(paciente.IdPaciente);

            // Verifica que exista
            if (pacienteActual == null)
                return false;

            // Actualiza únicamente los campos permitidos
            pacienteActual.InformacionClinica = paciente.InformacionClinica;
            pacienteActual.Estado = paciente.Estado;

            // Guarda los cambios
            return _context.SaveChanges() > 0;
        }

        // Verifica si la persona ya está registrada como paciente
        public bool ExistsByPersona(int idPersona)
        {
            return _context.TbPacientes
                           .Any(p => p.IdPersona == idPersona);
        }
    }
}