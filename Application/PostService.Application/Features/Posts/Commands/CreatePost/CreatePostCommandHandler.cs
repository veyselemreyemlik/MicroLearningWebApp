using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using PostService.Domain.Entities;
using PostService.Domain.Repositories;

namespace PostService.Application.Features.Posts.Commands.CreatePost
{
    public class CreatePostCommandHandler: IRequestHandler<CreatePostCommand, Guid>
    {
        private readonly IPostRepository _postRepository;
        public CreatePostCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            // 1. Domain kuralımızı kullanarak Entity'mizi oluşturuyoruz (300 karakter kuralı vb. burada çalışır)
            var post = UserPost.Create(request.TopicId, request.UserId, request.Type, request.Content);

            // 2. Veritabanına kaydetmesi için Repository'ye gönderiyoruz
            await _postRepository.AddAsync(post);

            // 3. Oluşan yeni içeriğin ID'sini geri dönüyoruz
            return post.Id;
        }

    }
}
