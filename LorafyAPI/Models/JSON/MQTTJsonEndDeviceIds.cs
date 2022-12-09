namespace LorafyAPI.Models.JSON
{
    public class MQTTJsonEndDeviceIds
    {
        public string device_id { get; set; }
        public string dev_eui { get; set; }
        public string join_eui { get; set; }
        public string dev_addr { get; set; }
        public MQTTJsonApplicationIds application_ids { get; set; }
    }
}
