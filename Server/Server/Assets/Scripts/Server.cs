using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleTcp;
using System;

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
        Server.instance.Events.ClientConnected += ClientConnected;
        Server.instance.Events.ClientDisconnected += ClientDisconnected;
        Server.instance.Events.DataReceived += DataReceived;
        Server.instance.Logger = Log;

        LogHandler.CreateLog();
    }

    private void Log(string msg) {
        Debug.Log(msg);
        LogHandler.LogMessage(LogLevel.Debug, this.name, msg);
    }

    private void DataReceived(object sender, DataReceivedEventArgs e) {
        throw new NotImplementedException();
    }

    private void ClientDisconnected(object sender, ClientDisconnectedEventArgs e) {
        throw new NotImplementedException();
    }

    private void ClientConnected(object sender, ClientConnectedEventArgs e) {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update() {
        if (!UnityEditor.EditorApplication.isPlaying) {
            LogHandler.Close();
        }
    }

    private void OnDestroy() {
        LogHandler.Close();
    }

}
