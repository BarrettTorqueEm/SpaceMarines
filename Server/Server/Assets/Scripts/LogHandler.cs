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
        byte[] info = Encoding.UTF8.GetBytes($"[{lvl.ToString()}({sender}): {message}");
        fs?.Write(info, 0, info.Length);
    }

    public static void LogMessage(LogLevel lvl, string sender, string message) {
        byte[] info = Encoding.UTF8.GetBytes($"[{lvl.ToString()}({sender}): {message}");
        fs?.Write(info, 0, info.Length);
    }

    public static void CreateLog() {
        bool exists = Directory.Exists(Application.persistentDataPath + "/log");

        if (!exists) {
            Directory.CreateDirectory(Application.persistentDataPath + "/log");
        }

        fs = File.Create(Application.persistentDataPath + "/log/SM5_Server_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss") + ".log");

        LogMessage(LogLevel.Info, "LogHandler", "");
    }
}