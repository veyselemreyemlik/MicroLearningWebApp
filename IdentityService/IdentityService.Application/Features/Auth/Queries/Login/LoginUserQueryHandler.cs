using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using IdentityService.Domain.Repositories;
using IdentityService.Application.Interfaces;

namespace IdentityService.Application.Features.Auth.Queries.Login
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public LoginUserQueryHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<string> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            // 1. Kullanıcıyı e-posta adresinden bul
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                throw new UnauthorizedAccessException("Hatalı e-posta veya şifre.");

            // 2. Girilen şifre ile veritabanındaki hash'i karşılaştır
            var isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
                throw new UnauthorizedAccessException("Hatalı e-posta veya şifre.");

            // 3. Bilgiler doğruysa Token üret ve dön
            return _jwtProvider.GenerateToken(user);
        }
    }
}