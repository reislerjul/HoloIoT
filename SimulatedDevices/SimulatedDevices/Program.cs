using System;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;

namespace CreateDeviceIdentity
{
    class Program
    {

        static RegistryManager registryManager;

        // TODO: Insert your IoT hub connection string
        static string connectionString = "";

        static string[] devices = new string[10];
        

        private static async Task AddDeviceAsync(string deviceId)
        {
            Device device;
            try
            {
                device = await registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }
            Console.WriteLine("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
        }

        static void Main(string[] args)
        {
            for (int i = 1; i <= 10; i++)
            {
                devices[i - 1] = "device" + i.ToString();
            }
            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            AddDeviceAsync("myFirstDevice").Wait();
            foreach (string id in devices)
            {
                AddDeviceAsync(id).Wait();
            }
            Console.ReadLine();
        }
    }
}
