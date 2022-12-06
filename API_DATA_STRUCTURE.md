# LoRaFY API structure
Version: 1

## GET /api/end-devices
```json
{
  "total": 2,
  "items": [
    {
      "eui": "AYFJDHFYGJEJ",
      "name": "lht-gronau",
      "address": "27427A4",
      "dateCreated": "2022-12-02",
      "dateUpdated": "2022-12-02",
      "metadata": {
        "batteryVoltage": 3.14,
        "battery": 123
      }
    },
    {
      "eui": "FJDJSOGJIGSD",
      "name": "py-saxion",
      "address": "37FDFA4",
      "dateCreated": "2022-12-02",
      "dateUpdated": "2022-12-02",
      "metadata": {
        "batteryVoltage": null,
        "battery": null
      }
    }
  ]
}
```

## GET /api/data?device-eui=FJDJSOGJIGSD&from=1670325881&to=1670425881&datapoints=24
```json
{
  "items": [
    {
      "index": 1,
      "device_eui": "FJDJSOGJIGSD",
      "date": "2022-12-02T00:00:00z",
      "data": { // Note: Every one of the data fields CAN be null.
        "temperature": 13.5,
        "humidity": 1,
        "light": 10,
        "pressure": 12
      }
    }
  ]
}
```

<!-- Ignore this for now, this is for version two: -->
<!-- # GET /api/messages
      "metadata": {
        "airtime": "10ms",
        "spreadingFactor": 7,
        "codingRate": "4/5",
        "bandwidth": 125000,
        "frequency": "868100000",
        "receivedAt": "2022-12-02T07:37:45.187496042Z",
      }, -->