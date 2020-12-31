/*===============================================================
*Project:    SM5_Client
*Developer:  Michael Girard (mgirard989@outlook.com)
*Company:    TeamTorqueEmTech
*Date:       12/31/2020 14:44
*Version:    0.1
*Description: 
*================================================================*/
/*===============================================================
*   AUDIT LOG
*
*
*================================================================*/
using UnityEngine;
using SimpleTcp;
using BarrettTorqueEm.Utilities;
using System;
using System.Text;

namespace SM5_Client {
    public class NetworkManager : MonoBehaviour {
        public static NetworkManager instance;

        public SimpleTcpClient Client;

        private void Awake() {
            if (instance == null) {
                instance = this;
            }
            DontDestroyOnLoad(this);
        }
        public void InitClient(int port) {
            Client = new SimpleTcpClient(SystemManager.IP, port);

            Client.Events.Connected += ClientConnected;
            Client.Events.DataReceived += DataReceived;
            Client.Events.Disconnected += ClientDisconnect;
            Client.Logger = Log;
        }

        private void ClientConnected(object sender, ClientConnectedEventArgs e) {
            LogHandler.LogMessage(LogLevel.Info, sender, $"Client Connected to {e.IpPort}");

        }

        private void DataReceived(object sender, DataReceivedEventArgs e) {
            LogHandler.LogMessage(LogLevel.Info, sender, $"Sever said: {Encoding.UTF8.GetString(e.Data)}");
        }

        private void ClientDisconnect(object sender, ClientDisconnectedEventArgs e) {
            LogHandler.LogMessage(LogLevel.Info, sender, $"Client disconnected from {e.IpPort}");
        }

        private void Log(string obj) {
            LogHandler.LogMessage(LogLevel.Info, this, obj);
        }
    }
}