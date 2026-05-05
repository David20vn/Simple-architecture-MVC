using System.Security.Cryptography;
using System.Text;

namespace Sistema_inventario_mvc.Helpers
{
    public static class PasswordHelper
    {
        private const string Salt = "SaltFijaParaDesarrollo_2024!";

        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                // Concatenamos contraseña y sal
                var combined = password + Salt;
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
                return Convert.ToBase64String(bytes);
            }
        }

        public static bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hash;
        }
    }
}