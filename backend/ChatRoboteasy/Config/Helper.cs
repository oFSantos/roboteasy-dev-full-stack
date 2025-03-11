using System.Security.Cryptography;
using System.Text;

namespace ChatRoboteasy.Config
{
    public static class Helper
    {
        public static string HashSenha(string senha)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(senha)));
        }
    }
}
