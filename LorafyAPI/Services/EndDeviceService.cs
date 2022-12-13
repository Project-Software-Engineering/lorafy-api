using LorafyAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LorafyAPI.Services
{
    public class EndDeviceService
    {
        private readonly AppContext _context;

        public EndDeviceService(AppContext context)
        {
            _context = context;
        }

        public IEnumerable<EndDevice> GetEndDevices()
        {
            var test = _context.EndDevices.ToList();
            return Enumerable.Range(1, 5).Select(index => new EndDevice
            {
                EUI = "A84041C1818350AD",
                Address = "260B6BA6",
                Name = "lht-gronau",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Metadata = new EndDeviceMetadata
                {
                    Battery = null,
                    BatteryVoltage = 3.4f
                }
            })
.ToArray();
        }
    }
}
