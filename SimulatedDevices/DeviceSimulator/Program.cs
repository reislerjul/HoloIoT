using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace SimulatedDevice
{
    class Program
    {

        static DeviceClient deviceClient;

        // TODO: fill in the URI for your IoT Hub
        static string iotHubUri = "";

        // TODO: device key corresponding to the device with ID myfirstdevice
        static string deviceKey = "";

        // TODO: device keys for the ten simulated devices
        static string[] deviceKeys = new string[10] {"",
            "", "", 
            "", "",
            "", "",
            "", "",
            ""};
        static string[] deviceids = new string[10] { "device1", "device2", "device3", "device4",
        "device5", "device6", "device7", "device8", "device9", "device10"};

        private static async void SendDeviceToCloudMessagesAsync()
        {
            // For non-anomaly devices, the temperature will be between 20 and 35 while the 
            // humidity will be between 60 and 80
            double minTemperature = 20;
            double minHumidity = 60;
            double minAnomTemperature = 65;
            double minAnomHumidity = 15;
            int messageId = 1;
            Random rand = new Random();

            while (true)
            {
                for (int i = 0; i < 10; i++)
                {
                    double currentTemperature = minTemperature + rand.NextDouble() * 15;
                    double currentHumidity = minHumidity + rand.NextDouble() * 20;

                    if (i == 2 || i == 7)
                    {
                        currentTemperature = minAnomTemperature + rand.NextDouble() * 15;
                        currentHumidity = minAnomHumidity + rand.NextDouble() * 20;
                    }

                    var telemetryDataPoint = new
                    {
                        messageId = messageId++,
                        deviceId = deviceids[i],
                        temperature = currentTemperature,
                        humidity = currentHumidity,
                        time = DateTimeOffset.UtcNow
                    };
                    var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                    var message = new Message(Encoding.ASCII.GetBytes(messageString));
                    message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");

                    await deviceClient.SendEventAsync(message);
                    Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);
                }
                await Task.Delay(5000);
            }
        }



        static void Main(string[] args)
        {
            Console.WriteLine("Simulated device\n");
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("myFirstDevice", deviceKey), TransportType.Mqtt);
            SendDeviceToCloudMessagesAsync();
            Console.ReadLine();
        }
    }
}
