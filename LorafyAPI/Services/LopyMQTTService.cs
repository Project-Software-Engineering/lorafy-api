using LorafyAPI;
using LorafyAPI.Services;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class LopyMQTTService : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<LopyMQTTService> _logger;
    private readonly MqttClient _client2;
    private readonly IServiceScopeFactory _scopeFactory;


    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var jsonService = new JsonModelsParsingService(context);
            var _json = System.Text.Encoding.Default.GetString(e.Message);
            jsonService.JsonToDatabase(_json);
        }
    }

    public LopyMQTTService(ILogger<LopyMQTTService> logger, IConfiguration configuration, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _configuration = configuration;
        _scopeFactory = scopeFactory;

        var host2 = _configuration["MQTT2:host"];
       
        var port2 = int.Parse(_configuration["MQTT2:port"]);


  
        _client2 = new MqttClient(host2, port2, false, MqttSslProtocols.None, null, null);
    }

    public override Task ExecuteTask => base.ExecuteTask;

    public override void Dispose()
    {
        base.Dispose();
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        
        var clientId2 = Guid.NewGuid().ToString();
      
        var topic2 = _configuration["MQTT2:topic"];
        var username2 = _configuration["MQTT2:username"];
        var password2 = _configuration["MQTT2:password"];

   
        _client2.Connect(clientId2, username2, password2);
        
        if (_client2.IsConnected)
        {
            _logger.LogInformation("Connected to TTN via MQTT");

            _client2.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            _client2.Subscribe(new string[] { topic2 }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });

        }
        else
        {
            _logger.LogError("Failed to connect");

        }


    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}