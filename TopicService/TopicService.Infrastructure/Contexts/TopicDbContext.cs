using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopicService.Domain.Entities;

namespace TopicService.Infrastructure.Contexts
{
    public class TopicDbContext: DbContext
    {
        public TopicDbContext(DbContextOptions<TopicDbContext> options) : base(options)
        {
        }
        public DbSet<DailyTopic> DailyTopics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DailyTopic>(entity =>

            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(e => e.Description).IsRequired();

            });
            
        }
    }
}
