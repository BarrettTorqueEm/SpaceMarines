using System;
using System.Text;
using SimpleTcp;

namespace Test {
    class Program {
        public static SimpleTcpServer server;
        static void Main(string[] args) {
            Init();


            while (Console.ReadLine() != "QUIT") {
                Console.WriteLine("Say 'QUIT'");
            }
        }

        private static void Init() {
            server = new SimpleTcpServer("127.0.0.1:65340");

            server.Events.ClientConnected += ClientConnected;
            server.Events.ClientDisconnected += ClientDisconnected;
            server.Events.DataReceived += DataReceived;
            server.Logger = Log;

            server.Start();
        }

        private static void Log(string obj) {
            Console.WriteLine("Log: " + obj);
        }

        private static void ClientConnected(object sender, ClientConnectedEventArgs e) {
            Console.WriteLine("CLient Connected");
        }

        private static void ClientDisconnected(object sender, ClientDisconnectedEventArgs e) {
            Console.WriteLine("Client disconnected");
        }

        private static void DataReceived(object sender, DataReceivedEventArgs e) {
            Console.WriteLine("Client said: " + Encoding.UTF8.GetString(e.Data));
        }
    }
}
