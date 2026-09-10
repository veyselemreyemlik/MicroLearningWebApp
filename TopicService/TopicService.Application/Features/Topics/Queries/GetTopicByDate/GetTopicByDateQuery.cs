using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopicService.Application.DTOs;

namespace TopicService.Application.Features.Topics.Queries.GetTopicByDate
{
    public class GetTopicByDateQuery : IRequest<DailyTopicDto>
    {
        public DateTime TargetDate { get; set; }

    }
    }
