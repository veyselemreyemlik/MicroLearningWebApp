using IdentityService.Application.Interfaces;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Application.Features.Auth.Command.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand , string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public RegisterUserCommandHandler(IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // 1. E-posta adresi kullanımda mı kontrol et
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
                throw new Exception("Bu e-posta adresi zaten kullanımda.");

            // 2. Şifreyi güvenli hale getir (Hash'le)
            var hashedPassword = _passwordHasher.Hash(request.Password);

            // 3. Domain Entity'sini oluştur
            var user = User.Create(request.Username, request.Email, hashedPassword);

            // 4. Veritabanına kaydet
            await _userRepository.AddAsync(user);

            // 5. Kayıt başarılıysa JWT Token üret ve dön
            return _jwtProvider.GenerateToken(user);
        }
    }
}
