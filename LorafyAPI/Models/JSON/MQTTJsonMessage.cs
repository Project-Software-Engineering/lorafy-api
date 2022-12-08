namespace LorafyAPI.Models.JSON
{
    public class MQTTJsonMessage
    {
        public MQTTJsonEndDeviceIds end_device_ids { get; set; }
        public List<string> correlation_ids { get; set; }
        public string received_at { get; set; }
        public List<MQTTJsonRXMetadata> rx_metadata { get; set; }
        public MQTTJsonSettings settings { get; set; }
        public MQTTJsonVersionIds? version_ids { get; set; }
        public MQTTJsonUplinkMessage uplink_message { get; set; }
    }
}

