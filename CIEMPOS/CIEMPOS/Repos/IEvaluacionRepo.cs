// Define el contrato que debe cumplir el repositorio de evaluaciones,
// especificando los métodos disponibles para realizar las operaciones
// de consulta, registro y actualización.

using CIEMPOS.Models;

namespace CIEMPOS.Repos
{
    public interface IEvaluacionRepo
    {
        IEnumerable<TbEvaluacionFisica> GetAll(
            int? idPaciente = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null);

        TbEvaluacionFisica? GetById(int id);

        bool Create(TbEvaluacionFisica evaluacion);

        bool Update(TbEvaluacionFisica evaluacion);
    }
}