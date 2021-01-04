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
using System.Text;
using BarrettTorqueEm.Utilities;

namespace SM5_Client.Net {
    public static class PacketHandler {
        public static void ParsePacket(Client sender, byte[] data) {
            string msg = Encoding.UTF8.GetString(data);

            string[] split = msg.Split(':');

            switch (split[0]) {
                case "99999":
                    break;
                default:
                    LogHandler.LogMessage(LogLevel.Warning, "PacketHander.cs", $"Invalid packet id {split[1]}");
                    break;
            }
        }
    }
}