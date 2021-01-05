/*===============================================================
*Project:    SM5_Client
*Developer:  Michael Girard (mgirard989@outlook.com)
*Company:    TeamTorqueEmTech
*Date:       01/03/2021 22:18
*Version:    0.1
*Description: 
*================================================================*/
/*===============================================================
*   AUDIT LOG
*
*
*================================================================*/
using System.Collections.Generic;
using BarrettTorqueEm.Utilities;
using SM5_Client.Entities;
using SM5_Client.Net;
using System.Collections;
using UnityEngine;

namespace SM5_Client.Utilities {
    public class LobbyManager : MonoBehaviour {
        public static LobbyManager instance;
        public List<Player> ListOfPlayers = new List<Player>();

        [SerializeField] private GameObject PlayerPrefab;

        private void Awake() {
            if (instance == null)
                instance = this;

        }
        private void Start() {
            GameObject go = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);

            if (NetworkManager.instance.client.IsConnected)
                go.GetComponent<Player>().client = new Client(NetworkManager.instance.client);
            else
                LogHandler.LogMessage(LogLevel.Warning, this, "Client not connected");

            go.name = go.GetComponent<Player>().client.UName;

            NetworkManager.instance.SendWelcome(go);
        }
    }
}