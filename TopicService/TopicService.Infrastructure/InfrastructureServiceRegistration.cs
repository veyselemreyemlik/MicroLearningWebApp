using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TopicService.Domain.Repositories;
using TopicService.Infrastructure.Contexts;
using TopicService.Infrastructure.Repositories;

namespace TopicService.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext Kaydı
            services.AddDbContext<TopicDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("TopicDbConnectionString")));

            // Repository Kaydı
            services.AddScoped<ITopicRepository, TopicRepository>();

            return services;
        }
    }
}
