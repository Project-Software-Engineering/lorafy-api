namespace LorafyAPI.Models.JSON
{
    public class MQTTJsonSettings
    {
        public MQTTJsonSettingsDataRate data_rate { get; set; }
        public string frequency { get; set; }
        public long? timestamp { get; set; }
        public string? time { get; set; }
    }
}
