/*
 * Nombre del archivo: Helper.cs
 * Descripción: Contiene métodos auxiliares reutilizables en diferentes módulos del sistema.
 */
using System.Text.RegularExpressions;


namespace CIEMPOS.Helpers
{
    public static class Helper
    {
        // Contraseña temporal utilizada para restablecer usuarios
        public const string PASSWORD_TEMPORAL = "Temporal123!";

        // Calcula la edad de una persona según su fecha de nacimiento
        public static int CalcularEdad(DateOnly fechaNacimiento)
        {
            DateOnly hoy = DateOnly.FromDateTime(DateTime.Today);

            int edad = hoy.Year - fechaNacimiento.Year;

            if (fechaNacimiento > hoy.AddYears(-edad))
                edad--;

            return edad;
        }

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
    }
}