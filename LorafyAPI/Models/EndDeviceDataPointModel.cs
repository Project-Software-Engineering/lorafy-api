namespace LorafyAPI.Models
{
    public class EndDeviceDataPointModel
    {
        public int Index { get; set; }
        public string DeviceEUI { get; set; }
        public string Date { get; set; }
        public EndDeviceDataPointPayloadModel Payload { get; set; }
    }
}
