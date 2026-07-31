// Define el contrato que debe cumplir el repositorio de personas,
// especificando los métodos disponibles para realizar las operaciones
// de consulta, registro y actualización.

using CIEMPOS.Models;

namespace CIEMPOS.Repos
{
    public interface IPersonaRepo
    {
        IEnumerable<TbPersona> GetAll(bool mostrarInactivos = false);

        TbPersona? GetById(int id);

        bool Create(TbPersona persona);

        bool Update(TbPersona persona);

        // Verifica si ya existe una persona con la identificación indicada
        bool ExistsByIdentification(string identificacion);

        // Verifica si otra persona ya utiliza la identificación indicada
        bool ExistsByIdentification(string identificacion, int idPersona);

        // Obtiene las personas que aún no están registradas como pacientes
        IEnumerable<TbPersona> GetDisponiblesParaPaciente();
    }
}