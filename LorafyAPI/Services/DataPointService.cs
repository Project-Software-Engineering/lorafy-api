using LorafyAPI.Entities;
using LorafyAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MySqlConnector;
using System.Data;
using System.Data.Common;

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
        /// <param name="datapoints">The amount of datapoints</param>
        /// <returns>A list of data points</returns>
        public IEnumerable<EndDeviceDataPoint> GetDataPoints(string endDeviceEUI, long from, long to, int datapoints)
        {
            long interval = (to - from) / datapoints;
            using var command = _context.Database.GetDbConnection().CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_GetAverageDataPoints";
            command.Parameters.Add(new MySqlParameter("DEVICE_EUI", endDeviceEUI));
            command.Parameters.Add(new MySqlParameter("FROM_TIMESTAMP", from));
            command.Parameters.Add(new MySqlParameter("TO_TIMESTAMP", to));
            command.Parameters.Add(new MySqlParameter("DATAPOINT_COUNT", datapoints));

            _context.Database.OpenConnection();
            using var reader = command.ExecuteReader();
            var dbResults = new List<EndDeviceDataPoint>();

            while (reader.Read())
            {
                dbResults.Add(new EndDeviceDataPoint
                {
                    Index = (long)Math.Floor(reader.GetDouble("TimeInterval")) / interval,
                    EndDeviceEUI = endDeviceEUI,
                    MessageCount = reader.GetInt32("MessageCount"),
                    Payload = new EndDeviceDataPointPayload
                    {
                        TemperatureInside = GetPayloadField(reader, "Average_TemperatureInside"),
                        TemperatureOutside = GetPayloadField(reader, "Average_TemperatureOutside"),
                        Humidity = GetPayloadField(reader, "Average_Humidity"),
                        Light = GetPayloadField(reader, "Average_Light"),
                        Pressure = GetPayloadField(reader, "Average_Pressure")
                    }
                });
            }
            _context.Database.CloseConnection();

            // Insert null values for the missing payloads
            var results = new List<EndDeviceDataPoint?>();
            for (int i = 0; i < datapoints; i++)
            {
                var dp = dbResults.Find(x => x.Index == i);

                results.Add(dp ?? new EndDeviceDataPoint { Index = i, EndDeviceEUI = endDeviceEUI, MessageCount = 0 });
            }


            return results;
        }

        private static float? GetPayloadField(DbDataReader reader, string fieldName)
        {
            return !reader.IsDBNull(fieldName) ? (float)Math.Round(reader.GetFloat(fieldName), 2) : null;
        }
    }
}