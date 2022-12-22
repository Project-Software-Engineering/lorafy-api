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

        public DataController(DataPointService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<EndDeviceDataPoint> Get() 
        {
            var deviceEUI = HttpContext.Request.Query["eui"];
            if (string.IsNullOrEmpty(deviceEUI))
            {
                throw new Exception("Please provide a device eui");
            }

            var from = HttpContext.Request.Query["from"];
            if (string.IsNullOrEmpty(from))
            {
                throw new Exception("Please provide an time period");
            }

            var to = HttpContext.Request.Query["to"];
            if (string.IsNullOrEmpty(to))
            {
                throw new Exception("Please provide an time period");
            }

            var dataPointsNum = HttpContext.Request.Query["datapoints"];
            if (string.IsNullOrEmpty(dataPointsNum))
            {
                throw new Exception("Please provide an amount of datapoints");
            }

            return _service.GetDataPoints(deviceEUI, long.Parse(from), long.Parse(to), int.Parse(dataPointsNum));
        }

    }
}
