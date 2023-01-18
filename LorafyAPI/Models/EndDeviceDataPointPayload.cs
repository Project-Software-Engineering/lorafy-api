namespace LorafyAPI.Models
{
    public class EndDeviceDataPointPayload
    {
        public float? TemperatureInside { get; set; }
        public float? TemperatureOutside { get; set; }
        public float? Humidity { get; set; }
        public float? Light { get; set; }
        public float? Pressure { get; set; }
    }
}
