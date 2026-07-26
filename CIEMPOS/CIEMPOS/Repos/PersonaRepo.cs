// Esta clase se encarga de realizar las operaciones de acceso a los datos
// de la tabla TB_Persona. Permite consultar, registrar y actualizar
// la información de las personas utilizando Entity Framework.

using CIEMPOS.Data;
using CIEMPOS.Models;

namespace CIEMPOS.Repos
{
    // Acceso a datos de TB_Persona
    public class PersonaRepo : IPersonaRepo
    {
        // Contexto de Entity Framework
        private readonly ApplicationDbContext _context;

        // Constructor con Dependency Injection
        public PersonaRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtiene las personas según su estado
        public IEnumerable<TbPersona> GetAll(bool mostrarInactivos = false)
        {
            if (mostrarInactivos)
                return _context.TbPersonas.ToList();

            return _context.TbPersonas
                           .Where(p => p.Estado)
                           .ToList();
        }

        // Busca una persona por Id
        public TbPersona? GetById(int id)
        {
            return _context.TbPersonas
                           .FirstOrDefault(p => p.IdPersona == id);
        }

        // Guarda una nueva persona
        public bool Create(TbPersona persona)
        {
            // Agrega la persona
            _context.TbPersonas.Add(persona);

            // Guarda los cambios y devuelve true si fue exitoso
            return _context.SaveChanges() > 0;
        }

        // Actualiza una persona existente
        public bool Update(TbPersona persona)
        {
            // Busca la persona en la base de datos
            TbPersona? personaActual = _context.TbPersonas.Find(persona.IdPersona);

            // Verifica que la persona exista
            if (personaActual == null)
                return false;

            // Actualiza únicamente los campos permitidos
            personaActual.Nombre = persona.Nombre;
            personaActual.Apellido = persona.Apellido;
            personaActual.Identificacion = persona.Identificacion;
            personaActual.FechaNacimiento = persona.FechaNacimiento;
            personaActual.Sexo = persona.Sexo;
            personaActual.Email = persona.Email;
            personaActual.Telefono = persona.Telefono;
            personaActual.Direccion = persona.Direccion;
            personaActual.ContactoEmergencia = persona.ContactoEmergencia;
            personaActual.TelefonoEmergencia = persona.TelefonoEmergencia;
            personaActual.Estado = persona.Estado;

            // Guarda los cambios
            return _context.SaveChanges() > 0;
        }

        // Verifica si ya existe una persona con la identificación indicada
        public bool ExistsByIdentification(string identificacion)
        {
            return _context.TbPersonas
                           .Any(p => p.Identificacion == identificacion);
        }

        // Verifica si otra persona ya utiliza la identificación indicada
        public bool ExistsByIdentification(string identificacion, int idPersona)
        {
            return _context.TbPersonas
                           .Any(p => p.Identificacion == identificacion &&
                                     p.IdPersona != idPersona);
        }
    }
}