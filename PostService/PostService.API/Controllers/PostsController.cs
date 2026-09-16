using System;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostService.Application.Features.Posts.Commands.CreatePost;
using PostService.Application.Features.Posts.Queries.GetPostsByTopic;

namespace PostService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/posts (Sadece giriş yapmış kullanıcılar içerik üretebilir)
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostCommand command)
        {
            // JWT Token'ın içindeki 'Sub' (Kullanıcı ID) değerini çekiyoruz
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized("Geçersiz token bilgisi.");

            // Güvenlik: Command'in UserId'sini API katmanında biz belirliyoruz
            command.UserId = Guid.Parse(userIdString);

            // YENİ EKLENEN KONTROL: Frontend TopicId göndermezse 500 patlamasın, 400 fırlatsın!
            if (command.TopicId == Guid.Empty)
            {
                return BadRequest(new { Message = "HATA: Frontend'den TopicId (Konu ID) eksik gönderildi!" });
            }

            var postId = await _mediator.Send(command);
            return Ok(new { PostId = postId, Message = "İçerik başarıyla eklendi." });
        }

        // GET: api/posts/topic/{topicId} (Konuya ait içerikleri herkes okuyabilir)
        [HttpGet("topic/{topicId}")]
        public async Task<IActionResult> GetPostsByTopic(Guid topicId)
        {
            var query = new GetPostsByTopicQuery { TopicId = topicId };
            var posts = await _mediator.Send(query);
            return Ok(posts);
        }
    }
}