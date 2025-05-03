using System.Security.Cryptography;
using RemoteControllerMaster.Database.Models;
using System.Text;

namespace RemoteControllerMaster.Helpers.Authorize
{
    public static class PasswordHelper
    {
        public static string HashPassword(User user, string password)
        {
            using var sha256 = SHA256.Create();
            var combined = $"{password}:{user.Login}";
            var bytes = Encoding.UTF8.GetBytes(combined);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(User user, string password)
        {
            return HashPassword(user, password) == user.PasswordHash;
        }
    }
}
