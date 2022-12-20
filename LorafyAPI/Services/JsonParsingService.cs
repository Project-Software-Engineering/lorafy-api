using LorafyAPI.Entities;
using LorafyAPI.Models.JSON;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;

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
            if (model.rx_metadata != null) {
                foreach (var metadata in model.rx_metadata)
                {
                   var _gateway= new Gateway
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
                
            }
            else
            {
                throw new Exception("No gateways present in this message.");
            }

            var device = model.end_device_ids;
                var _enddevice = new EndDevice
                {
                    EUI = device.dev_eui,
                    Name = device.device_id,
                    Address = device.dev_addr
                };

            var _payload = model.uplink_message.decoded_payload;
            var _settings = model.settings.data_rate.lora;
            var _message = new UplinkMessage
            {
                EndDeviceEUI = _enddevice.EUI,
                EndDevice = _enddevice,
                Payload = new UplinkMessagePayload
                {
                    Battery = float.Parse(_payload["Bat_status"]),
                    BatteryVoltage = float.Parse(_payload["BatV"]),
                    TemperatureInside = float.Parse(_payload["TempC_SHT"]),
                    TemperatureOutside = float.Parse(_payload["temperature"]),
                    Humidity = float.Parse(_payload["Hum_SHT"]),
                    Light = float.Parse(_payload["light"]),
                    Pressure = float.Parse(_payload["pressure"]),

                },
                
                DataRate = new UplinkMessageDataRate
                    {
                        Bandwidth= _settings.bandwidth,
                        SpreadingFactor= _settings.spreading_factor,
                        CodingRate= _settings.coding_rate
                    },
                    DateReceived = DateTime.Parse(model.uplink_message.received_at)
                };
           
            foreach (var gateway in gateways) {
                _message.GatewayEUI = gateway.EUI;
                _message.Gateway = gateway;
                    }
            
                _context.EndDevices.Add(_enddevice);

                _context.UplinkMessages.Add(_message);
                _context.SaveChanges();
            
        }
    }

}