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
using UnityEngine;
using BarrettTorqueEm.Utilities;
using UnityEngine.SceneManagement;

namespace SM5_Client {
    public class SystemManager : MonoBehaviour {
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

        public void ChangeLevel(int BuildIndex) {
            //FIXME: Scene name not printing correctly?
            LogHandler.LogMessage(LogLevel.Info, this, $"Changing level from {SceneManager.GetActiveScene().name} to {BuildIndex} {SceneManager.GetSceneByBuildIndex(BuildIndex).name}");
            SceneManager.LoadScene(BuildIndex);
        }

        private void OnApplicationQuit() {
            LogHandler.Close();
        }
    }
}