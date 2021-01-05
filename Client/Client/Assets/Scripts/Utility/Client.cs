/*===============================================================
*Project:    SM5_Client
*Developer:  Michael Girard (mgirard989@outlook.com)
*Company:    TeamTorqueEmTech
*Date:       01/03/2021 20:00
*Version:    0.1
*Description: 
*================================================================*/
/*===============================================================
*   AUDIT LOG
*
*
*================================================================*/
using SimpleTcp;

namespace SM5_Client.Net {
    public class Client {
        public SimpleTcpClient TcpClient { get; set; }
        public string UName { get; set; }

        public Client(SimpleTcpClient client) {
            TcpClient = client;
            UName = "BarrettTorqueEm";
        }
    }
}