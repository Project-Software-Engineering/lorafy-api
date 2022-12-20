using LazyCache;
using LorafyAPI.Models;
using LorafyAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching;
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


        /// <summary>
        /// Gets all end devices.
        /// </summary>
        /// <returns>A list of all end devices currently in the database.</returns>
        [HttpGet]
        public IEnumerable<EndDevice> Get()
        {
            return _service.GetEndDevices();
        }
    }
}
