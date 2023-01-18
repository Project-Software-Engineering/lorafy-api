using LorafyAPI.Models;
using LorafyAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
namespace LorafyAPI.Controllers
{
    [ApiController]
    [Route("api/end-device")]
    public class EndDeviceController : ControllerBase
    {
        private readonly EndDeviceService _service;
        private readonly ILogger<EndDeviceController> _logger;

        private IMemoryCache _cache;
        private const string CacheKey = "enddevicelist";

        public EndDeviceController(EndDeviceService service, IMemoryCache cache, ILogger<EndDeviceController> logger)
        {
            _service = service;
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Gets all end devices.
        /// </summary>
        /// <returns>A list of all end devices currently in the database.</returns>
        [HttpGet]
        public IEnumerable<EndDevice> Get()
        {
            _logger.Log(LogLevel.Information, "Trying to fetch the list of employees from cache.");
            if (_cache.TryGetValue(CacheKey, out IEnumerable<EndDevice> endDevices))
            {
                _logger.Log(LogLevel.Information, "End Device found in cache.");
            }
            else
            {
                _logger.Log(LogLevel.Information, "End Devices not in cache. Retrieving from Database.");
                endDevices = _service.GetEndDevices();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3))
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSize(1024);

                _cache.Set(CacheKey, endDevices, cacheEntryOptions);
                return endDevices;

            }

            return endDevices;
        }


    }
}