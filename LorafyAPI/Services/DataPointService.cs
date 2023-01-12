using LorafyAPI.Entities;
using LorafyAPI.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace LorafyAPI.Services
{
    public class DataPointService
    {
        private readonly AppDbContext _context;

        public DataPointService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the average data points for the given end devices and time period.
        /// </summary>
        /// <param name="endDeviceEUI">The end device to get the data points for</param>
        /// <param name="from">From which timepoint</param>
        /// <param name="to">To which timepoint</param>
        /// <param name="count">The amount of datapoints</param>
        /// <returns>A list of data points</returns>
        public IEnumerable<EndDeviceDataPoint> GetDataPoints(string endDeviceEUI, long from, long to, int count)
        {
            var fromDate = DateTimeOffset.FromUnixTimeSeconds(from).DateTime;
            var toDate = DateTimeOffset.FromUnixTimeSeconds(to).DateTime;
            var interval = (to - from) / count;
            var query =
                from m in _context.UplinkMessages
                where m.EndDeviceEUI == endDeviceEUI
                    && m.DateReceived >= fromDate
                    && m.DateReceived <= toDate
                group m by
                    Math.Floor((decimal)EF.Functions.DateDiffSecond(fromDate, m.DateReceived) / interval) * interval
                    into g
                select new
                {
                    AverageTemperatureInside = g.Average(m => m.Payload.TemperatureInside),
                    AverageTemperatureOutside = g.Average(m => m.Payload.TemperatureOutside),
                    AverageHumidity = g.Average(m => m.Payload.Humidity),
                    AverageLight = g.Average(m => m.Payload.Light),
                    AveragePressure = g.Average(m => m.Payload.Pressure),
                    MessageCount = g.Count(),
                    FirstMessageDate = g.Min(m => m.DateReceived),
                    LastMessageDate = g.Max(m => m.DateReceived)
                };

            return query.ToList().Select((x) => new EndDeviceDataPoint
            {
                EndDeviceEUI = endDeviceEUI,
                FromDate = x.FirstMessageDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                ToDate = x.LastMessageDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                MessageCount = x.MessageCount,
                Payload = new EndDeviceDataPointPayload
                    {
                        Humidity = x.AverageHumidity,
                        Light = x.AverageLight,
                        Pressure = x.AveragePressure,
                        TemperatureInside = x.AverageTemperatureInside,
                        TemperatureOutside = x.AverageTemperatureOutside,
                    }
            });
        }
    }
}