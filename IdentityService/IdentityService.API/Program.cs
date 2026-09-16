using IdentityService.Infrastructure;
using IdentityService.Application.Features.Auth.Command.Register;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// YENİ EKLENEN KISIM: CORS Politikasını tanımlıyoruz (Next.js'e kapıyı açıyoruz)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJs", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Next.js'in adresi
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// 1. Infrastructure (Altyapı) Bağımlılıklarını ekliyoruz (Veritabanı, BCrypt, JWT)
builder.Services.AddInfrastructureServices(builder.Configuration);

// 2. MediatR'ı Application katmanındaki bir sınıf üzerinden kaydediyoruz
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// YENİ EKLENEN KISIM: CORS Middleware'i (Sırası çok önemli, Authorization'dan önce olmalı!)
app.UseCors("AllowNextJs");

// Global Hata Yakalayıcı (Middleware)
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        // Sistemde bir hata olursa bağlantıyı koparmak yerine JSON dön
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 400; // Bad Request
        await context.Response.WriteAsJsonAsync(new { HataMesaji = ex.Message });
    }
});

app.UseAuthorization();
app.MapControllers();
app.Run();