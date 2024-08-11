
using StackExchange.Redis;

public class MainService : IHostedService, IDisposable
{
    private readonly ILogger<MainService> _log;
    private readonly RedisConnection _connection;
    private Timer? _timer = null;

    public MainService(ILogger<MainService> log, RedisConnection connection) {
        _log = log;
        _connection = connection;
        _connection.Start();
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _log.LogInformation("Starting...");
        _timer = new Timer(ReadFromDb, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _log.LogInformation("Stopping...");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    private void ReadFromDb(object? state) {
        var result = _connection.RetrieveData();
        Console.WriteLine($"Data: {result}");
    }
}