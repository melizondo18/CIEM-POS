// Define el contrato que debe cumplir el repositorio de usuarios,
// especificando los métodos disponibles para realizar las operaciones
// de consulta, registro y actualización.

using CIEMPOS.Models;

namespace CIEMPOS.Repos
{
    public interface IUsuarioRepo
    {
        IEnumerable<TbUsuario> GetAll(bool mostrarInactivos = false);

        TbUsuario? GetById(int id);

        bool Create(TbUsuario usuario);

        bool Update(TbUsuario usuario);

        // Verifica si la persona ya tiene un usuario registrado
        bool ExistsByPersona(int idPersona);

        // Busca un usuario por su número de identificación
        TbUsuario? GetByIdentification(string identificacion);

        // Actualiza la contraseña del usuario y desactiva el cambio obligatorio
        bool UpdatePassword(int idUsuario, string nuevaContrasena);
    }
}