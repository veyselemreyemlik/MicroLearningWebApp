using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopicService.Domain.Entities;

namespace TopicService.Domain
{
    public class DailyTopic: BaseEntity
    {
        public string Title {  get; private set; }
        public string Description { get; private set; }
        public DateTime TargetDate { get; private set; }

        // EF core için boş veri okurken constructor
        protected DailyTopic() { }

        private DailyTopic(string title, string description, DateTime targetDate) { 
        
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            TargetDate = targetDate;
            CreatedAt = DateTime.UtcNow;
        }

        public static DailyTopic Create(string title, string description, DateTime targetDate) 
        {
            if(string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Konu Başlığı Boş Olamaz.");

            if (targetDate.Date < DateTime.UtcNow.Date)
                throw new ArgumentException("Üretilen konunun hedef tarihi geçmiş bir zaman olamaz.");

            return new DailyTopic(title, description, targetDate);
        }
    }
}
