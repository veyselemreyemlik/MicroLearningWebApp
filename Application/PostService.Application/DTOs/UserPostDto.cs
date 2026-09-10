using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostService.Domain.Entities; // Doğru enum sınıfımızı ekledik

namespace PostService.Application.DTOs
{
    public class UserPostDto
    {
        public Guid Id { get; set; }
        public Guid TopicId { get; set; }
        public Guid UserId { get; set; }
        public ContentType Type { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}