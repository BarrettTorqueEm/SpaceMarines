using System;
using System.Text;
using SimpleTcp;
using BarrettTorqueEm.Utilities;
using System.Collections.Generic;

class Server {
    public static SimpleTcpServer server;

    private static List<Client> clients = new List<Client>();
    private static bool isRunning = false;
    static void Main(string[] args) {
        Init();

        while (isRunning) {
            Console.Write(">");
            string input = Console.ReadLine();

            switch (input) {
                case "status":
                    PrintStatus();
                    break;
                case "clients":
                    ListClients();
                    break;
                case "quit":
                    Stop();
                    break;
                default:
                    DisplayMenu();
                    break;
            }
        }
    }

    private static void DisplayMenu() {
        Console.Clear();
        Console.WriteLine("Menu:");
        Console.WriteLine("\tStatus");
        Console.WriteLine("\tClients");
        Console.WriteLine("\tQuit");
    }

    private static void Init() {
        LogHandler.CreateLog();
        server = new SimpleTcpServer("192.168.254.100:65341");

        server.Events.ClientConnected += ClientConnected;
        server.Events.ClientDisconnected += ClientDisconnected;
        server.Events.DataReceived += DataReceived;
        server.Logger = Log;

        PacketHandler.Init();

        server.Start();
        isRunning = true;
        DisplayMenu();
    }

    private static void Log(string obj) {
        LogHandler.LogMessage(LogLevel.Info, "Server.cs", obj);
    }

    private static void ClientConnected(object sender, ClientConnectedEventArgs e) {
        Client c = new Client(e.IpPort);
        clients.Add(c);
        server.Send(c.IPport, Encoding.UTF8.GetBytes("CONNECTED"));

        LogHandler.LogMessage(LogLevel.Info, sender, $"Client {e.IpPort} connected at {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");
    }

    private static void ClientDisconnected(object sender, ClientDisconnectedEventArgs e) {
        Console.WriteLine("Client disconnected");
    }

    private static void DataReceived(object sender, DataReceivedEventArgs e) {
        Console.WriteLine("Client said: " + Encoding.UTF8.GetString(e.Data));
    }
    private static void PrintStatus() {
        int count = 0;

        foreach (Client c in clients)
            count++;

        string msg = "*************************************\n";
        msg += $"Clients connected: {count}\n";
        msg += $"Is Listening: {server.IsListening}\n";
        msg += $"Keepalive: {server.Keepalive.EnableTcpKeepAlives}\n";
        msg += $"Start Time: {server.Statistics.StartTime}\n";
        msg += $"Up Time: {server.Statistics.UpTime}\n";
        msg += $"Sent Bytes: {server.Statistics.SentBytes}\n";
        msg += $"Recieved Bytes: {server.Statistics.ReceivedBytes}\n";
        msg += "**********************************************\n\n";

        LogHandler.LogMessage(LogLevel.Info, "Program.cs", msg);
    }

    private static void ListClients() {
        string msg = $"Clients ({clients.Count}): \n";

        if (clients.Count == 0) {
            LogHandler.LogMessage(LogLevel.Info, "Server.cs", "No Clients connected.");
            return;
        }

        foreach (string ip in server.GetClients()) {
            foreach (Client c in clients)
                if (c.IPport == ip)
                    msg += $"\t{c.UName}";
        }
        LogHandler.LogMessage(LogLevel.Info, "Server.cs", msg);
    }

    private static void Stop(string msg = "Safe stopping") {
        LogHandler.LogMessage(LogLevel.Info, "Server.cs", $"Stopping server due to {msg}");

        foreach (string c in server.GetClients()) {
            //TODO: Send message to clients the server is shutting down
            server.DisconnectClient(c);
        }

        if (server.IsListening)
            server.Stop();

        isRunning = false;
    }
}
