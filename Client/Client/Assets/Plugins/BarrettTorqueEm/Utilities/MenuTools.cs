/*===============================================================
*	Project:    	Libary
*	Developer:  	Michael Girard (mgirard989@outlook.com)
*	Company:    	TeamTorqueEmTech
*	Creation Date:	12/29/2020 13:40
*	Version:    	1.3
*	Description:	Tools to be used for menu control
*===============================================================*/

/*==============================================================
*	Audit Log:
*	
*
*
*===============================================================*/
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BarrettTorqueEm.Utilities {
    public static class MenuTools {
        public static void Quit() {
            LogHandler.LogMessage(LogLevel.Info, "MenuTools.cs", "Closing game safely.");
            LogHandler.Close();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public static void ChangeLevel(int BuildIndex) {
            LogHandler.LogMessage(LogLevel.Info, "MenuTools.cs", $"Changing level to build index of {BuildIndex}");
            //TODO: Change to loading scene and load.
            SceneManager.LoadScene(BuildIndex);
        }

        public static void Help() {
            LogHandler.LogMessage(LogLevel.Error, "MenuTools.cs", "Help() not implemented yet.");
            throw new NotImplementedException("Sorry this method is not implemented yet. Go yell at me!");
        }
    }
}
