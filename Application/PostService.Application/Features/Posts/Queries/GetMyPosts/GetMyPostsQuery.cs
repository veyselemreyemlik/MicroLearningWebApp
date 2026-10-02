using MediatR;
using System;
using System.Collections.Generic;

namespace PostService.Application.Features.Posts.Queries.GetMyPosts;

// DTO yapın varsa List<PostDto> kullanabilirsin, şimdilik dinamik (object) geçelim hızlıca test etmek için
public class GetMyPostsQuery : IRequest<object>
{
    public Guid UserId { get; set; }
}