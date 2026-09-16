using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopicService.Domain.Entities;

namespace TopicService.Domain.Repositories
{
    public interface ITopicRepository
    {
        Task<DailyTopic?> GetTopicByDateAsync(DateTime targetDate);
        Task<DailyTopic?> GetRandomTopicAsync(); // YENİ EKLENEN
        Task AddAsync(DailyTopic topic);
    }
}
