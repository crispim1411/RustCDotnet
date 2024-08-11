var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<RedisConnection>();
builder.Services.AddHostedService<MainService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.Run();
