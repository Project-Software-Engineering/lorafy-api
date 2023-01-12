using LorafyAPI.Entities;
using LorafyAPI.Models.JSON;
using Newtonsoft.Json;

namespace LorafyAPI.Services
{
    public class JsonModelsParsingService
    {

        private readonly AppDbContext _context;

        public JsonModelsParsingService(AppDbContext context) => _context = context;
       

        public void JsonToDatabase(string _jsonString)
        {
            var model = JsonConvert.DeserializeObject<MQTTJsonMessage>(_jsonString);
            List<Gateway> gateways = new List<Gateway>();


           foreach (var metadata in model.uplink_message.rx_metadata)
                {
              
                    var _gateway = new Gateway
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
                    gateways.Add(_gateway);
               }            

            var device = model.end_device_ids;
            var _enddevice = new EndDevice
            {
                EUI = device.dev_eui,
                Name = device.device_id,
                Address = device.dev_addr
            };

            var _payload = model.uplink_message.decoded_payload;
            var _settings = model.uplink_message.settings.data_rate.lora;
            var payload = new UplinkMessagePayload();
         
       
            if (_payload.ContainsKey("Bat_status"))
            {
                payload.Battery = float.Parse(_payload["Bat_status"]);
            }
            if (_payload.ContainsKey("BatV"))
            {
                payload.BatteryVoltage = float.Parse(_payload["BatV"]);
            }

            if (_payload.ContainsKey("Hum_SHT"))
            {
                payload.Humidity = float.Parse(_payload["Hum_SHT"]);
            }
            if (_payload.ContainsKey("ILL_lx"))
            {
                payload.Light = float.Parse(_payload["ILL_lx"]);
            }
            if (_payload.ContainsKey("light"))
            {
                payload.Light = float.Parse(_payload["light"]);
            }
            if (_payload.ContainsKey("pressure"))
            {
                payload.Pressure = float.Parse(_payload["pressure"]);
            }
            if (_payload.ContainsKey("temperature"))
            {
                payload.TemperatureInside = float.Parse(_payload["temperature"]);
            }
            if (_payload.ContainsKey("TempC_DS") && _payload.ContainsKey("TempC_SHT"))
            {
                payload.TemperatureInside = float.Parse(_payload["TempC_SHT"]);
                payload.TemperatureOutside = float.Parse(_payload["TempC_DS"]);
            }
            else if (_payload.ContainsKey("TempC_SHT"))
            {
                payload.TemperatureOutside = float.Parse(_payload["TempC_SHT"]);
            }


            var _message = new UplinkMessage
            {
                EndDeviceEUI = _enddevice.EUI,
                EndDevice = _enddevice,
                Payload = payload,

               DataRate = new UplinkMessageDataRate
                {
                    Bandwidth = _settings.bandwidth,
                    SpreadingFactor = _settings.spreading_factor,
                    CodingRate = _settings.coding_rate
                },
                DateReceived = DateTime.Parse(model.uplink_message.received_at)
            };

            foreach (var gateway in gateways)
            {
                if (gateway.EUI != null)
                {
                    _message.GatewayEUI = gateway.EUI;

                    _message.Gateway = gateway;
                    if (gateway != null )
                    {
                        _context.Gateways.Update(gateway);
                    }
                }
            }

            _context.EndDevices.Update(_enddevice);
           
            _context.UplinkMessages.Add(_message);
            _context.SaveChanges();

        }
    }

}