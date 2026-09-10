using System;
using System.Collections.Generic;
using MediatR;
using PostService.Application.DTOs;

namespace PostService.Application.Features.Posts.Queries.GetPostsByTopic
{
    public class GetPostsByTopicQuery : IRequest<IEnumerable<UserPostDto>>
    {
        public Guid TopicId { get; set; }
    }
}