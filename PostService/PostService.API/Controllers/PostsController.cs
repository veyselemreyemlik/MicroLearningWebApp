using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostService.Application.Features.Posts.Commands.CreatePost;
using PostService.Application.Features.Posts.Queries.GetMyPosts;
using PostService.Application.Features.Posts.Queries.GetPostsByTopic;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

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
            try
            {
                command.UserId = GetUserId();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }

            if (command.TopicId == Guid.Empty)
            {
                return BadRequest(new { Message = "HATA: Frontend'den TopicId (Konu ID) eksik gönderildi!" });
            }

            try
            {
                // İşlemi yapmayı dener
                var postId = await _mediator.Send(command);
                return Ok(new { PostId = postId, Message = "İçerik başarıyla eklendi." });
            }
            catch (Exception ex)
            {
                // EĞER PATLARSA: Hatanın tam içeriğini frontend'e JSON olarak gönderir!
                return StatusCode(500, new
                {
                    Message = "Sunucu Hatası: " + ex.Message,
                    InnerException = ex.InnerException?.Message,
                    StackTrace = ex.StackTrace
                });
            }
        }

        // GET: api/posts/topic/{topicId} (Konuya ait içerikleri herkes okuyabilir)
        [HttpGet("topic/{topicId}")]
        public async Task<IActionResult> GetPostsByTopic(Guid topicId)
        {
            var query = new GetPostsByTopicQuery { TopicId = topicId };
            var posts = await _mediator.Send(query);
            return Ok(posts);
        }

        // GET: api/posts/my-notes (Sadece giriş yapmış kullanıcılar notlarını görebilir)
        [Authorize]
        [HttpGet("my-notes")]
        public async Task<IActionResult> GetMyNotes()
        {
            try
            {
                var userId = GetUserId();
                var query = new GetMyPostsQuery { UserId = userId };
                var notes = await _mediator.Send(query);
                return Ok(notes);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        // UserId alma yardımcı metodu:
        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value
                           ?? User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("Geçerli bir kullanıcı kimliği bulunamadı.");
            }

            return userId;
        }
    }
}