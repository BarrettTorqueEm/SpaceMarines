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

namespace BarrettTorqueEm.Utilities {
    public static class LogHandler {
        private static FileStream fs;
        private static string logPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/TeamTorqueEmTech/log";

        /// <summary>
        /// Format messages and write them to the log file.
        /// <param name="Level">Log severarity <see cref="LogLevel.cs"></param>
        /// <param name="Sender">Object that is sending the log message.</param>
        /// <param name="Message">Object that is sending the log message.</param>
        /// </summary>
        public static void LogMessage(LogLevel Level, object Sender, string Message) {
            if (fs == null) {
                Console.WriteLine("Log file not created please call LogHandler.CreateLog()");
                return;
            }
            string msg = $"[{Level.ToString()}] ({Sender.ToString()} - {DateTime.Now.ToString("HH:mm:ss")}): {Message} \n";
            Console.WriteLine(msg);
            byte[] info = Encoding.UTF8.GetBytes(msg);
            fs.Write(info, 0, info.Length);
        }

        /// <summary>
        /// Format messages and write them to the log file.
        /// <param name="Level">Log severarity <see cref="LogLevel.cs"></param>
        /// <param name="Sender">Object that is sending the log message.</param>
        /// <param name="Message">Object that is sending the log message.</param>
        /// </summary>
        public static void LogMessage(LogLevel Level, string Sender, string Message) {
            if (fs == null) {
                Console.WriteLine("Log file not created please call LogHandler.CreateLog()");
                return;
            }
            string msg = $"[{Level.ToString()}] ({Sender} - {DateTime.Now.ToString("HH:mm:ss")}): {Message}\n";
            Console.WriteLine(msg);
            byte[] info = Encoding.UTF8.GetBytes(msg);
            fs.Write(info, 0, info.Length);
        }

        /// <summary>
        /// Checks perscistant data path for a "/log" directory if exists creates file 
        /// </summary>
        public static void CreateLog() {

            Console.WriteLine("Creating log file at " + logPath);

            if (!Directory.Exists(logPath)) {
                Directory.CreateDirectory(logPath);
            }

            fs = File.Create($"{logPath}/SM5_Server_TEST_{DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss")}.log");
        }

        ///<summary>
        ///Closes the log file.
        ///</summary>
        public static void Close() {
            fs.Close();
        }
    }
}