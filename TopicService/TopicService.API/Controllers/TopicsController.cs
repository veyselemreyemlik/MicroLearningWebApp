using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization; // [Authorize] için gerekli kütüphane
using Microsoft.AspNetCore.Mvc;
using TopicService.Application.Features.Topics.Commands.CreateTopic;
using TopicService.Application.Features.Topics.Queries.GetTopicByDate;

namespace TopicService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // 1. EKLENEN: Sadece giriş yapmış (Token'ı olan) kullanıcılar okuyabilsin
    public class TopicsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TopicsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/topics
        // n8n'in gece 00:00'da tetikleyip konu göndereceği uç nokta
        [HttpPost]
        [AllowAnonymous] // 3. EKLENEN: n8n şimdilik JWT token olmadan da veri basabilsin
        public async Task<IActionResult> CreateTopic([FromBody] CreateTopicCommand command)
        {
            var topicId = await _mediator.Send(command);
            return Ok(new { Id = topicId, Message = "Günün konusu başarıyla oluşturuldu." });
        }

        // GET: api/topics/daily
        // Next.js (Önyüz) veya Gateway'den günün konusunu çekmek için kullanılacak uç nokta
        [HttpGet("daily")] // 2. GÜNCELLENEN: Frontend ile uyumlu olması için "today" yerine "daily" yapıldı
        public async Task<IActionResult> GetDailyTopic()
        {
            var query = new GetTopicByDateQuery { TargetDate = DateTime.UtcNow.Date };
            var topic = await _mediator.Send(query);

            if (topic == null)
            {
                // Frontend'in hata mesajını okuyabilmesi için JSON formatında dönüyoruz
                return NotFound(new { message = "Bugün için henüz bir konu üretilmemiş." });
            }

            // DİKKAT: Frontend'deki setDailyTopic(data.title) kısmının çalışması için
            // MediatR'dan dönen 'topic' modelinin içinde 'Title' isimli bir property olması gerekiyor!
            return Ok(topic);
        }
    }
}