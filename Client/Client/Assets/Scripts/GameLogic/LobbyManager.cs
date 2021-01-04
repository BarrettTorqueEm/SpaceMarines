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
        public List<Player> ListOfPlayers = new List<Player>();

        [SerializeField] private GameObject PlayerPrefab;

        private Player p;
        private void Start() {
            p = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
            p.client = new Client(SystemManager.IP + ":" + SystemManager.PRODServer);

            if (NetworkManager.instance.Join(p.client))
                p.client.TcpClient.Send("99999:BarrettTorqueEm");
            else {
                LogHandler.LogMessage(LogLevel.Warning, this, "Could not connect to the server.");
                MenuTools.ChangeLevel(0);
            }
        }

        private IEnumerator Test() {
            yield return new WaitUntil(() => NetworkManager.instance.Join(p.client) || !NetworkManager.instance.Join(p.client));
        }
    }
}