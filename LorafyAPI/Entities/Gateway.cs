
namespace LorafyAPI.Entities
{
    public class GatewayLocation
    {
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public float? Altitude { get; set; }
    }

    public class Gateway
    {
        public string EUI { get; set; }
        public string Name { get; set; }
        public int RSSI { get; set; }
        public float SNR { get; set; }
        public GatewayLocation Location { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public List<UplinkMessage> UplinkMessages { get; set; }
    }
}
