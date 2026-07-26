// Define el contrato que debe cumplir el repositorio de roles,
// especificando los métodos disponibles para realizar las operaciones
// de consulta, registro, actualización y cambio de estado.

using CIEMPOS.Models;

namespace CIEMPOS.Repos
{
    public interface IRolRepo
    {
        IEnumerable<TbRol> GetAll(bool mostrarInactivos = false);

        TbRol? GetById(int id);

        bool Create(TbRol rol);

        bool Update(TbRol rol);

        bool ChangeStatus(int id);
    }
}
