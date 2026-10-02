using MediatR;
using PostService.Domain.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PostService.Application.Features.Posts.Queries.GetMyPosts;

public class GetMyPostsQueryHandler : IRequestHandler<GetMyPostsQuery, object>
{
    private readonly IPostRepository _repository;

    public GetMyPostsQueryHandler(IPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<object> Handle(GetMyPostsQuery request, CancellationToken cancellationToken)
    {
        var posts = await _repository.GetPostsByUserIdAsync(request.UserId);

        // Frontend'in okuyabileceği temiz bir formata (DTO) çeviriyoruz
        return posts.Select(p => new
        {
            Id = p.Id,
            TopicId = p.TopicId,
            Content = p.Content,
            CreatedAt = p.CreatedAt
        }).ToList();
    }
}