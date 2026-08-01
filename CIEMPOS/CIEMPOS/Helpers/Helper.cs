/*
 Contiene métodos auxiliares reutilizables en diferentes módulos del sistema.
 */

using System.Text.RegularExpressions;

namespace CIEMPOS.Helpers
{
    public static class Helper
    {
        #region Constantes

        // Contraseña temporal utilizada para restablecer usuarios
        public const string PASSWORD_TEMPORAL = "Temporal123!";

        // Identificadores de los roles del sistema
        public const int ROL_ADMINISTRADOR = 1;
        public const int ROL_REGENTE = 2;
        public const int ROL_CONTABILIDAD = 3;
        public const int ROL_SERVICIO_CLIENTE = 4;

        #endregion

        #region Personas

        // Calcula la edad de una persona según su fecha de nacimiento
        public static int CalcularEdad(DateOnly fechaNacimiento)
        {
            DateOnly hoy = DateOnly.FromDateTime(DateTime.Today);

            int edad = hoy.Year - fechaNacimiento.Year;

            if (fechaNacimiento > hoy.AddYears(-edad))
                edad--;

            return edad;
        }

        #endregion

        #region Seguridad

        // Encripta una contraseña utilizando BCrypt
        public static string EncriptarContrasena(string contrasena)
        {
            return BCrypt.Net.BCrypt.HashPassword(contrasena);
        }

        // Verifica si una contraseña coincide con su versión encriptada
        public static bool VerificarContrasena(string contrasena, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(contrasena, hash);
        }

        // Valida la complejidad de una contraseña
        public static void ValidarContrasena(string contrasena)
        {
            if (string.IsNullOrWhiteSpace(contrasena))
                throw new Exception("La contraseña es obligatoria.");

            string patron = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";

            if (!Regex.IsMatch(contrasena, patron))
                throw new Exception("La contraseña debe tener al menos 8 caracteres, una letra mayúscula, una letra minúscula, un número y un carácter especial.");
        }

        #endregion

        #region Permisos

        // Roles
        public static bool TieneAccesoRoles(int? idRol)
        {
            return idRol == ROL_ADMINISTRADOR;
        }

        // Personas
        public static bool TieneAccesoPersonas(int? idRol)
        {
            return idRol == ROL_ADMINISTRADOR
                || idRol == ROL_REGENTE
                || idRol == ROL_SERVICIO_CLIENTE;
        }

        // Usuarios
        public static bool TieneAccesoUsuarios(int? idRol)
        {
            return idRol == ROL_ADMINISTRADOR;
        }

        // Pacientes
        public static bool TieneAccesoPacientes(int? idRol)
        {
            return idRol == ROL_ADMINISTRADOR
                || idRol == ROL_REGENTE
                || idRol == ROL_SERVICIO_CLIENTE;
        }

        // Evaluaciones Físicas
        public static bool TieneAccesoEvaluaciones(int? idRol)
        {
            return idRol == ROL_ADMINISTRADOR
                || idRol == ROL_REGENTE;
        }

        // Prescripciones
        public static bool TieneAccesoPrescripciones(int? idRol)
        {
            return idRol == ROL_ADMINISTRADOR
                || idRol == ROL_REGENTE;
        }

        // Pagos
        public static bool TieneAccesoPagos(int? idRol)
        {
            return idRol == ROL_ADMINISTRADOR
                || idRol == ROL_CONTABILIDAD
                || idRol == ROL_SERVICIO_CLIENTE;
        }

        // Reportes
        public static bool TieneAccesoReportes(int? idRol)
        {
            return idRol == ROL_ADMINISTRADOR
                || idRol == ROL_REGENTE
                || idRol == ROL_CONTABILIDAD;
        }

        // Bitácora
        public static bool TieneAccesoBitacora(int? idRol)
        {
            return idRol == ROL_ADMINISTRADOR;
        }

        #endregion
    }
}