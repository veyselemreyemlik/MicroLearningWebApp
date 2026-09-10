using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PostService.Application.DTOs;
using PostService.Domain.Repositories;

namespace PostService.Application.Features.Posts.Queries.GetPostsByTopic
{
    public class GetPostsByTopicQueryHandler : IRequestHandler<GetPostsByTopicQuery, IEnumerable<UserPostDto>>
    {
        private readonly IPostRepository _postRepository;

        public GetPostsByTopicQueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IEnumerable<UserPostDto>> Handle(GetPostsByTopicQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetPostsByTopicIdAsync(request.TopicId);

            return posts.Select(p => new UserPostDto
            {
                Id = p.Id,
                TopicId = p.TopicId,
                UserId = p.UserId,
                Type = p.Type,
                Content = p.Content,
                CreatedAt = p.CreatedAt
            });
        }
    }
}