using LorafyAPI.Models;
using LorafyAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LorafyAPI.Controllers
{
    [ApiController]
    [Route("api/end-device")]
    public class EndDeviceController : ControllerBase
    {
        private readonly EndDeviceService _service;

        public EndDeviceController(EndDeviceService service)
        {
            _service = service;
        }

        // TODO: Remove: In The end, this should return a Model instead of an Entity. But this is fine for testing purposes.
        [HttpGet]
        public IEnumerable<EndDevice> Get()
        {
            return _service.GetEndDevices();
        }
    }
}
