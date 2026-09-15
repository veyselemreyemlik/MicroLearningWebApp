var builder = WebApplication.CreateBuilder(args);

// 1. CORS Servisini ekliyoruz (Build'den önce!)
builder.Services.AddCors(options =>
{
    options.AddPolicy("NextJsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Sadece bizim Frontend'e izin ver
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 2. YARP Servisini ekliyoruz
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// BÜYÜK SATIR: Beton döküldü, uygulama inşa edildi!
var app = builder.Build();

// 3. CORS Politikasını devreye alıyoruz (Sıralama önemli, Map'ten önce olmalı)
app.UseCors("NextJsPolicy");

// 4. YARP Middleware'ini çalıştırıyoruz
app.MapReverseProxy();

app.Run();