using System;

namespace PostService.Domain.Entities
{
    public class UserPost : BaseEntity
    {
        public Guid TopicId { get; private set; }
        public Guid UserId { get; private set; }
        public ContentType Type { get; private set; }
        public string Content { get; private set; }

        protected UserPost() { }

        public static UserPost Create(Guid topicId, Guid userId, ContentType type, string content)
        {
            if (topicId == Guid.Empty || userId == Guid.Empty)
                throw new ArgumentException("Konu veya Kullanıcı bilgisi eksik olamaz.");

            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("İçerik alanı boş bırakılamaz.");

            if (type == ContentType.Text && content.Length > 300)
                throw new ArgumentException("Metin yanıtları 300 karakteri geçemez.");

            return new UserPost
            {
                Id = Guid.NewGuid(),
                TopicId = topicId,
                UserId = userId,
                Type = type,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}