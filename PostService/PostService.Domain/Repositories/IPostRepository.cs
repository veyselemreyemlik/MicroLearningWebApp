using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PostService.Domain.Entities;

namespace PostService.Domain.Repositories
{
    public interface IPostRepository
    {
        Task AddAsync(UserPost post);
        Task<IReadOnlyList<UserPost>> GetPostsByTopicIdAsync(Guid topicId);
        Task<IReadOnlyList<UserPost>> GetPostsByUserIdAsync(Guid userId);
    }
}