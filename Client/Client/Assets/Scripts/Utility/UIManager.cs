/*===============================================================
*Product:    SM5_Client
*Developer:  Michael Girard (mgirard989@outlook.com)
*Company:    TeamTorqueEmTech
*Date:       12/29/2020 19:51
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

namespace SM5_Client {
    public class UIManager : MonoBehaviour {
        public void Quit() {
            MenuTools.Quit();
        }

        public void Join(int port) {
            LogHandler.LogMessage(LogLevel.Info, this, "Attempting to join " + port);
            SimpleTcpClient c = new SimpleTcpClient("127.0.0.1", port);
            c.Connect();

            c.Logger = Log;
        }

        private void Log(string message) {
            LogHandler.LogMessage(LogLevel.Info, this, message);
        }
    }
}