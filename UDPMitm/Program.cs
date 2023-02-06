using System.Net;
using System.Text;
using System.Net.Sockets;

namespace UDPMitm
{
    class Program
    {
        private static IPEndPoint? remoteServerEndPoint;
        private static IPEndPoint? clientServerEndPoint;

        private static IPAddress? remoteServerIp;
        private static UdpClient? remoteServer;
        private static UdpClient? localServer;

        private static int remoteServerPort;
        private static int localServerPort;

        static void Main(string[] args)
        {
            Console.Title = "UDPMitm - By Moch";
            string[] lines =
            {
                @"  _    _ _____  _____  __  __ _____ _______ __  __ ",
                @" | |  | |  __ \|  __ \|  \/  |_   _|__   __|  \/  |",
                @" | |  | | |  | | |__) | \  / | | |    | |  | \  / |",
                @" | |  | | |  | |  ___/| |\/| | | |    | |  | |\/| |",
                @" | |__| | |__| | |    | |  | |_| |_   | |  | |  | |",
                @"  \____/|_____/|_|    |_|  |_|_____|  |_|  |_|  |_|", ""
            };
            foreach (string line in lines)
                Console.WriteLine(line);

            if (args.Length != 3)
            {
                Console.WriteLine("Usage: UDPMitm.exe <REMOTE_IP_ADDRESS> <REMOTE_IP_PORT> <LOCAL_PORT>");
                Console.ReadKey();
                return;
            }

            remoteServerIp = IPAddress.Parse(args[0]);

            remoteServerPort = int.Parse(args[1]);
            localServerPort = int.Parse(args[2]);

            remoteServer = new UdpClient();
            localServer = new UdpClient(localServerPort);

            clientServerEndPoint = new IPEndPoint(IPAddress.Any, 0);
            remoteServerEndPoint = new IPEndPoint(remoteServerIp, remoteServerPort);

            try
            {
                Console.WriteLine("Listening on port: " + localServerPort);
                byte[] initialData = localServer.Receive(ref clientServerEndPoint);

                Console.WriteLine("Initial data received.");

                remoteServer.Send(initialData, initialData.Length, remoteServerEndPoint);

                Task? localServerTask = Task.Run(() => HandleData(localServer));
                Task? remoteServerTask = Task.Run(() => HandleData(remoteServer));

                Task.WaitAll(localServerTask, remoteServerTask);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.ReadKey();
            }
        }

        private static void HandleData(UdpClient client)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                byte[] data = client.Receive(ref endPoint);

                UdpClient? targetClient = (client == localServer) ? remoteServer : localServer;
                IPEndPoint? targetEndPoint = (client == localServer) ? remoteServerEndPoint : clientServerEndPoint;

                targetClient.Send(data, data.Length, targetEndPoint);
                Console.WriteLine((client == localServer ? "ToServer --> " : "ToClient <-- ") +
                $"{Encoding.ASCII.GetString(data)}");
            }
        }
    }
}