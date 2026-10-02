using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostService.Domain.Entities;

namespace PostService.Infrastructure.Configurations
{
    public class UserPostConfiguration : IEntityTypeConfiguration<UserPost>
    {
        public void Configure(EntityTypeBuilder<UserPost> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.TopicId)
                   .IsRequired();

            builder.Property(p => p.UserId)
                   .IsRequired();

            builder.Property(p => p.Content)
                   .IsRequired()
                   .HasMaxLength(4000);

            builder.Property(p => p.CreatedAt)
                   .IsRequired();
        }
    }
}