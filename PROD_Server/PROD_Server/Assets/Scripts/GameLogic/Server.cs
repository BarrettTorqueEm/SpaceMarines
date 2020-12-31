using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleTcp;
using System;
using BarrettTorqueEm.Utilities;

public class Server : MonoBehaviour {
    public static SimpleTcpServer instance;

    private void Awake() {
        if (instance == null) {
            instance = new SimpleTcpServer("127.0.0.1:9999");
        }

        Init();

        instance.Start();
    }

    private void Init() {
        LogHandler.CreateLog();

        Server.instance.Logger = Log;
        Server.instance.Events.ClientConnected += ClientConnected;
        Server.instance.Events.ClientDisconnected += ClientDisconnected;
        Server.instance.Events.DataReceived += DataReceived;
    }

    private void Log(string msg) {
        LogHandler.LogMessage(LogLevel.Info, this.name, msg);
    }

    private void DataReceived(object sender, DataReceivedEventArgs e) {
        throw new NotImplementedException();
    }

    private void ClientDisconnected(object sender, ClientDisconnectedEventArgs e) {
        throw new NotImplementedException();
    }

    private void ClientConnected(object sender, ClientConnectedEventArgs e) {
        LogHandler.LogMessage(LogLevel.Info, sender, $"Client connected ({e.IpPort})");
    }

    // Update is called once per frame
    void Update() {

    }

    //Close the Logger when the program exits
    private void OnDestroy() {
        Server.instance.Stop();
        LogHandler.Close();
    }

}
