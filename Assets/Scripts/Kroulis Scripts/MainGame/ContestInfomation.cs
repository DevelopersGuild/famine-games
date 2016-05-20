using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace Kroulis.Components
{
    public class ContestInfomation : NetworkBehaviour
    {
        [SyncVar]
        public string player_name;

        private string pid = "";

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        [Command]
        public void CmdUpdatePlayerName(string playername,string pid)
        {
            if (!isServer)
                return;
            player_name = playername;
            this.pid = pid;
        }

        [Server]
        public string GetPid()
        {
            return pid;
        }
    }
}