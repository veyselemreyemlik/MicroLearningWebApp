using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopicService.Application.Features.Topics.Commands.CreateTopic;
using TopicService.Application.Features.Topics.Queries.GetRandomTopic;

namespace TopicService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Sadece giriş yapmış (Token sahibi) kullanıcılar erişebilir
    public class TopicsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TopicsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/topics
        // Gece otomasyonunun/workflow'un konu havuzuna veri basacağı uç nokta
        [HttpPost]
        [AllowAnonymous] // Servisler arası tetiklemeler için token zorunluluğu olmadan açık
        public async Task<IActionResult> CreateTopic([FromBody] CreateTopicCommand command)
        {
            var topicId = await _mediator.Send(command);
            return Ok(new { Id = topicId, Message = "Konu havuzuna yeni konu başarıyla eklendi." });
        }

        // GET: api/topics/daily
        // Frontend'in rastgele konu çekmek için çağıracağı uç nokta
        [HttpGet("daily")]
        public async Task<IActionResult> GetDailyTopic()
        {
            var query = new GetRandomTopicQuery();
            var topic = await _mediator.Send(query);

            if (topic == null)
            {
                return NotFound(new { message = "Konu havuzunda kayıtlı konu bulunamadı." });
            }

            return Ok(topic);
        }
    }
}