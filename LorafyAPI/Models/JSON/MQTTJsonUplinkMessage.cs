namespace LorafyAPI.Models.JSON
{
    public class MQTTJsonUplinkMessage
    {
        public string session_key_id { get; set; }
        public int f_port { get; set; }
        public int f_cnt { get; set; }
        public string frm_payload { get; set; }
        public Dictionary<string, string> decoded_payload { get; set; }
        public string received_at { get; set; }
        public string consumed_airtime { get; set; }
        public MQTTJsonNetworkIds network_ids { get; set; }
        public List<MQTTJsonRXMetadata> rx_metadata { get; set; }
        public MQTTJsonSettings settings { get; set; }
        public MQTTJsonVersionIds? version_ids { get; set; }
    }
}
