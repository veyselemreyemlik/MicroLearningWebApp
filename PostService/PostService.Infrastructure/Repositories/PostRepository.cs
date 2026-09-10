using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Domain.Repositories;
using PostService.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostService.Infrastructure.Repositories
{
    public class PostRepository: IPostRepository
    {
        private readonly PostDbContext _context;

        public PostRepository(PostDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserPost post) { 
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserPost>> GetPostsByTopicIdAsync(Guid topicId)
        {
            // İlgili konuya ait içerikleri tarihe göre azalan (en yeni en üstte) sırayla getiriyoruz
            return await _context.Posts
                .AsNoTracking()
                .Where(p => p.TopicId == topicId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}
