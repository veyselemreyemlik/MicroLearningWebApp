using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TopicService.Application.Features.Topics.Commands.CreateTopic;
using TopicService.Application.Features.Topics.Queries.GetTopicByDate;

namespace TopicService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class TopicsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TopicsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //POST
        // POST: api/topics
        // n8n'in gece 00:00'da tetikleyip konu göndereceği uç nokta
        [HttpPost]
        public async Task<IActionResult> CreateTopic([FromBody] CreateTopicCommand command)
        {
            var topicId = await _mediator.Send(command);
            return Ok(new { Id = topicId, Message = "Günün konusu başarıyla oluşturuldu." });
        }

        // GET: api/topics/today
        // Next.js (Önyüz) veya Gateway'den günün konusunu çekmek için kullanılacak uç nokta

        [HttpGet("today")]
        public async Task<IActionResult> GetTodayTopic()
        {
            var query = new GetTopicByDateQuery { TargetDate = DateTime.UtcNow.Date };
            var topic = await _mediator.Send(query);

            if (topic == null)
                return NotFound("Bugün için henüz bir konu üretilmemiş.");

            return Ok(topic);
        }
    }

}
