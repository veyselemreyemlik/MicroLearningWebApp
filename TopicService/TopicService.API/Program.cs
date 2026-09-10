using TopicService.Infrastructure;
using TopicService.Application.Features.Topics.Commands.CreateTopic;

var builder = WebApplication.CreateBuilder(args);

// 1. Controller desteğini ekliyoruz
builder.Services.AddControllers();

// 2. Swagger/OpenAPI yapılandırması
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. Infrastructure katmanında yazdığımız kayıt metodunu çağırıyoruz
builder.Services.AddInfrastructureServices(builder.Configuration);

// 4. MediatR'ı Application katmanındaki bir sınıf üzerinden (Assembly) kaydediyoruz
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateTopicCommand).Assembly));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();