/* Define el contrato que debe cumplir el repositorio de
 * pacientes, especificando las operaciones disponibles para consultar,
 * registrar y actualizar la información.
 */

using CIEMPOS.Models;

namespace CIEMPOS.Repos
{
    public interface IPacienteRepo
    {
        // Obtiene todos los pacientes
        IEnumerable<TbPaciente> GetAll(bool mostrarInactivos = false);

        // Obtiene un paciente por su Id
        TbPaciente? GetById(int id);

        // Registra un nuevo paciente
        bool Create(TbPaciente paciente);

        // Actualiza la información de un paciente
        bool Update(TbPaciente paciente);

        // Verifica si la persona ya está registrada como paciente
        bool ExistsByPersona(int idPersona);
    }
}