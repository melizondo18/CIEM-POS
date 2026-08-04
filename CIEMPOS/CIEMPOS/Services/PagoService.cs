/* Contiene la lógica de negocio relacionada con la
 * administración de los pagos del sistema.
 */

using CIEMPOS.Models;
using CIEMPOS.Repos;
using Microsoft.AspNetCore.Http;

namespace CIEMPOS.Services
{
    public class PagoService
    {
        // Repositorios
        private readonly PagoRepo _pagoRepo;
        private readonly IPacienteRepo _pacienteRepo;
        private readonly IUsuarioRepo _usuarioRepo;

        // Permite acceder a la sesión del usuario autenticado
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Configuración de la mensualidad
        private static decimal _montoBase = 17699.12m;
        private static decimal _iva = 2300.88m;

        // Constructor con Dependency Injection
        public PagoService(
            PagoRepo pagoRepo,
            IPacienteRepo pacienteRepo,
            IUsuarioRepo usuarioRepo,
            IHttpContextAccessor httpContextAccessor)
        {
            _pagoRepo = pagoRepo;
            _pacienteRepo = pacienteRepo;
            _usuarioRepo = usuarioRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        // Obtiene los pagos según los filtros indicados
        public IEnumerable<TbPago> GetAll(
            int? idPaciente = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null)
        {
            IEnumerable<TbPago> pagos =
                _pagoRepo.GetAll(idPaciente, fechaInicio, fechaFin);

            // Si no se indicó un rango de fechas,
            // muestra únicamente los últimos seis pagos.
            if (!fechaInicio.HasValue && !fechaFin.HasValue)
                return pagos.Take(6);

            return pagos;
        }

        // Obtiene un pago por su Id
        public TbPago? GetById(int id)
        {
            return _pagoRepo.GetById(id);
        }

        // Obtiene el monto base de la mensualidad
        public decimal ObtenerMontoBase()
        {
            return _montoBase;
        }

        // Obtiene el monto del IVA
        public decimal ObtenerIva()
        {
            return _iva;
        }

        // Obtiene el total de la mensualidad
        public decimal ObtenerTotal()
        {
            return _montoBase + _iva;
        }

        // Actualiza la configuración de la mensualidad
        public void ActualizarMontos(
            decimal montoBase,
            decimal iva)
        {
            _montoBase = montoBase;
            _iva = iva;
        }

        // Registra un nuevo pago
        public bool Create(TbPago pago)
        {
            // Verifica que el paciente exista
            if (!pago.IdPaciente.HasValue)
                throw new Exception("Debe seleccionar un paciente.");

            TbPaciente? paciente =
                _pacienteRepo.GetById(pago.IdPaciente.Value);

            if (paciente == null)
                throw new Exception(
                    "El paciente seleccionado no existe.");

            // Obtiene el usuario autenticado
            int? idUsuario =
                _httpContextAccessor.HttpContext?
                .Session.GetInt32("IdUsuario");

            if (idUsuario == null)
                throw new Exception(
                    "La sesión ha expirado. Inicie sesión nuevamente.");

            pago.IdUsuario = idUsuario.Value;

            // Verifica que el usuario exista
            TbUsuario? usuario =
                _usuarioRepo.GetById(pago.IdUsuario);

            if (usuario == null)
                throw new Exception(
                    "No fue posible identificar el usuario que registra el pago.");

            // Valida el número de autorización
            if (string.IsNullOrWhiteSpace(pago.NumeroAutorizacion))
                throw new Exception(
                    "El número de autorización es obligatorio.");

            // Asigna automáticamente la información del pago
            pago.FechaPago = DateTime.Now;
            pago.Monto = ObtenerTotal();
            pago.Estado = true;

            // Guarda el pago
            return _pagoRepo.Create(pago);
        }

        // Actualiza un pago existente
        public bool Update(TbPago pago)
        {
            // Verifica que el pago exista
            TbPago? pagoActual =
                _pagoRepo.GetById(pago.IdPago);

            if (pagoActual == null)
                throw new Exception(
                    "El pago no existe.");

            // Valida el número de autorización
            if (string.IsNullOrWhiteSpace(pago.NumeroAutorizacion))
                throw new Exception(
                    "El número de autorización es obligatorio.");

            // Conserva la información original
            pago.IdPaciente = pagoActual.IdPaciente;
            pago.IdUsuario = pagoActual.IdUsuario;
            pago.FechaPago = pagoActual.FechaPago;
            pago.Monto = pagoActual.Monto;
            pago.Estado = pagoActual.Estado;

            // Actualiza el pago
            return _pagoRepo.Update(pago);
        }
    }
}