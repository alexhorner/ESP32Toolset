using System;
using System.Net.NetworkInformation;
using System.Threading;
using Windows.Devices.WiFi;

namespace ESP32Toolset.Communication
{
    /// <summary>
    /// Allows the simple control of a network connection
    /// </summary>
    public class NetworkController
    {
        /// <summary>
        /// The automatically selected WiFi adapter used to connect to the network
        /// </summary>
        public readonly WiFiAdapter Adapter = WiFiAdapter.FindAllAdapters()[0];

        /// <summary>
        /// The network interface the controller is attached to
        /// </summary>
        public readonly NetworkInterface Interface;

        /// <summary>
        /// Instantiate a controller which automatically establishes a network connection
        /// </summary>
        /// <param name="networkSsid">The SSID to connect to</param>
        /// <param name="networkPassword">The password for the network, if required</param>
        public NetworkController(string networkSsid, string networkPassword = "")
        {
            Interface = NetworkInterface.GetAllNetworkInterfaces()[Adapter.NetworkInterface];

            Adapter.ScanAsync();

            WiFiAvailableNetwork requestedNetwork = null;

            while (true)
            {
                while (Adapter.NetworkReport.AvailableNetworks.Length < 0)
                {
                    Thread.Sleep(100);
                }

                foreach (WiFiAvailableNetwork network in Adapter.NetworkReport.AvailableNetworks)
                {
                    if (network.Ssid.ToLower() == networkSsid.ToLower())
                    {
                        requestedNetwork = network;
                    }
                }

                if (requestedNetwork != null)
                {
                    break;
                }

                Thread.Sleep(10000);

                Adapter.ScanAsync();
            }

            WiFiConnectionResult connection = Adapter.Connect(requestedNetwork, WiFiReconnectionKind.Automatic, networkPassword);

            if (connection.ConnectionStatus != WiFiConnectionStatus.Success)
            {
                throw new Exception($"Failed to connect to the requested network: {connection.ConnectionStatus.ToString()}");
            }
        }
    }
}
