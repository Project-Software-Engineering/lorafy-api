using LorafyAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LorafyAPI.Controllers
{
    [ApiController]
    [Route("api/end-device")]
    public class EndDeviceController : ControllerBase
    {
        // In The end, this should return a Model instead of an Entity. But this is fine for testing purposes.
        [HttpGet]
        public EndDevice Get()
        {
            return new EndDevice
            {
               EUI = "A84041C1818350AD",
               Address = "260B6BA6",
               Name = "lht-gronau",
               DateCreated = DateTime.Now,
               DateUpdated = DateTime.Now,
            };
        }
    }
}
