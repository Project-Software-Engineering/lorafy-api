namespace LorafyAPI.Models
{
    public class EndDeviceDataPointPayloadModel
    {
        public float? TemperatureInside { get; set; }
        public float? TemperatureOutside { get; set; }
        public float? Humidity { get; set; }
        public float? Light { get; set; }
        public float? Pressure { get; set; }
    }
}
