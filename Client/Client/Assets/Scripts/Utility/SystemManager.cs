/*===============================================================
*Product:    SM5_Client
*Developer:  Michael Girard (mgirard989@outlook.com)
*Company:    TeamTorqueEmTech
*Date:       12/29/2020 19:56
*Version:    0.1
*Description: Manages jobs for entire system such as logging, error handling, etc
*================================================================*/
/*===============================================================
*   AUDIT LOG
*
*
*================================================================*/
using BarrettTorqueEm.Utilities;
using SimpleTcp;
using SM5_Client.Net;
using System;
using UnityEngine;

namespace SM5_Client.Utilities {
    public class SystemManager : MonoBehaviour {
        public static string IP = "38.27.130.133";
        public static int PRODServer = 65341;

        public static SystemManager instance;
        ///<summary>
        ///Entire system init jobs
        ///</summary>
        private void Awake() {
            LogHandler.CreateLog();

            if (instance == null) {
                instance = this;
            }
            DontDestroyOnLoad(this);
        }

        private void OnApplicationQuit() {
            LogHandler.Close();
        }
    }
}