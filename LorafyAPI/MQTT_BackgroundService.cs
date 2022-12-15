using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class MQTTBackgroundService : BackgroundService
{
    private readonly IConfiguration? Configuration;


    readonly ILogger<MQTTBackgroundService> _logger;
    private MqttClient? client;
    private string broker;
    private int port = 1883;
    private string topic;
    private string clientId = Guid.NewGuid().ToString();
    private string username;
    private string password;
    static public string? payload;


    static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {

        payload = System.Text.Encoding.Default.GetString(e.Message);
        Console.WriteLine(payload);
    }

    public MQTTBackgroundService(ILogger<MQTTBackgroundService> logger, IConfiguration configuration)
    {
        _logger = logger;
        Configuration = configuration;
    }

    public override Task ExecuteTask => base.ExecuteTask;



    public override void Dispose()
    {
        base.Dispose();
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        broker = Configuration["MQTT_variables:broker"];
        topic = Configuration["MQTT_variables:topic"];
        username = Configuration["MQTT_variables:username"];
        password = Configuration["MQTT_variables:password"];

        client = new MqttClient(broker, port, false, MqttSslProtocols.None, null, null);
        client.Connect(clientId, username, password);
        if (client.IsConnected)
        {
            _logger.LogInformation("Connected to TTN via MQTT.");

            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
           
   
        }
        else
        {
            _logger.LogError("Failed to connect.");

        }

        return Task.CompletedTask;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}