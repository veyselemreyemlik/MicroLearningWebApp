using IdentityService.Application.Features.Auth.Command.Register;
using IdentityService.Application.Features.Auth.Queries.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers
{

    [ApiController]
    [Route("api/[Controller]")]

    public class AuthController: ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //Post: api/Auth/register

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            // İstek MediatR aracılığıyla Handler'a gider, işlenir ve geriye JWT string'i döner
            var token = await _mediator.Send(command);

            return Ok(new
            {
                Token = token,
                Message = "Kullanıcı başarıyla kaydedildi."
            });
        }
        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserQuery query)
        {
            // İsteği MediatR'a gönderiyoruz, arka planda LoginUserQueryHandler çalışıyor
            var token = await _mediator.Send(query);
            return Ok(new { Token = token, Message = "Giriş başarılı." });
        }

    }
}
