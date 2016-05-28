using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using Kroulis.UI.MainGame;

namespace Kroulis.Components
{

    public class GameProcess : NetworkBehaviour
    {

        [SyncVar]
        public float timestamp;
        [SyncVar]
        public bool GameStarted;

        private string log="";

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
                    CmdStartGameCDShowTips(StartGameCountDown);
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
                    log += "[" + timestamp + "]=> Winner is: " + point_list[i].GetComponent<ContestInfomation>().player_name;
                    Debug.Log("Winner is:" + point_list[i].GetComponent<ContestInfomation>().player_name);
                    GetComponent<Logic_ResultUpload>().UploadResult();
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
            log += "[TimeUp]=> Winner is: " + point_list[winner].GetComponent<ContestInfomation>().player_name;
            Debug.Log("Winner is: " + point_list[winner].GetComponent<ContestInfomation>().player_name);
            GetComponent<Logic_ResultUpload>().UploadResult();
            GameStarted = false;
        }

        [Command]
        public void CmdAddingKillingTab(string name1, string name2, int type)
        {
            if (!isServer)
                return;
            log += "[" + timestamp + "] => Player " + name1 + "used ";
            switch (type)
            {
                case 1:
                    log += "Melee Weapon";
                    break;
                case 2:
                    log += "Ranged Weapon";
                    break;
                default:
                    log += "Unknown Weapon";
                    break;
            }
            log += " Killed " + name2 + ".\n";
            RpcAddingKillingTab(name1, name2, type);
        }

        [ClientRpc]
        public void RpcAddingKillingTab(string name1,string name2 ,int type)
        {
            if(!GameStarted)
                return;
            MainPanelFullControl mainui = GameObject.Find("Main_UI").GetComponentInChildren<MainPanelFullControl>();
            if(mainui)
            {
                mainui.AddKillingTab(type, name1, name2);
            }
        }

        [Server]
        public string GetLog()
        {
            return log;
        }

        [Command]
        public void CmdStartGameCDShowTips(float times)
        {
            if (!isServer)
                return;
            if(times<0)
                return;
        }

        [ClientRpc]
        public void RpcShowStartGameTips(string tips)
        {
            MainPanelFullControl mpfc = GameObject.Find("Main_UI").GetComponentInChildren<MainPanelFullControl>();
            if(mpfc)
            {
                mpfc.UpdateTips(tips);
            }
        }
    }
}