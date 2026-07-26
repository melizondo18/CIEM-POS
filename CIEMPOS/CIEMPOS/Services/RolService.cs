// Esta clase contiene la lógica de negocio relacionada con los roles.
// Se encarga de validar la información antes de interactuar con el
// repositorio y aplicar las reglas definidas para el sistema.

using CIEMPOS.Models;
using CIEMPOS.Repos;

namespace CIEMPOS.Services
{
    public class RolService
    {
        // Repositorio de roles
        private readonly IRolRepo _rolRepo;

        // Constructor con Dependency Injection
        public RolService(IRolRepo rolRepo)
        {
            _rolRepo = rolRepo;
        }

        // Obtiene los roles
        public IEnumerable<TbRol> GetAll(bool mostrarInactivos = false)
        {
            // Solicita al repositorio la lista de roles
            return _rolRepo.GetAll(mostrarInactivos);
        }

        // Obtiene un rol por Id
        public TbRol? GetById(int id)
        {
            // Solicita al repositorio el rol correspondiente al Id
            return _rolRepo.GetById(id);
        }

        // Crea un nuevo rol
        public bool Create(TbRol rol)
        {
            // Valida que el objeto exista
            if (rol == null)
                return false;

            // Valida que el nombre sea obligatorio
            if (string.IsNullOrWhiteSpace(rol.Nombre))
                return false;

            // Todo rol nuevo se registra como activo
            rol.Estado = true;

            // Guarda el rol
            return _rolRepo.Create(rol);
        }

        // Actualiza un rol
        public bool Update(TbRol rol)
        {
            // Valida que el objeto exista
            if (rol == null)
                return false;

            // Valida que el Id sea válido
            if (rol.IdRol <= 0)
                return false;

            // Valida que el nombre sea obligatorio
            if (string.IsNullOrWhiteSpace(rol.Nombre))
                return false;

            // Actualiza el rol
            return _rolRepo.Update(rol);
        }

        // Cambia el estado del rol
        public bool ChangeStatus(int id)
        {
            // Valida que el Id sea válido
            if (id <= 0)
                return false;

            // Cambia el estado del rol
            return _rolRepo.ChangeStatus(id);
        }
    }
}
