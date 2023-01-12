using LorafyAPI.Entities;
using LorafyAPI.Models.JSON;
using Newtonsoft.Json;
using System.Globalization;

namespace LorafyAPI.Services
{
    public class JsonModelsParsingService
    {
        private readonly AppDbContext _context;

        public JsonModelsParsingService(AppDbContext context) => _context = context;

        public void JsonToDatabase(string _jsonString)
        {
            var model = JsonConvert.DeserializeObject<MQTTJsonMessage>(_jsonString);
            if (model == null || model.uplink_message == null)
            {
                // It's null for some reason, so we don't care about the message
                return;
            }

            List<Gateway> gateways = new List<Gateway>();
            foreach (var metadata in model.uplink_message.rx_metadata)
            {
                // Some gateways may not have ids, so skip them.
                if (metadata.gateway_ids == null || metadata.gateway_ids.eui == null)
                {
                    continue;
                }

                var gateway = new Gateway
                {
                    EUI = metadata.gateway_ids.eui,
                    Name = metadata.gateway_ids.gateway_id,
                    RSSI = metadata.rssi,
                    SNR = metadata.snr,

                    Location = new GatewayLocation
                    {
                        Altitude = metadata.location.altitude,
                        Latitude = metadata.location.latitude,
                        Longitude = metadata.location.longitude
                    }
                };
                gateways.Add(gateway);
            }

            var device = model.end_device_ids;
            var endDevice = new EndDevice
            {
                EUI = device.dev_eui,
                Name = device.device_id,
                Address = device.dev_addr
            };

            var payload = model.uplink_message.decoded_payload;
            var settings = model.uplink_message.settings.data_rate.lora;
            var messagePayload = new UplinkMessagePayload();


            if (payload.ContainsKey("Bat_status"))
            {
                messagePayload.Battery = float.Parse(payload["Bat_status"], NumberStyles.Any, CultureInfo.InvariantCulture);
            }
            if (payload.ContainsKey("BatV"))
            {
                messagePayload.BatteryVoltage = float.Parse(payload["BatV"], NumberStyles.Any, CultureInfo.InvariantCulture);
            }
            if (payload.ContainsKey("Hum_SHT"))
            {
                messagePayload.Humidity = float.Parse(payload["Hum_SHT"], NumberStyles.Any, CultureInfo.InvariantCulture);
            }
            if (payload.ContainsKey("ILL_lx"))
            {
                messagePayload.Light = float.Parse(payload["ILL_lx"], NumberStyles.Any, CultureInfo.InvariantCulture);
            }
            if (payload.ContainsKey("light"))
            {
                messagePayload.Light = float.Parse(payload["light"], NumberStyles.Any, CultureInfo.InvariantCulture);
            }
            if (payload.ContainsKey("pressure"))
            {
                messagePayload.Pressure = float.Parse(payload["pressure"], NumberStyles.Any, CultureInfo.InvariantCulture);
            }
            if (payload.ContainsKey("temperature"))
            {
                messagePayload.TemperatureInside = float.Parse(payload["temperature"], NumberStyles.Any, CultureInfo.InvariantCulture);
            }
            if (payload.ContainsKey("TempC_DS") && payload.ContainsKey("TempC_SHT"))
            {
                messagePayload.TemperatureInside = float.Parse(payload["TempC_SHT"], NumberStyles.Any, CultureInfo.InvariantCulture);
                messagePayload.TemperatureOutside = float.Parse(payload["TempC_DS"], NumberStyles.Any, CultureInfo.InvariantCulture);
            }
            else if (payload.ContainsKey("TempC_SHT"))
            {
                messagePayload.TemperatureOutside = float.Parse(payload["TempC_SHT"], NumberStyles.Any, CultureInfo.InvariantCulture);
            }

            var message = new UplinkMessage
            {
                EndDeviceEUI = endDevice.EUI,
                EndDevice = endDevice,
                Payload = messagePayload,

                DataRate = new UplinkMessageDataRate
                {
                    Bandwidth = settings.bandwidth,
                    SpreadingFactor = settings.spreading_factor,
                    CodingRate = settings.coding_rate
                },
                DateReceived = DateTime.Parse(model.uplink_message.received_at)
            };

            foreach (var gateway in gateways)
            {
                if (gateway.EUI != null)
                {
                    message.GatewayEUI = gateway.EUI;

                    message.Gateway = gateway;
                    if (gateway != null)
                    {
                        if (_context.Gateways.Any(x => x.EUI == gateway.EUI))
                        {
                            _context.Gateways.Update(gateway);
                        }
                        else
                        {
                            _context.Gateways.Add(gateway);
                        }
                    }
                }
            }

            if (_context.EndDevices.Any(x => x.EUI == endDevice.EUI))
            {
                _context.EndDevices.Update(endDevice);
            }
            else
            {
                _context.EndDevices.Add(endDevice);
            }
            _context.UplinkMessages.Add(message);
            _context.SaveChanges();
        }
    }

}