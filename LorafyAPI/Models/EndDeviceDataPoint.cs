namespace LorafyAPI.Models
{
    public class EndDeviceDataPoint
    {
        public long Index { get; set; }
        public string EndDeviceEUI { get; set; }
        public int MessageCount { get; set; }
        public EndDeviceDataPointPayload? Payload { get; set; }
    }
}
