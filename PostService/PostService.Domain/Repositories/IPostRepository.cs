using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PostService.Domain.Entities;

namespace PostService.Domain.Repositories
{
    public interface IPostRepository
    {
        Task AddAsync(UserPost post);
        Task<IEnumerable<UserPost>> GetPostsByTopicIdAsync(Guid topicId);
    }
}