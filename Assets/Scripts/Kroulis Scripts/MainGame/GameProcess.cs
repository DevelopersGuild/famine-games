using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace Kroulis.Components
{

    public class GameProcess : NetworkBehaviour
    {

        [SyncVar]
        public float timestamp;
        [SyncVar]
        public bool GameStarted;


        private float StartGameCountDown=20f;
        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            StartGameChecking();
        }

        void StartGameChecking()
        {
            if (!isServer || GameStarted)
                return;
            if(Application.platform==RuntimePlatform.WindowsEditor)
            {
                StartGameCountDown = 0f;
                CmdStartGame();
            }
            else
            {
                if(StartGameCountDown==20f)
                {
                    ContestInfomation[] player_list = GameObject.FindObjectsOfType<ContestInfomation>();
                    if(player_list.Length>1)
                    {
                        StartGameCountDown = 15f;
                    }
                }
                else
                {
                    StartGameCountDown -= Time.deltaTime;
                    if(StartGameCountDown<=0)
                    {
                        CmdStartGame();
                    }
                }
                

            }
        }

        [Command]
        public void CmdStartGame()
        {
            if (!isServer || GameStarted || StartGameCountDown>0)
                return;
            GameStarted = true;
            InvokeRepeating("CmdUpdateTimestamp",1f,1f);
        }

        [Command]
        public void CmdUpdateTimestamp()
        {
            if (!isServer || !GameStarted)
                return;
            timestamp += 1f;
            if(timestamp >= 900)
            {
                CancelInvoke("CmdUpdateTimestamp");
                CmdTimeUp();
            }
            if(timestamp % 5 >=4)
            {
                CmdCheckGameStatus();
            }
        }

        [Command]
        public void CmdCheckGameStatus()
        {
            if (!isServer)
                return;
            Point[] point_list = GameObject.FindObjectsOfType<Point>();
            if (point_list.Length == 0)
                return;
            for (int i = 0; i < point_list.Length; i++)
            {
                if (point_list[i].points >= 100)
                {
                    CancelInvoke("CmdUpdateTimestamp");
                    Debug.Log("Winner is:" + point_list[i].GetComponent<ContestInfomation>().player_name);
                    GameStarted = false;
                    return;
                }
            }
        }

        [Command]
        public void CmdTimeUp()
        {
            if(!isServer)
                return;
            Point[] point_list=GameObject.FindObjectsOfType<Point>();
            if (point_list.Length == 0)
                return;
            Globe.errorid = "0";
            int winner = 0;
            int winner_pts = point_list[0].points;
            for (int i = 1; i < point_list.Length; i++)
            {
                if (winner_pts < point_list[i].points)
                {
                    winner = i;
                    winner_pts = point_list[i].points;
                }
            }
            Debug.Log("Winner is:" + point_list[winner].GetComponent<ContestInfomation>().player_name);
            GameStarted = false;
        }
    }
}