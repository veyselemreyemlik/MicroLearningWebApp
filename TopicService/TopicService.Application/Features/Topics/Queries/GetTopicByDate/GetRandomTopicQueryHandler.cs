using MediatR;
using TopicService.Application.DTOs;
using TopicService.Domain.Repositories;

namespace TopicService.Application.Features.Topics.Queries.GetRandomTopic;

public class GetRandomTopicQueryHandler : IRequestHandler<GetRandomTopicQuery, DailyTopicDto?>
{
    private readonly ITopicRepository _repository;

    public GetRandomTopicQueryHandler(ITopicRepository repository)
    {
        _repository = repository;
    }

    public async Task<DailyTopicDto?> Handle(GetRandomTopicQuery request, CancellationToken cancellationToken)
    {
        var topic = await _repository.GetRandomTopicAsync();
        if (topic == null) return null;

        return new DailyTopicDto
        {
            Id = topic.Id,
            Title = topic.Title,
            Description = topic.Description
        };
    }
}