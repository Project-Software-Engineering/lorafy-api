namespace LorafyAPI.Models.JSON
{
    public class MQTTJsonRXMetadata
    {
        public int rssi { get; set; }
        public int channel_rssi { get; set; }
        public long snr { get; set; }
        public string time { get; set; }
        public long? timestamp { get; set; }
        public MQTTJsonRXMetadataGatewayIds? gateway_ids { get; set; }
        public MQTTJsonRXMetadataLocation? location { get; set; }
        public string uplink_token { get; set; }
        public string received_at { get; set; }
    }
}
