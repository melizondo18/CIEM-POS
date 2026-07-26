// Esta clase se encarga de realizar las operaciones de acceso a los datos
// de la tabla TB_Rol. Permite consultar, registrar, actualizar y cambiar
// el estado de los roles utilizando Entity Framework.

using CIEMPOS.Data;      
using CIEMPOS.Models;

namespace CIEMPOS.Repos
{
    // Acceso a datos de TB_Rol
    public class RolRepo : IRolRepo
    {
        // Contexto de Entity Framework
        private readonly ApplicationDbContext _context;

        // Constructor con Dependency Injection
        public RolRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtiene los roles según su estado
        public IEnumerable<TbRol> GetAll(bool mostrarInactivos = false)
        {
            if (mostrarInactivos)
                return _context.TbRols.ToList();

            return _context.TbRols
                           .Where(r => r.Estado)
                           .ToList();
        }

        // Busca un rol por Id
        public TbRol? GetById(int id)
        {
            return _context.TbRols
                           .FirstOrDefault(r => r.IdRol == id);
        }

        // Guarda un nuevo rol
        public bool Create(TbRol rol)
        {
            // Agrega el rol
            _context.TbRols.Add(rol);

            // Guarda los cambios y devuelve true si fue exitoso
            return _context.SaveChanges() > 0;
        }

        // Actualiza un rol existente
        public bool Update(TbRol rol)
        {
            // Busca el rol en la base de datos
            TbRol? rolActual = _context.TbRols.Find(rol.IdRol);

            // Verifica que el rol exista
            if (rolActual == null)
                return false;

            // Actualiza únicamente los campos permitidos
            rolActual.Nombre = rol.Nombre;
            rolActual.Descripcion = rol.Descripcion;
            rolActual.Estado = rol.Estado;

            // Guarda los cambios
            return _context.SaveChanges() > 0;
        }

        // Habilita o inhabilita un rol
        public bool ChangeStatus(int id)
        {
            // Busca el rol por su Id
            TbRol? rol = _context.TbRols
                     .FirstOrDefault(r => r.IdRol == id);

            // Si no existe, devuelve false
            if (rol == null)
                return false;

            // Cambia el estado actual
            rol.Estado = !rol.Estado;

            // Guarda los cambios y devuelve true si fue exitoso
            return _context.SaveChanges() > 0;
        }
    }
}
