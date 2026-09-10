
using BCrypt.Net;
using IdentityService.Application.Interfaces;

namespace IdentityService.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        // Şifreyi şifrelenmiş metne dönüştür
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Gelen şifre ile veritabanındaki hash eşleşiyor mu kontrol et
        public bool Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}