using StackExchange.Redis;

public sealed class RedisConnection {
    private readonly ILogger<RedisConnection> _log;
    private readonly string _connectionUrl;
    private IDatabase? _connection;
    public RedisConnection(ILogger<RedisConnection> log, IConfiguration config) 
    {
        _log = log;
        _connectionUrl = config.GetConnectionString("RedisConnection") 
            ?? throw new Exception("Redis connection string not found");
    }   

    public void Start() 
    {
        var redis = ConnectionMultiplexer.Connect(_connectionUrl);
        _connection = redis.GetDatabase();
    }

    public string RetrieveData() {
        if (_connection == null) {
            _log.LogInformation("Disconnected");
            return string.Empty;
        }
        return _connection.StringGet("timestamp").ToString();
    }
}