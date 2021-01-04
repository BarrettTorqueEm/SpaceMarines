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
using System.Collections;
using System.Text;
using System.Net.Sockets;
using SM5_Client.Utilities;

namespace SM5_Client.Net {
    public class NetworkManager : MonoBehaviour {
        public static NetworkManager instance;

        private SimpleTcpClient client;
        private void Awake() {
            if (instance == null) {
                instance = this;
            }
            DontDestroyOnLoad(this);
        }

        // public void TestConnection() {
        //     Join(new Client(SystemManager.IP + ":" + SystemManager.PRODServer));

        //     if (client.IsConnected) {
        //         client.Disconnect();
        //         SystemManager.instance.ChangeLevel(2);
        //     } else {
        //         LogHandler.LogMessage(LogLevel.Warning, this, "Could not connect to server.");
        //     }
        // }

        public bool Join(Client c) {
            SimpleTcpClient client = c.TcpClient;
            try {
                client = new SimpleTcpClient(SystemManager.IP, SystemManager.PRODServer);

                client.Events.Connected += ClientConnected;
                client.Events.DataReceived += DataReceived;
                client.Events.Disconnected += ClientDisconnect;
                client.Logger = Log;

                client.Connect();

                StartCoroutine(AwaitConnection());
            } catch (SocketException e) {
                LogHandler.LogMessage(LogLevel.Warning, this, e.Message);
                StopCoroutine(AwaitConnection());
            }

            return client.IsConnected;
        }

        private IEnumerator AwaitConnection() {
            LogHandler.LogMessage(LogLevel.Info, this, "Attempting to connect to server.");
            bool waiting = true;
            float timeout = 5f;

            while (waiting) {
                if (client.IsConnected)
                    break;

                timeout -= Time.deltaTime;

                if (timeout <= 0f)
                    break;

                yield return null;
            }
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