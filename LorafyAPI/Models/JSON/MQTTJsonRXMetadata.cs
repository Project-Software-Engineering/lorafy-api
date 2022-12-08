namespace LorafyAPI.Models.JSON
{
    public class MQTTJsonRXMetadata
    {
        public int rssi { get; set; }
        public int channel_rssi { get; set; }
        public int snr { get; set; }
        public string time { get; set; }
        public int? timestamp { get; set; }
        public MQTTJsonRXMetadataLocation? location { get; set; }
        public string uplink_token { get; set; }
        public string received_at { get; set; }
    }
}
