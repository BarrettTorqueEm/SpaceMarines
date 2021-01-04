/*===============================================================
*Product:    SM5_Client
*Developer:  Michael Girard (mgirard989@outlook.com)
*Company:    TeamTorqueEmTech
*Date:       12/29/2020 19:51
*Version:    0.1
*Description: Commands and lodgic to be used while in the UI
*================================================================*/
/*===============================================================
*   AUDIT LOG
*
*
*================================================================*/
using UnityEngine;
using SimpleTcp;
using BarrettTorqueEm.Utilities;
using System;
using SM5_Client.Net;

namespace SM5_Client.Utilities {
    public class UIManager : MonoBehaviour {
        public static UIManager instance;
        public GameObject JoiningImage;

        private void Awake() {
            if (instance == null)
                instance = this;
        }

        public void Quit() {
            MenuTools.Quit();
        }

        public void Join(int port) {
            MenuTools.ChangeLevel(1);
        }
    }
}