using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using System.Collections.Generic;

public class Logic_MatchOperation : MonoBehaviour
{
    public bool ShowGUI = true;
    public bool UsingNewNetworkSystem = false;
    private Logic_MasterServerConf lmc;
    private Logic_UNetConfig luc;
    private HostData[] hosts;
    private List<MatchDesc> matchlist;
    private NetworkConnection current_connection;
    private Logic_Chat lc;
    private NetworkManager nm;

    void Start()
    {
        lc = GetComponentInChildren<Logic_Chat>();
    }

    void Awake()
    {
        lmc = GetComponent<Logic_MasterServerConf>();
        if (!lmc)
        {
            lmc = gameObject.AddComponent<Logic_MasterServerConf>();
        }
        luc = GetComponent<Logic_UNetConfig>();
        if(!luc)
        {
            luc = gameObject.AddComponent<Logic_UNetConfig>();
        }
        nm = GetComponentInParent<NetworkManager>();

    }


    void OnGUI()
    {
        if (ShowGUI)
        {
            if (UsingNewNetworkSystem)
            {
                if(!luc.GetHostStatus())
                {
                    if (!lmc.GetConnectStatus())
                    {
                        if (GUILayout.Button("Create Room"))
                        {
                            luc.CreateRoom("TestRoom", "This is a test room for this project");
                        }

                        if (GUILayout.Button("List rooms"))
                        {
                            luc.GetHosts(true);
                        }

                        if (matchlist == null || matchlist.Count == 0)
                        {
                            GUILayout.Label("No Room.");
                        }
                        else
                        {
                            foreach (MatchDesc md in matchlist)
                            {
                                if (GUILayout.Button("[" + md.currentSize + @"/4] " + md.name))
                                {
                                    luc.JoinRoom(md);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (GUILayout.Button("Remove Host"))
                    {
                        luc.UnHost();
                    }
                    
                    /*int length = Network.connections.Length;
                    GUILayout.Label("Connections: X" + length);
                    for (int i = 0; i < length; i++)
                    {
                        GUILayout.Label("Cilent" + (i + 1));
                        GUILayout.Label("Cilent IP: " + Network.connections[i].ipAddress);
                        GUILayout.Label("Cilent Port: " + Network.connections[i].port);
                    }*/
                }
            }
            else
            {
                if (!lmc.GetHostStatus())
                {
                    if (!lmc.GetConnectStatus())
                    {
                        if (GUILayout.Button("Create Room"))
                        {
                            lmc.CreateRoom("TestRoom", "This is a test room for this project");
                        }

                        if (GUILayout.Button("List rooms"))
                        {
                            lmc.GetHosts(true);
                        }

                        if (hosts == null || hosts.Length == 0)
                        {
                            GUILayout.Label("No Room.");
                        }
                        else
                        {
                            foreach (HostData h in hosts)
                            {
                                if (GUILayout.Button("[" + h.connectedPlayers + @"/4] " + h.gameName + " " + h.comment))
                                {
                                    lmc.JoinRoom(h);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("Disconnect"))
                        {
                            lmc.Disconnect();
                        }
                    }
                }
                else
                {
                    if (GUILayout.Button("Remove Host"))
                    {
                        lmc.UnHost();
                    }
                    int length = Network.connections.Length;
                    GUILayout.Label("Connections: X" + length);
                    for (int i = 0; i < length; i++)
                    {
                        GUILayout.Label("Cilent" + (i + 1));
                        GUILayout.Label("Cilent IP: " + Network.connections[i].ipAddress);
                        GUILayout.Label("Cilent Port: " + Network.connections[i].port);
                    }

                }
            }
        }
    }

   void Update()
    {
       if(ShowGUI)
       {
           if (UsingNewNetworkSystem)
           {
               if(luc.GetHostStatus() || luc.GetConnectStatus())
               {
                   GetComponentInParent<NetworkManagerHUD>().showGUI = true;
               }
               else
               {
                   GetComponentInParent<NetworkManagerHUD>().showGUI = false;
               }
           }
           else
           {
               if (lmc.GetHostStatus() || lmc.GetConnectStatus())
               {
                   GetComponentInParent<NetworkManagerHUD>().showGUI = true;
               }
               else
               {
                   GetComponentInParent<NetworkManagerHUD>().showGUI = false;
               }
           }
       }
       if(!nm.isNetworkActive)
       {
           lc.gameObject.SetActive(true);
       }
    }

    void OnConnected(NetworkMessage msg)
    {
        current_connection = msg.conn;
        Debug.Log("Connected!");
    }

    void OnDisconnectedFromServer(NetworkDisconnection dis)
    {
        if(!UsingNewNetworkSystem)
        {
            lmc.SetDisconnect();
        }
        
        Debug.Log("Disconnected!");
    }

    public void UpdateHostData(HostData[] hd)
    {
        hosts = hd;
    }

    public void UpdateMatchData(List<MatchDesc> md)
    {
        matchlist = md;
    }

    public void StartMatching()
    {
        if(!UsingNewNetworkSystem)
        {
            lmc.StartMatching();
        }
        else
        {
            luc.StartMatching();
        }
    }

    public void InRoomCheck()
    {
        if(!UsingNewNetworkSystem)
        {
            lmc.InRoomCheck();
        }
        else
        {
            luc.InRoomCheck();
        }
    }

    public void QuitMathces()
    {
        if(!UsingNewNetworkSystem)
        {
            lmc.Disconnect();
        }
        else
        {
            luc.Disconnect();
        }
        Globe.roomid = "";
        
    }

    public void StartGame(string ip="")
    {
        if(!UsingNewNetworkSystem)
        {
            if(lmc.GetHostStatus())
            {
                //if(NetworkServer.Listen(4444))
                nm.StartHost();
                {
                    lc.SetChatBox(null);
                    Globe.gamestarted = true;
                    lc.SendCilentStartGameInfo();
                }

            }
            else
            {
                nm.networkAddress=ip;
                lc.SetChatBox(null);
                nm.StartClient();
            }
        }
    }
    public void JoinRoom(HostData hd)
    {
        lmc.JoinRoom(hd);
    }

    public void JoinRoom(MatchDesc md)
    {
        luc.JoinRoom(md);
    }

    public void CreateRoom()
    {
        Logic_LauncherGetInfo info = GetComponent<Logic_LauncherGetInfo>();
        if(UsingNewNetworkSystem)
        {
            luc.CreateRoom(info.GetCharacterNameA() + "'s Room", "Custom Created.");
        }
        else
        {
            lmc.CreateRoom(info.GetCharacterNameA() + "'s Room", "Custom Created.");
        }
    }

    public void CreateRoom(string Name,string Comment,bool psw_protected=false,string psw="")
    {
        if(UsingNewNetworkSystem)
        {
            luc.CreateRoom(name, Comment, psw_protected, psw);
        }
        else
        {
            lmc.CreateRoom(name, Comment, psw_protected, psw);
        }
    }

    public bool IsUsingNewNetworkSystem()
    {
        return UsingNewNetworkSystem;
    }
}
