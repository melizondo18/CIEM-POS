// Define el contrato que debe cumplir el repositorio de usuarios,
// especificando los métodos disponibles para realizar las operaciones
// de consulta, registro y actualización.

using CIEMPOS.Models;

namespace CIEMPOS.Repos
{
    public interface IUsuarioRepo
    {
        // Obtiene la lista de usuarios registrados
        IEnumerable<TbUsuario> GetAll(bool mostrarInactivos = false);

        // Busca un usuario por su identificador
        TbUsuario? GetById(int id);

        // Registra un nuevo usuario
        bool Create(TbUsuario usuario);

        // Actualiza la información de un usuario
        bool Update(TbUsuario usuario);

        // Verifica si la persona ya tiene un usuario registrado
        bool ExistsByPersona(int idPersona);

        // Busca un usuario por su número de identificación
        TbUsuario? GetByIdentification(string identificacion);
    }
}