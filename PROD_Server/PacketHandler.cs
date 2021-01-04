using System.Collections;
using System.Collections.Generic;
using System.Text;
using BarrettTorqueEm.Utilities;

static class PacketHandler {
    public static Dictionary<int, string> PacketDef = new Dictionary<int, string>();

    public static void Init() {
        PacketDef.Add(635088, "Connected");
    }

    public static void ParsePacket(Client sender, byte[] data) {
        string msg = Encoding.UTF8.GetString(data);

        string[] split = msg.Split(':');

        switch (split[0]) {
            case "99999":
                ClientConnected(sender, split[1]);
                break;
            default:
                LogHandler.LogMessage(LogLevel.Warning, "PacketHander.cs", $"Invalid packet id {split[1]}");
                break;
        }
    }

    private static void ClientConnected(Client sender, string Username) {
        sender.SetName(Username);
        LogHandler.LogMessage(LogLevel.Info, sender, $"Client {sender.IPport} set Uname to {sender.UName}");
    }
}