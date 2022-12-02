namespace LorafyAPI.Entities
{
    public class EndDevice
    {
        public string EUI { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
