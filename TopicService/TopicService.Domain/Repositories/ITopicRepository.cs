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
        Task<DailyTopic> GetByIdAsync(Guid id);
        Task<DailyTopic> GetTopicByDateAsync(DateTime targetDate);
        Task AddAsync(DailyTopic topic);

    }
}
