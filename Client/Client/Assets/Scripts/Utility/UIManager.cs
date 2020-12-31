/*===============================================================
*Product:    SM5_Client
*Developer:  Michael Girard (mgirard989@outlook.com)
*Company:    TeamTorqueEmTech
*Date:       12/29/2020 19:51
*Version:    0.1
*Description: Commands and lodgic to be used while in the UI
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

namespace SM5_Client {
    public class UIManager : MonoBehaviour {
        public void Quit() {
            MenuTools.Quit();
        }

        public void Join(int port) {
            if (port != SystemManager.TESTServer || port != SystemManager.PRODServer) {
                LogHandler.LogMessage(LogLevel.Error, this, $"Invalid port {port}");
                return;
            }

            NetworkManager.instance.InitClient(port);
        }
    }
}