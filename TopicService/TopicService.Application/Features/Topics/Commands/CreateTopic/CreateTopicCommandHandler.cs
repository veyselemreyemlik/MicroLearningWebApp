using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopicService.Domain.Entities;
using TopicService.Domain.Repositories;

namespace TopicService.Application.Features.Topics.Commands.CreateTopic
{
    public class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand, Guid>
    {
        private readonly ITopicRepository _topicRepository;

        public CreateTopicCommandHandler(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public async Task<Guid> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
        {// 1. Domain kuralımızı kullanarak Entity'mizi oluşturuyoruz (Factory Method)
            var dailyTopic = DailyTopic.Create(request.Title, request.Description, request.TargetDate);

            // 2. Veritabanına (ileride yazacağımız altyapı ile) kaydediyoruz
            await _topicRepository.AddAsync(dailyTopic);

            // 3. Oluşan ID'yi geri dönüyoruz
            return dailyTopic.Id;
        }
    }
}
