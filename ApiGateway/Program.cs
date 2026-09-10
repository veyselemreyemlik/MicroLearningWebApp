var builder = WebApplication.CreateBuilder(args);

//yarp servisi
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// YARP Middleware'ini çalıştırıyoruz
app.MapReverseProxy();

app.Run();