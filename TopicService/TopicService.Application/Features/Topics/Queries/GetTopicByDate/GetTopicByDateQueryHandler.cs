using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TopicService.Application.DTOs;
using TopicService.Domain.Repositories;

namespace TopicService.Application.Features.Topics.Queries.GetTopicByDate
{
    public class GetTopicByDateQueryHandler : IRequestHandler<GetTopicByDateQuery, DailyTopicDto>
    {
        private readonly ITopicRepository _topicRepository;

        public GetTopicByDateQueryHandler(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public async Task<DailyTopicDto> Handle(GetTopicByDateQuery request, CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetTopicByDateAsync(request.TargetDate);

            if (topic == null)
                return null;

            // Entity'yi DTO'ya manuel çeviriyoruz (İstenirse AutoMapper da kurulabilir)
            return new DailyTopicDto
            {
                Id = topic.Id,
                Title = topic.Title,
                Description = topic.Description,
                TargetDate = topic.TargetDate
            };
        }
    }
}