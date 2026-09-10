using MediatR;
using PostService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostService.Application.Features.Posts.Commands.CreatePost
{
    public class CreatePostCommand: IRequest<Guid>
    {
        public Guid TopicId {  get; set; }

        // Not: Bu UserId bilgisini dışarıdan (istemciden) JSON olarak almayacağız.
        // Güvenlik için, API katmanında kullanıcının JWT pasaportunu (Token) okuyup buraya otomatik basacağız.ktg
        public Guid UserId { get; set; }

        public ContentType Type { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
