/*===============================================================
*	Project:    	Libary
*	Developer:  	Michael Girard (mgirard989@outlook.com)
*	Company:    	TeamTorqueEmTech
*	Creation Date:	12/29/2020 13:40
*	Version:    	1.0
*	Description:	Creates a log file and methods to write to that file.
*===============================================================*/

/*==============================================================
*	Audit Log:
*	
*
*
*===============================================================*/
using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace BarrettTorqueEm.Utilities {
    public static class LogHandler {
        private static FileStream fs;
        private static string logPath = Application.persistentDataPath + "/log";

        /// <summary>
        /// Format messages and write them to the log file.
        /// <param name="Level">Log severarity <see cref="LogLevel.cs"></param>
        /// <param name="Sender">Object that is sending the log message.</param>
        /// <param name="Message">Object that is sending the log message.</param>
        /// </summary>
        public static void LogMessage(LogLevel Level, object Sender, string Message) {
            byte[] info = Encoding.UTF8.GetBytes($"[{Level.ToString()}] ({Sender.ToString()} - {DateTime.Now.ToString("HH:mm:ss")}): {Message} \n");
            fs.Write(info, 0, info.Length);
        }

        /// <summary>
        /// Format messages and write them to the log file.
        /// <param name="Level">Log severarity <see cref="LogLevel.cs"></param>
        /// <param name="Sender">Object that is sending the log message.</param>
        /// <param name="Message">Object that is sending the log message.</param>
        /// </summary>
        public static void LogMessage(LogLevel Level, string Sender, string Message) {

            byte[] info = Encoding.UTF8.GetBytes($"[{Level.ToString()}] ({Sender} - {DateTime.Now.ToString("HH:mm:ss")}): {Message} \n");
            fs.Write(info, 0, info.Length);
        }

        /// <summary>
        /// Checks perscistant data path for a "/log" directory if exists creates file 
        /// </summary>
        public static void CreateLog() {
            Debug.Log("Creating log file at " + logPath);

            bool exists = Directory.Exists(logPath);

            if (!exists) {
                Directory.CreateDirectory(logPath);
            }

            fs = File.Create($"{logPath}/{Application.productName}_{DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss")}.log");

            LogMessage(LogLevel.Info, "LogHandler.cs", "Product Version: " + Application.version);
            LogMessage(LogLevel.Info, "LogHandler.cs", "Unity Version: " + Application.unityVersion);
            LogMessage(LogLevel.Info, "LogHandler.cs", "Genuine: " + Application.genuine.ToString());
            LogMessage(LogLevel.Info, "LogHandler.cs", "Platform: " + Application.platform.ToString());
        }

        ///<summary>
        ///Closes the log file.
        ///</summary>
        public static void Close() {
            fs.Close();
        }
    }
}