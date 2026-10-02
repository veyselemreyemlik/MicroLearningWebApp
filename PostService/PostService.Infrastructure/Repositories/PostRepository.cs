using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Domain.Repositories;
using PostService.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly PostDbContext _context;

        public PostRepository(PostDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserPost post)
        {
            await _context.UserPosts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<UserPost>> GetPostsByTopicIdAsync(Guid topicId)
        {
            return await _context.UserPosts
                .AsNoTracking()
                .Where(p => p.TopicId == topicId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<UserPost>> GetPostsByUserIdAsync(Guid userId)
        {
            return await _context.UserPosts
                .AsNoTracking()
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}