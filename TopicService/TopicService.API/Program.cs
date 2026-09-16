using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TopicService.Application.Features.Topics.Commands.CreateTopic;
using TopicService.Application.Features.Topics.Queries.GetRandomTopic;
using TopicService.Domain.Repositories;
using TopicService.Infrastructure.Contexts;
using TopicService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. CORS Yapılandırması
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJs", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// 2. MediatR Kaydı
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetRandomTopicQuery).Assembly));

// 3. JWT Authentication (Hata yakalayıcı Event'ler eklendi)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };

        options.Events = new JwtBearerEvents
        {
            // Token parse edilemediğinde veya imza/tarih patladığında burası çalışır
            OnAuthenticationFailed = context =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[JWT AUTH HATASI]: " + context.Exception.Message + "\n");
                Console.ResetColor();
                return Task.CompletedTask;
            },
            // Token hiç gelmediğinde veya header geçersiz olduğunda burası çalışır
            OnChallenge = context =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n[JWT CHALLENGE]: Yetkisiz istek. Hata detayı: {context.ErrorDescription ?? "Token bulunamadı veya geçersiz."}\n");
                Console.ResetColor();
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// 4. Veritabanı (DbContext) Kaydı
builder.Services.AddDbContext<TopicDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 5. Repository'i sisteme tanıtıyoruz
builder.Services.AddScoped<ITopicRepository, TopicRepository>();

// =========================================================
// Uygulama ayağa kalkıyor
// =========================================================
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// --- MİDDLEWARE KISMI ---
app.UseCors("AllowNextJs");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();