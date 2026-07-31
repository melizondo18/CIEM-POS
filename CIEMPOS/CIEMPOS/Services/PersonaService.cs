// Esta clase contiene la lógica de negocio relacionada con las personas.
// Se encarga de validar la información antes de interactuar con el
// repositorio y aplicar las reglas definidas para el sistema.

using CIEMPOS.Models;
using CIEMPOS.Repos;

namespace CIEMPOS.Services
{
    public class PersonaService
    {
        // Repositorio de personas
        private readonly IPersonaRepo _personaRepo;

        // Constructor con Dependency Injection
        public PersonaService(IPersonaRepo personaRepo)
        {
            _personaRepo = personaRepo;
        }

        // Obtiene las personas
        public IEnumerable<TbPersona> GetAll(bool mostrarInactivos = false)
        {
            // Solicita al repositorio la lista de personas
            return _personaRepo.GetAll(mostrarInactivos);
        }

        // Obtiene las personas activas que aún no están registradas como pacientes
        public IEnumerable<TbPersona> GetDisponiblesParaPaciente()
        {
            return _personaRepo.GetDisponiblesParaPaciente();
        }

        // Obtiene una persona por Id
        public TbPersona? GetById(int id)
        {
            // Solicita al repositorio la persona correspondiente al Id
            return _personaRepo.GetById(id);
        }

        // Crea una nueva persona
        public bool Create(TbPersona persona)
        {
            // Valida que el objeto exista
            if (persona == null)
                throw new Exception("La información de la persona es obligatoria.");

            // Valida los campos obligatorios
            if (string.IsNullOrWhiteSpace(persona.Nombre))
                throw new Exception("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(persona.Apellido))
                throw new Exception("El apellido es obligatorio.");

            if (string.IsNullOrWhiteSpace(persona.Identificacion))
                throw new Exception("La identificación es obligatoria.");

            if (string.IsNullOrWhiteSpace(persona.Email))
                throw new Exception("El correo electrónico es obligatorio.");

            // Verifica que la identificación no esté registrada
            if (_personaRepo.ExistsByIdentification(persona.Identificacion))
                throw new Exception("Ya existe una persona registrada con esa identificación.");

            // Valida la fecha de nacimiento
            ValidarFechaNacimiento(persona.FechaNacimiento);

            // Toda persona nueva se registra como activa
            persona.Estado = true;

            // Guarda la persona
            return _personaRepo.Create(persona);
        }

        // Actualiza una persona
        public bool Update(TbPersona persona)
        {
            // Valida que el objeto exista
            if (persona == null)
                throw new Exception("La información de la persona es obligatoria.");

            // Valida que el Id sea válido
            if (persona.IdPersona <= 0)
                throw new Exception("La persona seleccionada no es válida.");

            // Valida los campos obligatorios
            if (string.IsNullOrWhiteSpace(persona.Nombre))
                throw new Exception("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(persona.Apellido))
                throw new Exception("El apellido es obligatorio.");

            if (string.IsNullOrWhiteSpace(persona.Identificacion))
                throw new Exception("La identificación es obligatoria.");

            if (string.IsNullOrWhiteSpace(persona.Email))
                throw new Exception("El correo electrónico es obligatorio.");

            // Verifica que otra persona no tenga la misma identificación
            if (_personaRepo.ExistsByIdentification(persona.Identificacion, persona.IdPersona))
                throw new Exception("Ya existe una persona registrada con esa identificación.");

            // Valida la fecha de nacimiento
            ValidarFechaNacimiento(persona.FechaNacimiento);

            // Actualiza la persona
            return _personaRepo.Update(persona);
        }

        // Valida la fecha de nacimiento de la persona
        private void ValidarFechaNacimiento(DateOnly fechaNacimiento)
        {
            DateOnly hoy = DateOnly.FromDateTime(DateTime.Today);

            if (fechaNacimiento > hoy)
                throw new Exception("La fecha de nacimiento no puede ser futura.");

            int edad = hoy.Year - fechaNacimiento.Year;

            if (fechaNacimiento > hoy.AddYears(-edad))
                edad--;

            if (edad < 4)
                throw new Exception("La persona debe tener al menos 4 años cumplidos.");

            if (edad > 120)
                throw new Exception("La fecha de nacimiento no es válida.");
        }


    }
}