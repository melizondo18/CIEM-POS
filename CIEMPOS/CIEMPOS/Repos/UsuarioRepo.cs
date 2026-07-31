// Esta clase se encarga de realizar las operaciones de acceso a los datos
// de la tabla TB_Usuario. Permite consultar, registrar y actualizar
// la información de los usuarios utilizando Entity Framework.

using CIEMPOS.Data;
using CIEMPOS.Models;
using Microsoft.EntityFrameworkCore;

namespace CIEMPOS.Repos
{
    // Acceso a datos de TB_Usuario
    public class UsuarioRepo : IUsuarioRepo
    {
        // Contexto de Entity Framework
        private readonly ApplicationDbContext _context;

        // Constructor con Dependency Injection
        public UsuarioRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtiene la lista de usuarios según el filtro indicado
        public IEnumerable<TbUsuario> GetAll(bool mostrarInactivos = false)
        {
            IQueryable<TbUsuario> query = _context.TbUsuarios
                                                  .Include(u => u.IdPersonaNavigation)
                                                  .Include(u => u.IdRolNavigation);

            if (!mostrarInactivos)
                query = query.Where(u => u.Estado);

            return query.ToList();
        }

        // Busca un usuario por su identificador
        public TbUsuario? GetById(int id)
        {
            return _context.TbUsuarios
                           .Include(u => u.IdPersonaNavigation)
                           .Include(u => u.IdRolNavigation)
                           .FirstOrDefault(u => u.IdUsuario == id);
        }

        // Registra un nuevo usuario
        public bool Create(TbUsuario usuario)
        {
            // Agrega el usuario
            _context.TbUsuarios.Add(usuario);

            // Guarda los cambios y devuelve true si fue exitoso
            return _context.SaveChanges() > 0;
        }

        // Actualiza la información de un usuario
        public bool Update(TbUsuario usuario)
        {
            // Busca el usuario en la base de datos
            TbUsuario? usuarioActual = _context.TbUsuarios.Find(usuario.IdUsuario);

            // Verifica que el usuario exista
            if (usuarioActual == null)
                return false;

            // Actualiza únicamente los campos permitidos
            usuarioActual.IdRol = usuario.IdRol;
            usuarioActual.Estado = usuario.Estado;

            // Guarda los cambios
            return _context.SaveChanges() > 0;
        }

        // Verifica si la persona ya tiene un usuario registrado
        public bool ExistsByPersona(int idPersona)
        {
            return _context.TbUsuarios
                           .Any(u => u.IdPersona == idPersona);
        }

        // Busca un usuario por su número de identificación
        public TbUsuario? GetByIdentification(string identificacion)
        {
            return _context.TbUsuarios
                           .Include(u => u.IdPersonaNavigation)
                           .Include(u => u.IdRolNavigation)
                           .FirstOrDefault(u =>
                               u.IdPersonaNavigation.Identificacion == identificacion);
        }
    }
}