/** LogInService
 * Servicio encargado de la lógica de autenticación de los usuarios del
 * sistema, incluyendo la validación de credenciales y el control de
 * acceso a la aplicación.
 * -----------------------------------------------------------------------
 */

using CIEMPOS.Helpers;
using CIEMPOS.Models;
using CIEMPOS.Repos;

namespace CIEMPOS.Services
{
    public class LogInService
    {
        private readonly IUsuarioRepo _usuarioRepo;

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

        // Indica si el usuario debe cambiar su contraseña
        public bool MustChangePassword(TbUsuario usuario)
        {
            return usuario.DebeCambiarContrasena;
        }

        // Cambia la contraseña del usuario
        public bool ChangePassword(
            int idUsuario,
            string nuevaContrasena,
            string confirmarContrasena)
        {
            // Verifica que ambas contraseñas coincidan
            if (nuevaContrasena != confirmarContrasena)
                throw new Exception("Las contraseñas no coinciden.");

            // Valida la complejidad de la contraseña
            Helper.ValidarContrasena(nuevaContrasena);

            // Encripta la nueva contraseña
            string hash = Helper.EncriptarContrasena(nuevaContrasena);

            // Actualiza la contraseña del usuario
            bool actualizado = _usuarioRepo.UpdatePassword(idUsuario, hash);

            // Verifica que la actualización haya sido exitosa
            if (!actualizado)
                throw new Exception("No fue posible actualizar la contraseña.");

            // Indica que el cambio fue exitoso
            return true;
        }
    }
}