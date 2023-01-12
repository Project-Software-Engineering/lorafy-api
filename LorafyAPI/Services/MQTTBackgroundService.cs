using LorafyAPI;
using LorafyAPI.Models;
using LorafyAPI.Services;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class MQTTBackgroundService : BackgroundService
{
    private readonly List<ConfiguredMqttService> _clients;
    private readonly IConfiguration _configuration;
    private readonly ILogger<MQTTBackgroundService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly AppDbContext _appContext;

    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var jsonService = new JsonModelsParsingService(context);
            var jsonString = System.Text.Encoding.Default.GetString(e.Message);
            jsonService.JsonToDatabase(jsonString);
        }
    }

    public MQTTBackgroundService(
        ILogger<MQTTBackgroundService> logger,
        IConfiguration configuration,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _configuration = configuration;
        _scopeFactory = scopeFactory;

        _clients = new List<ConfiguredMqttService> {
            new ConfiguredMqttService
            {
                Client = new MqttClient(
                    _configuration["MQTT:host"],
                    int.Parse(_configuration["MQTT:port"]),
                    false,
                    MqttSslProtocols.None,
                    null,
                    null
                ),
                Username = _configuration["MQTT:username"],
                Password = _configuration["MQTT:password"]
            },
            //new ConfiguredMqttService
            //{
            //    Client = new MqttClient(
            //        _configuration["MQTT2:host"],
            //        int.Parse(_configuration["MQTT2:port"]),
            //        false,
            //        MqttSslProtocols.None,
            //        null,
            //        null
            //    ),
            //    Username = _configuration["MQTT2:username"],
            //    Password = _configuration["MQTT2:password"]
            //}
        };
    }

    public override Task ExecuteTask => base.ExecuteTask;

    public override void Dispose()
    {
        base.Dispose();
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var configuredClient in _clients)
        {
            var clientId = Guid.NewGuid().ToString();
            configuredClient.Client.Connect(clientId, configuredClient.Username, configuredClient.Password);

            if (configuredClient.Client.IsConnected)
            {
                _logger.LogInformation($"Connected to TTN via MQTT - {configuredClient.Username}");

                configuredClient.Client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
                configuredClient.Client.Subscribe(new string[] { "#" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
            }
            else
            {
                _logger.LogError("Failed to connect");
            }
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