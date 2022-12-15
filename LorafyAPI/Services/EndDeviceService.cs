using LorafyAPI.Models;

namespace LorafyAPI.Services
{
    public class EndDeviceService
    {
        private readonly AppDbContext _context;

        public EndDeviceService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all end devices with their corrosponding metadata.
        /// The metadata is fetched from the latest uplink message of the corrosponding device.
        /// </summary>
        /// <returns>All EndDevice models from the database.</returns>
        public IEnumerable<EndDevice> GetEndDevices()
        {
            // This query gets all end devices in the database and joins it with the most recent uplink messages for that end device.
            // We then include the battery from that latest message.
            var endDevicesQuery =
                from endDevices in _context.EndDevices
                join uplinkMessages in _context.UplinkMessages on endDevices.EUI equals uplinkMessages.EndDeviceEUI
                let maxId = (
                    from m in _context.UplinkMessages
                    where m.EndDeviceEUI == endDevices.EUI
                    select m.Id
                ).Max()
                where uplinkMessages.Id == maxId
                select new EndDevice
                {
                    EUI = endDevices.EUI,
                    Name = endDevices.Name,
                    Address = endDevices.Address,
                    DateCreated = endDevices.DateCreated,
                    DateUpdated = endDevices.DateUpdated,
                    Metadata = new EndDeviceMetadata {
                        BatteryVoltage = uplinkMessages.Payload.BatteryVoltage,
                        Battery = uplinkMessages.Payload.Battery
                    }
                };

            return endDevicesQuery.ToList();
        }
    }
}
