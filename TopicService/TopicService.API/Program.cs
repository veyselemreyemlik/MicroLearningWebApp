using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- DİĞER SERVİSLER (MediatR, Controllers vb. buralarda olur) ---
builder.Services.AddControllers();
// builder.Services.AddMediatR(...);

// --- BURAYA YAPIŞTIRIYORSUN (app = builder.Build() satırından ÖNCE) ---
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
            // İŞTE DÜZELTİLEN SATIR BURASI (Key yerine SecretKey yazıyor):
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });

builder.Services.AddAuthorization();
// -------------------------------------------------------------------

// BÜYÜK SATIR: Builder işini bitirdi, uygulama ayağa kalkıyor
var app = builder.Build();

// --- MİDDLEWARE KISMI ---
// DİKKAT: Bu iki satırı da eklemeyi unutma ve sıralaması tam böyle olsun!
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();