/*Define las operaciones de acceso a datos
 * relacionadas con las prescripciones del sistema.
 */
using CIEMPOS.Models;

namespace CIEMPOS.Repos
{
    public interface IPrescripcionRepo
    {
        // Obtiene todas las prescripciones
        IEnumerable<TbPrescripcion> GetAll(
            int? idPaciente = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null);

        // Obtiene una prescripción por su Id
        TbPrescripcion? GetById(int id);

        // Obtiene las prescripciones de un paciente
        IEnumerable<TbPrescripcion> GetByPaciente(int idPaciente);

        // Registra una nueva prescripción
        bool Create(TbPrescripcion prescripcion);

        // Actualiza una prescripción existente
        bool Update(TbPrescripcion prescripcion);
    }
}
