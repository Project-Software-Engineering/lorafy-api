using LorafyAPI.Services;
using System.Text.Json;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class MQTTBackgroundService : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MQTTBackgroundService> _logger;
    private readonly MqttClient _client;
    private string _json { get; set; }
    JsonModelsParsingService _jsonModelsParsingService;
    

    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        _json = System.Text.Encoding.Default.GetString(e.Message);
        Console.WriteLine(_json);
    }

    public MQTTBackgroundService(ILogger<MQTTBackgroundService> logger, IConfiguration configuration, JsonModelsParsingService jsonModelsParsingService)
    {
        _logger = logger;
        _configuration = configuration;
       _jsonModelsParsingService= jsonModelsParsingService;

        var host = _configuration["MQTT:host"];
        var port = int.Parse(_configuration["MQTT:port"]);

        _client = new MqttClient(host, port, false, MqttSslProtocols.None, null, null);
    }

    public override Task ExecuteTask => base.ExecuteTask;

    public override void Dispose()
    {
        base.Dispose();
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var clientId = Guid.NewGuid().ToString();
        var topic = _configuration["MQTT:topic"];
        var username = _configuration["MQTT:username"];
        var password = _configuration["MQTT:password"];

        _client.Connect(clientId, username, password);
        if (_client.IsConnected)
        {
            _logger.LogInformation("Connected to TTN via MQTT");

            _client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            _client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
            _jsonModelsParsingService.JsonToDatabase(_json);
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