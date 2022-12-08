
namespace LorafyAPI.Entities
{
    public class UplinkMessagePayload
    {
        public float Battery { get; set; }
        public float BatteryVoltage { get; set; }
        public float TemperatureInside { get; set; }
        public float TemperatureOutside { get; set; }
        public float Humidity { get; set; }
        public float Light { get; set; }
        public float Pressure { get; set; }
    }

    public class UplinkMessageDataRate
    {
        public int Bandwidth { get; set; }
        public int SpreadingFactor { get; set; }
        public string CodingRate { get; set; }
    }

    public class UplinkMessage
    {
        public int Id { get; set; }
        public string EndDeviceEUI { get; set; }
        public EndDevice EndDevice { get; set; }
        public string GatewayEUI { get; set; }
        public Gateway Gateway { get; set; }
        public UplinkMessagePayload Payload { get; set; }
        public UplinkMessageDataRate DataRate { get; set; }
        public DateTime DateReceived { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
