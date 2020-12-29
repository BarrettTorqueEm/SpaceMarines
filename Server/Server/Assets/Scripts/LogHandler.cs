using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public enum LogLevel {
    Debug,
    Info,
    Warning,
    Error,
    Fatal,
}
public static class LogHandler {
    private static FileStream fs;

    public static void LogMessage(LogLevel lvl, object sender, string message) {
        byte[] info = Encoding.UTF8.GetBytes($"[{lvl.ToString()}] ({sender} - {DateTime.Now.ToString("HH:mm:ss")}): {message} \n");
        fs.Write(info, 0, info.Length);
    }

    public static void LogMessage(LogLevel lvl, string sender, string message) {

        byte[] info = Encoding.UTF8.GetBytes($"[{lvl.ToString()}] ({sender} - {DateTime.Now.ToString("HH:mm:ss")}): {message} \n");
        fs.Write(info, 0, info.Length);
    }

    public static void CreateLog() {
        Debug.Log("Creating log file at " + Application.persistentDataPath + "/log");

        bool exists = Directory.Exists(Application.persistentDataPath + "/log");

        if (!exists) {
            Directory.CreateDirectory(Application.persistentDataPath + "/log");
        }

        fs = File.Create(Application.persistentDataPath + "/log/SM5_Server_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss") + ".log");

        LogMessage(LogLevel.Info, "LogHandler.cs", "Product Version: " + Application.version);
        LogMessage(LogLevel.Info, "LogHandler.cs", "Unity Version: " + Application.unityVersion);
        LogMessage(LogLevel.Info, "LogHandler.cs", "Genuine: " + Application.genuine.ToString());
        LogMessage(LogLevel.Info, "LogHandler.cs", "Platform: " + Application.platform.ToString());
    }

    public static void Close() {
        LogMessage(LogLevel.Debug, "LogHandler.cs", "Closing");
        fs.Close();
    }
}