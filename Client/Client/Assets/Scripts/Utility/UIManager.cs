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
using BarrettTorqueEm.Utilities;
using SimpleTcp;
using SM5_Client.Net;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SM5_Client.Utilities {
    public class UIManager : MonoBehaviour {
        public static UIManager instance;
        public GameObject JoiningImage;

        private void Awake() {
            if (instance == null)
                instance = this;

            DontDestroyOnLoad(this.gameObject);
        }

        public static void ChangeLevel(int BuildIndex) {
            SceneManager.sceneLoaded += OnSceneLoaded;
            MenuTools.ChangeLevel(BuildIndex);
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            if (scene.buildIndex == 1)
                NetworkManager.instance.Join();
        }

        public static void Quit() {
            MenuTools.Quit();
        }
    }
}