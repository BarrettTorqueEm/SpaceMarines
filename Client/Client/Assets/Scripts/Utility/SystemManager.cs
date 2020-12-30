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

namespace SM5_Client {
    public class SystemManager : MonoBehaviour {
        ///<summary>
        ///Entire system init jobs
        ///</summary>
        private void Awake() {
            LogHandler.CreateLog();
        }
        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                LogHandler.LogMessage(LogLevel.Debug, this.name, "Test log");
            }
        }
    }
}