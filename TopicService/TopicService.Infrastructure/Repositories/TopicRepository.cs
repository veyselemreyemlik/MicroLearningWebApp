using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopicService.Domain.Entities;
using TopicService.Domain.Repositories;
using TopicService.Infrastructure.Contexts;

namespace TopicService.Infrastructure.Repositories
{
    public class TopicRepository : ITopicRepository
    {

        private readonly TopicDbContext _context;
        public TopicRepository(TopicDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DailyTopic topic)
        {
            await _context.DailyTopics.AddAsync(topic);
            await _context.SaveChangesAsync();
        }

        public async Task<DailyTopic> GetByIdAsync(Guid id)
        {
            return await _context.DailyTopics.FindAsync(id);
        }

        public async Task<DailyTopic> GetTopicByDateAsync(DateTime targetDate)
        {
            // Sadece gün/ay/yıl olarak eşleşen konuyu getiriyoruz
            return await _context.DailyTopics
                .FirstOrDefaultAsync(t => t.TargetDate.Date == targetDate.Date);
        }
    }
}
