/*
 * Nombre del archivo: LogInService.cs
 * Descripción: Servicio encargado de la lógica de autenticación de los usuarios
 * del sistema, incluyendo la validación de credenciales y el control de acceso.
 */

using CIEMPOS.Helpers;
using CIEMPOS.Models;
using CIEMPOS.Repos;

namespace CIEMPOS.Services
{
    public class LogInService
    {
        // Repositorio para el acceso a los datos de los usuarios
        private readonly IUsuarioRepo _usuarioRepo;

        // Constructor con Dependency Injection
        public LogInService(IUsuarioRepo usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        // Autentica un usuario utilizando su identificación y contraseña
        public TbUsuario Authenticate(string identificacion, string password)
        {
            // Busca el usuario por número de identificación
            TbUsuario? usuario = _usuarioRepo.GetByIdentification(identificacion);

            // Verifica que el usuario exista
            if (usuario == null)
                throw new Exception("No existe un usuario registrado con esa identificación.");

            // Verifica que el usuario se encuentre activo
            if (!usuario.Estado)
                throw new Exception("El usuario se encuentra inactivo.");

            // Verifica que la contraseña sea correcta
            if (!Helper.VerificarContrasena(password, usuario.Contrasena))
                throw new Exception("La contraseña es incorrecta.");

            // Devuelve el usuario autenticado
            return usuario;
        }

        // Verifica si el usuario debe cambiar su contraseña
        public bool MustChangePassword(TbUsuario usuario)
        {
            return usuario.DebeCambiarContrasena;
        }
    }
}