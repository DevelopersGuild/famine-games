using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking.Match;
using System.Collections.Generic;

namespace Kroulis.UI.Launcher
{
    public class Match_Panel_Control : MonoBehaviour
    {
        private Matches_Roomlist rooms;
        private HostData[] data;
        private List<MatchDesc> matchlist;
        private int datanum=0;
        private int pages=0;
        private int current_pages = 0;
        private int current_id = 0;
        private bool newnetwork = false;
        // Use this for initialization
        void Start()
        {
            rooms = GetComponentInChildren<Matches_Roomlist>();
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.PageDown))
            {
                NextPage();
            }
            else if(Input.GetKeyDown(KeyCode.PageUp))
            {
                PreviousPage();
            }
        }

        public void UpdateRooms(HostData[] data)
        {
            newnetwork = false;
            if (!rooms)
                return;
            this.data = data;
            datanum = data.Length;
            current_pages = 0;
            //Debug.Log(datanum);
            if(datanum%13==0)
            {
                pages = datanum / 13;
            }
            else
            {
                pages = datanum / 13 + 1;
            }
            rooms.Select.SetAllTogglesOff();
            for(int i=0;i<(datanum<=13?datanum:13);i++)
            {
                rooms.ID[i].text = data[i].guid;
                rooms.Name[i].text = data[i].gameName;
                rooms.Enrolled[i].text = data[i].connectedPlayers.ToString();
                rooms.Capicity[i].text = "4";
                rooms.Psw[i].text = data[i].passwordProtected ? "Yes" : "No";
            }
            if(datanum<13)
            {
                for(int i=datanum;i<13;i++)
                {
                    rooms.ID[i].text = rooms.Name[i].text = rooms.Enrolled[i].text = rooms.Capicity[i].text = rooms.Psw[i].text="";
                }
            }
            if(datanum>0)
            {
                rooms.Select.gameObject.SetActive(true);
                rooms.GetComponentInChildren<MatchesToggleControl>().SetShowItems(datanum > 13 ? 13 : datanum);
            }
            else
            {
                rooms.Select.gameObject.SetActive(false);
            }
                
        }
        public void UpdateRooms(List<MatchDesc> md)
        {
            newnetwork = true;
            if (!rooms)
                return;
            matchlist = md;
            datanum = matchlist.Count;
            //Debug.Log("Match numbers: "+datanum.ToString());
            current_pages = 0;
            //Debug.Log(datanum);
            if (datanum % 13 == 0)
            {
                pages = datanum / 13;
            }
            else
            {
                pages = datanum / 13 + 1;
            }
            rooms.Select.SetAllTogglesOff();
            for (int i = 0; i < (datanum <= 13 ? datanum : 13); i++)
            {
                rooms.ID[i].text = matchlist[i].hostNodeId.ToString();
                rooms.Name[i].text = matchlist[i].name;
                rooms.Enrolled[i].text = matchlist[i].currentSize.ToString();
                rooms.Capicity[i].text = matchlist[i].maxSize.ToString();
                rooms.Psw[i].text = matchlist[i].isPrivate ? "Yes" : "No";
            }
            if (datanum < 13)
            {
                for (int i = datanum; i < 13; i++)
                {
                    rooms.ID[i].text = rooms.Name[i].text = rooms.Enrolled[i].text = rooms.Capicity[i].text = rooms.Psw[i].text = "";
                }
            }
            if (datanum > 0)
            {
                rooms.Select.gameObject.SetActive(true);
                rooms.GetComponentInChildren<MatchesToggleControl>().SetShowItems(datanum > 13 ? 13 : datanum);
            }
            else
            {
                rooms.Select.gameObject.SetActive(false);
            }

        }
        public void CleanRoomInfo()
        {
            Start();
            for (int i = 0; i < 13; i++)
            {
                rooms.ID[i].text = rooms.Name[i].text = rooms.Psw[i].text = rooms.Enrolled[i].text = rooms.Capicity[i].text = "";
            }
            rooms.Select.SetAllTogglesOff();
            rooms.Select.gameObject.SetActive(false);
            rooms.CurrentID = -1;
            current_id = -1;
            pages = 0;
            current_pages = 0;
        }

        public void NextPage()
        {
            if (current_pages >= pages-1)
                return;
            rooms.Select.SetAllTogglesOff();
            current_id = -1;
            current_pages++;
            if(current_pages==pages-1)
            {
                int process = datanum % 13;
                if (process == 0)
                    process = 13;
                for(int i=0;i<process;i++)
                {
                    if (newnetwork)
                    {
                        rooms.ID[i].text = matchlist[current_pages * 13 + i].hostNodeId.ToString();
                        rooms.Name[i].text = matchlist[current_pages * 13 + i].name;
                        rooms.Enrolled[i].text = matchlist[current_pages * 13 + i].currentSize.ToString();
                        rooms.Capicity[i].text = matchlist[current_pages * 13 + i].maxSize.ToString();
                        rooms.Psw[i].text = matchlist[current_pages * 13 + i].isPrivate ? "Yes" : "No";
                    }
                    else
                    {
                        rooms.ID[i].text = data[current_pages * 13 + i].guid;
                        rooms.Name[i].text = data[current_pages * 13 + i].gameName;
                        rooms.Enrolled[i].text = data[current_pages * 13 + i].connectedPlayers.ToString();
                        rooms.Capicity[i].text = "4";
                        rooms.Psw[i].text = data[current_pages * 13 + i].passwordProtected ? "Yes" : "No";
                    }
                }
            }
            else
            {
                for(int i=0;i<13;i++)
                {
                    if (newnetwork)
                    {
                        rooms.ID[i].text = matchlist[current_pages * 13 + i].hostNodeId.ToString();
                        rooms.Name[i].text = matchlist[current_pages * 13 + i].name;
                        rooms.Enrolled[i].text = matchlist[current_pages * 13 + i].currentSize.ToString();
                        rooms.Capicity[i].text = matchlist[current_pages * 13 + i].maxSize.ToString();
                        rooms.Psw[i].text = matchlist[current_pages * 13 + i].isPrivate ? "Yes" : "No";
                    }
                    else
                    {
                        rooms.ID[i].text = data[current_pages * 13 + i].guid;
                        rooms.Name[i].text = data[current_pages * 13 + i].gameName;
                        rooms.Enrolled[i].text = data[current_pages * 13 + i].connectedPlayers.ToString();
                        rooms.Capicity[i].text = "4";
                        rooms.Psw[i].text = data[current_pages * 13 + i].passwordProtected ? "Yes" : "No";
                    }
                   }
            }

        }

        public void PreviousPage()
        {
            if (current_pages <= 0)
                return;
            rooms.Select.SetAllTogglesOff();
            current_id = -1;
            current_pages--;
            for (int i = 0; i < 13; i++)
            {
                if (newnetwork)
                {
                    rooms.ID[i].text = matchlist[current_pages * 13 + i].hostNodeId.ToString();
                    rooms.Name[i].text = matchlist[current_pages * 13 + i].name;
                    rooms.Enrolled[i].text = matchlist[current_pages * 13 + i].currentSize.ToString();
                    rooms.Capicity[i].text = matchlist[current_pages * 13 + i].maxSize.ToString();
                    rooms.Psw[i].text = matchlist[current_pages * 13 + i].isPrivate ? "Yes" : "No";
                }
                else
                {
                    rooms.ID[i].text = data[current_pages * 13 + i].guid;
                    rooms.Name[i].text = data[current_pages * 13 + i].gameName;
                    rooms.Enrolled[i].text = data[current_pages * 13 + i].connectedPlayers.ToString();
                    rooms.Capicity[i].text = "4";
                    rooms.Psw[i].text = data[current_pages * 13 + i].passwordProtected ? "Yes" : "No";
                }
            }
        }

        private void UpdateInformation_R()
        {
            Logic_MatchOperation lmo = GameObject.Find("Logic_Network").GetComponentInChildren<Logic_MatchOperation>();
            if(lmo.UsingNewNetworkSystem)
            {
                lmo.GetComponent<Logic_UNetConfig>().GetHosts();
            }
            else
            {
                lmo.GetComponent<Logic_MasterServerConf>().GetHosts();
            }
            GetComponentInParent<UI_FunctionControl>().StartWWWLoading();

        }
        public void StartUpdating()
        {
            InvokeRepeating("UpdateInformation_R",0f,30f);
        }
        public void StopUpdating()
        {
            CancelInvoke("UpdateInformation_R");
        }

        public void Join()
        {
            if(rooms.CurrentID==-1)
            {
                Debug.Log("Custom Join Unfinished.");
                return;
            }
            current_id = current_pages * 13 + rooms.CurrentID;
            if(current_id<datanum)
            {
                Debug.Log("Host data ID:" + current_id);
                Logic_MatchOperation lmo = GameObject.Find("Logic_Network").GetComponentInChildren<Logic_MatchOperation>();
                if(lmo.IsUsingNewNetworkSystem())
                {
                    lmo.JoinRoom(matchlist[current_id]);
                }
                else
                {
                    lmo.JoinRoom(data[current_id]);
                }
            }
            else
            {
                Debug.LogError("Select room out of range.");
            }
        }
    }
}

