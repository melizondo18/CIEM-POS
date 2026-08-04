// Define el contrato que debe cumplir el repositorio de evaluaciones,
// especificando los métodos disponibles para realizar las operaciones
// de consulta, registro y actualización.

using CIEMPOS.Models;

namespace CIEMPOS.Repos
{
    public interface IEvaluacionRepo
    {
        // Obtiene las evaluaciones según los filtros indicados
        IEnumerable<TbEvaluacionFisica> GetAll(
            int? idPaciente = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null);

        // Obtiene una evaluación por su Id
        TbEvaluacionFisica? GetById(int id);

        // Verifica si un paciente tiene una evaluación
        // realizada dentro del rango de fechas indicado
        bool ExisteEvaluacionReciente(
            int idPaciente,
            DateTime fechaInicio,
            DateTime fechaFin);

        // Registra una nueva evaluación
        bool Create(TbEvaluacionFisica evaluacion);

        // Actualiza una evaluación existente
        bool Update(TbEvaluacionFisica evaluacion);
    }
}