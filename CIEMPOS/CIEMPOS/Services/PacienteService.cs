/* Contiene la lógica de negocio relacionada con la administración
 * de pacientes del sistema.
 */

using CIEMPOS.Models;
using CIEMPOS.Repos;

namespace CIEMPOS.Services
{
    public class PacienteService
    {
        // Repositorios
        private readonly IPacienteRepo _pacienteRepo;
        private readonly IPersonaRepo _personaRepo;

        // Constructor con Dependency Injection
        public PacienteService(
            IPacienteRepo pacienteRepo,
            IPersonaRepo personaRepo)
        {
            _pacienteRepo = pacienteRepo;
            _personaRepo = personaRepo;
        }

        // Obtiene todos los pacientes
        public IEnumerable<TbPaciente> GetAll(bool mostrarInactivos = false)
        {
            return _pacienteRepo.GetAll(mostrarInactivos);
        }

        // Obtiene un paciente por su Id
        public TbPaciente? GetById(int id)
        {
            return _pacienteRepo.GetById(id);
        }

        // Crea un nuevo paciente
        public bool Create(TbPaciente paciente)
        {
            // Valida la información básica
            ValidarPaciente(paciente);

            // Obtiene y valida la persona
            TbPersona persona = ObtenerPersona(paciente.IdPersona);

            // Verifica que la persona no esté registrada como paciente
            if (_pacienteRepo.ExistsByPersona(paciente.IdPersona))
                throw new Exception("La persona seleccionada ya está registrada como paciente.");

            // Todo paciente nuevo se registra como activo
            paciente.Estado = true;

            // Guarda el paciente
            return _pacienteRepo.Create(paciente);
        }

        // Actualiza un paciente existente
        public bool Update(TbPaciente paciente)
        {
            // Valida que el objeto exista
            if (paciente == null)
                throw new Exception("La información del paciente es obligatoria.");

            // Valida que el Id sea válido
            if (paciente.IdPaciente <= 0)
                throw new Exception("El paciente seleccionado no es válido.");

            // Actualiza el paciente
            return _pacienteRepo.Update(paciente);
        }

        // Valida la información necesaria para crear un paciente
        private void ValidarPaciente(TbPaciente paciente)
        {
            // Valida que el objeto exista
            if (paciente == null)
                throw new Exception("La información del paciente es obligatoria.");

            // Valida que se haya seleccionado una persona
            if (paciente.IdPersona <= 0)
                throw new Exception("Debe seleccionar una persona.");
        }

        // Obtiene y valida la persona asociada al paciente
        private TbPersona ObtenerPersona(int idPersona)
        {
            // Busca la persona
            TbPersona? persona = _personaRepo.GetById(idPersona);

            // Verifica que exista
            if (persona == null)
                throw new Exception("La persona seleccionada no existe.");

            // Verifica que esté activa
            if (!persona.Estado)
                throw new Exception("La persona seleccionada se encuentra inactiva.");

            return persona;
        }
    }
}