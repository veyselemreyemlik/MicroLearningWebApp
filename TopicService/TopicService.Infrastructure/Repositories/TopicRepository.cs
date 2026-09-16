using Microsoft.EntityFrameworkCore;
using TopicService.Domain.Entities;
using TopicService.Domain.Repositories;
using TopicService.Infrastructure.Contexts;

namespace TopicService.Infrastructure.Repositories;

public class TopicRepository : ITopicRepository
{
    private readonly TopicDbContext _context;

    public TopicRepository(TopicDbContext context)
    {
        _context = context;
    }

    public async Task<DailyTopic?> GetTopicByDateAsync(DateTime targetDate)
    {
        return await _context.DailyTopics
            .FirstOrDefaultAsync(t => t.TargetDate.Date == targetDate.Date);
    }

    // YENİ EKLENEN METOD:
    public async Task<DailyTopic?> GetRandomTopicAsync()
    {
        return await _context.DailyTopics
            .OrderBy(r => Guid.NewGuid()) // SQL: ORDER BY NEWID()
            .FirstOrDefaultAsync();
    }

    public async Task AddAsync(DailyTopic topic)
    {
        await _context.DailyTopics.AddAsync(topic);
        await _context.SaveChangesAsync();
    }
}