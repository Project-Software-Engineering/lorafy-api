namespace LorafyAPI.Models
{
    public class EndDeviceDataPoint
    {
        public string EndDeviceEUI { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int MessageCount { get; set; }
        public EndDeviceDataPointPayload Payload { get; set; }
    }
}
