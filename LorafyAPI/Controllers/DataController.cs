using LorafyAPI.Models;
using LorafyAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LorafyAPI.Controllers
{
    [ApiController]
    [Route("api/data")]
    public class DataController : ControllerBase
    {
        private readonly DataPointService _service;
        private readonly IConfiguration _configuration;

        public DataController(DataPointService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<IEnumerable<EndDeviceDataPoint>> Get([FromQuery] string eui, [FromQuery] long from, [FromQuery] long to, [FromQuery] int datapoints)
        {
            // Make sure from, to and data points are positive
            if (from < 0 || to < 0 || datapoints < 0)
            {
                return BadRequest(new { error = "From, to and data points must be positive" });
            }
            var maxDataPoints = _configuration.GetValue<int>("MaxDataPoints");
            if (maxDataPoints != 0 && datapoints > maxDataPoints)
            {
                return BadRequest(new { error = $"Data points must be less than {maxDataPoints}" });
            }

            return Ok(_service.GetDataPoints(eui, from, to, datapoints));
        }

    }
}
