/*
 * Nombre del archivo: UsuarioService.cs
 * Descripción: Contiene la lógica de negocio relacionada con la administración
 * de usuarios del sistema.
 */

using CIEMPOS.Helpers;
using CIEMPOS.Models;
using CIEMPOS.Repos;

namespace CIEMPOS.Services
{
    public class UsuarioService
    {
        // Repositorios
        private readonly IUsuarioRepo _usuarioRepo;
        private readonly IPersonaRepo _personaRepo;
        private readonly IRolRepo _rolRepo;

        // Constructor con Dependency Injection
        public UsuarioService(
            IUsuarioRepo usuarioRepo,
            IPersonaRepo personaRepo,
            IRolRepo rolRepo)
        {
            _usuarioRepo = usuarioRepo;
            _personaRepo = personaRepo;
            _rolRepo = rolRepo;
        }

        // Obtiene todos los usuarios
        public IEnumerable<TbUsuario> GetAll(bool mostrarInactivos = false)
        {
            return _usuarioRepo.GetAll(mostrarInactivos);
        }

        // Obtiene un usuario por su Id
        public TbUsuario? GetById(int id)
        {
            return _usuarioRepo.GetById(id);
        }

        // Crea un nuevo usuario
        public bool Create(TbUsuario usuario)
        {
            // Valida la información básica
            ValidarUsuario(usuario);

            // Obtiene y valida la persona
            TbPersona persona = ObtenerPersona(usuario.IdPersona);

            // Obtiene y valida el rol
            TbRol rol = ObtenerRol(usuario.IdRol);

            // Verifica que la persona no tenga un usuario registrado
            if (_usuarioRepo.ExistsByPersona(usuario.IdPersona))
                throw new Exception("La persona seleccionada ya tiene un usuario registrado.");

            // Valida la contraseña
            Helper.ValidarContrasena(usuario.Contrasena);

            // Encripta la contraseña
            usuario.Contrasena = Helper.EncriptarContrasena(usuario.Contrasena);

            // Todo usuario nuevo debe cambiar su contraseña al iniciar sesión
            usuario.DebeCambiarContrasena = true;

            // Todo usuario nuevo se registra como activo
            usuario.Estado = true;

            // Guarda el usuario
            return _usuarioRepo.Create(usuario);
        }

        // Actualiza un usuario existente
        public bool Update(TbUsuario usuario)
        {
            // Valida que el objeto exista
            if (usuario == null)
                throw new Exception("La información del usuario es obligatoria.");

            // Valida que el Id sea válido
            if (usuario.IdUsuario <= 0)
                throw new Exception("El usuario seleccionado no es válido.");

            // Valida el rol
            ValidarRol(usuario.IdRol);

            // Obtiene y valida el rol
            TbRol rol = ObtenerRol(usuario.IdRol);

            // No permite desactivar cuentas con rol Administrador
            if (!usuario.Estado && usuario.IdRol == Helper.ROL_ADMINISTRADOR)
            {
                throw new Exception(
                    "Los usuarios con rol Administrador no pueden ser desactivados desde la aplicación. " +
                    "Si requiere desactivar una cuenta administrativa, contacte al desarrollador del sistema.");
            }

            // Actualiza el usuario
            return _usuarioRepo.Update(usuario);
        }

        // Restablece la contraseña de un usuario
        public bool ResetPassword(int idUsuario)
        {
            // Valida que el Id sea correcto
            if (idUsuario <= 0)
                throw new Exception("El usuario seleccionado no es válido.");

            // Obtiene el usuario
            TbUsuario? usuario = _usuarioRepo.GetById(idUsuario);

            // Verifica que exista
            if (usuario == null)
                throw new Exception("El usuario seleccionado no existe.");

            // Encripta la contraseña temporal
            string passwordEncriptada =
                Helper.EncriptarContrasena(Helper.PASSWORD_TEMPORAL);

            // Restablece la contraseña
            return _usuarioRepo.ResetPassword(
                idUsuario,
                passwordEncriptada);
        }

        // Valida la información necesaria para crear un usuario
        private void ValidarUsuario(TbUsuario usuario)
        {
            // Valida que el objeto exista
            if (usuario == null)
                throw new Exception("La información del usuario es obligatoria.");

            // Valida que se haya seleccionado una persona
            if (usuario.IdPersona <= 0)
                throw new Exception("Debe seleccionar una persona.");

            // Valida que se haya seleccionado un rol
            ValidarRol(usuario.IdRol);
        }

        // Valida que el Id del rol sea válido
        private void ValidarRol(int idRol)
        {
            if (idRol <= 0)
                throw new Exception("Debe seleccionar un rol.");
        }

        // Obtiene y valida la persona asociada al usuario
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

            // Verifica que sea mayor de edad
            if (Helper.CalcularEdad(persona.FechaNacimiento) < 18)
                throw new Exception("Solo las personas mayores de edad pueden tener un usuario del sistema.");

            return persona;
        }

        // Obtiene y valida el rol asociado al usuario
        private TbRol ObtenerRol(int idRol)
        {
            // Busca el rol
            TbRol? rol = _rolRepo.GetById(idRol);

            // Verifica que exista
            if (rol == null)
                throw new Exception("El rol seleccionado no existe.");

            // Verifica que esté activo
            if (!rol.Estado)
                throw new Exception("El rol seleccionado se encuentra inactivo.");

            return rol;
        }
    }
}