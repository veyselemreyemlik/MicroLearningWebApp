using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PostService.Infrastructure;
using PostService.Application.Features.Posts.Commands.CreatePost;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 1. Swagger Ayarları (Build'den ÖNCEYE aldık ve birleştirdik)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PostService.API", Version = "v1" });

    // Swagger arayüzüne JWT Bearer yetkilendirme butonunu ekler
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Aşağıdaki kutucuğa IdentityService'ten aldığınız token'ı yapıştırın."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// 2. Altyapı (Infrastructure) servislerini dahil et
builder.Services.AddInfrastructureServices(builder.Configuration);

// 3. MediatR'ı Application katmanından kaydet
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreatePostCommand).Assembly));

// 4. JWT Doğrulama (Authentication) Ayarları
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
    });


// BÜTÜN SERVİS KAYITLARI BURADAN ÖNCE BİTMELİDİR!
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Kimlik doğrulama (Authentication) UseAuthorization'dan ÖNCE gelmelidir!
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();