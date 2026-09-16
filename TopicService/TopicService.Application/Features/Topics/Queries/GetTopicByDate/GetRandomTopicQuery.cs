using MediatR;
using TopicService.Application.DTOs;

namespace TopicService.Application.Features.Topics.Queries.GetRandomTopic;

public class GetRandomTopicQuery : IRequest<DailyTopicDto?>
{
}