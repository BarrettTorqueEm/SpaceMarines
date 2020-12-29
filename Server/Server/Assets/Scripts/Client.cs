using System;
using NUnit.Framework.Internal;
using SimpleTcp;

public class Client {
    public SimpleTcpClient tcp { get; private set; }

    public Client(string ServerIp, string ServerPort) {
        //TODO: Sanitize inputs
        tcp = new SimpleTcpClient(ServerIp + ":" + ServerPort);
        Init();
    }

    private void Init() {
        tcp.Events.Connected += Connected;
        tcp.Events.Disconnected += Disconnected;
        tcp.Events.DataReceived += DataReceived;
        tcp.Logger = Log;

        tcp.Connect();
    }

    private void Log(string msg) {
        LogHandler.LogMessage(LogLevel.Info, this, msg);
    }

    private void DataReceived(object sender, DataReceivedEventArgs e) {
        throw new NotImplementedException();
    }

    private void Disconnected(object sender, ClientDisconnectedEventArgs e) {
        throw new NotImplementedException();
    }

    private void Connected(object sender, ClientConnectedEventArgs e) {
        LogHandler.LogMessage(LogLevel.Info, sender, "Client connected to server at " + e.IpPort.ToString());
    }
}