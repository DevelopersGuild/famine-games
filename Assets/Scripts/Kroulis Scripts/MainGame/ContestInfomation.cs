using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace Kroulis.Components
{
    public class ContestInfomation : NetworkBehaviour
    {
        [SyncVar]
        public string player_name;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        [Command]
        public void CmdUpdatePlayerName(string playername)
        {
            if (!isServer)
                return;
            player_name = playername;
        }

    }
}