using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.NetworkSystem;
using System.Collections.Generic;
using Kroulis.UI.Launcher;
using UnityEngine.SceneManagement;

public class Logic_UNetConfig : MonoBehaviour {

    private List<MatchDesc> matchList = new List<MatchDesc>();
    private bool matchCreated=false;
    private NetworkMatch networkMatch;
    private MatchInfo minfo;
    private bool GuiMode = false;
    private bool isConnected = false;
    private bool matching = false;
    private bool inroomcheck = false;
    private List<UserInfoMessage> uimlist; 
    void Awake()
    {
        networkMatch = gameObject.AddComponent<NetworkMatch>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CreateRoom(string room_name,string comment,bool psw_protected=false, string password="")
    {
        CreateMatchRequest create = new CreateMatchRequest();
        uimlist=new List<UserInfoMessage>();
        create.name = room_name;
        create.size = 4;
        create.advertise = true;
        if (psw_protected)
            create.password = password;
        else
            create.password = "";
        networkMatch.CreateMatch(create, OnMatchCreate);
    }
    public void JoinRoom(MatchDesc md)
    {
        Globe.roomid = md.hostNodeId.ToString();
        networkMatch.JoinMatch(md.networkId, "", OnMatchJoined);
    }

    public void JoinRoom(MatchDesc md, string password)
    {
        Globe.roomid = md.hostNodeId.ToString();
        networkMatch.JoinMatch(md.networkId, password, OnMatchJoined);
    }

    public bool GetHostStatus()
     {
         return matchCreated;
     }
    public bool GetConnectStatus()
     {
         return isConnected;
     }
    public void GetHosts()
     {
         networkMatch.ListMatches(0,100,"",OnMatchList);
     }
    public void GetHosts(bool gui)
     {
         GuiMode = gui;
         networkMatch.ListMatches(0, 100, "", OnMatchList);
     }

    public void UnHost()
     {
         if(matchCreated)
         {
             networkMatch.DestroyMatch(minfo.networkId,OnMatchDestoryed);
         }    
     }
    public void Disconnect()
    {
        if(matchCreated)
        {
            UnHost();
            return;
        }
        if(isConnected)
            networkMatch.DropConnection(minfo.networkId, minfo.nodeId, OnDisconnected);
    }
    public void SetDisconnect()
    {
        isConnected = false;
    }

    public int GetConnectedPeopleNUM()
    {
        if (minfo==null)
            return 0;
        return 0;
    }
    public void StartMatching()
    {
        if (!matchCreated && !isConnected)
        {
            matching = true;
            UI_FunctionControl roots = GameObject.Find("Launcher UI Root").GetComponent<UI_FunctionControl>();
            roots.StartWWWLoading();
            GetHosts();
        }
    }
    public void InRoomCheck()
    {
        inroomcheck = true;
        GetHosts();
    }
    private void OnMatchCreate(CreateMatchResponse matchResponse)
     {
         if(matchResponse.success)
         {
             matchCreated = true;
             Utility.SetAccessTokenForNetwork(matchResponse.networkId, new NetworkAccessToken(matchResponse.accessTokenString));
             minfo = new MatchInfo(matchResponse);
             //AfterJoin();
             GetComponentInParent<NetworkManager>().StartHost(minfo);
             Globe.errorid = "EHCONQ";
         }
         else
         {
             Debug.LogError("Failed to create a match.");
             SceneManager.LoadScene("Login");
         }
     }
    private void OnMatchJoined(JoinMatchResponse matchjoin)
    {
        if(matchjoin.success)
        {
            isConnected = true;
            minfo = new MatchInfo(matchjoin);
            Utility.SetAccessTokenForNetwork(matchjoin.networkId, new NetworkAccessToken(matchjoin.accessTokenString));
            //AfterJoin();
            GetComponentInParent<NetworkManager>().StartClient(minfo);
            Globe.errorid = "CONNLST";
            //NetworkClient myClient = new NetworkClient();
            //myClient.RegisterHandler(MsgType.Connect, OnConnected);
            //myClient.Connect(minfo);
            //GetComponentInParent<NetworkManager>().client=myClient;
            
        }
        else
        {
            Debug.LogWarning("Failed to join the room.");
        }

    }

    public void RejoinMatch()
    {
        if(minfo!=null)
        {
            GetComponentInParent<NetworkManager>().StartClient(minfo);
        }
    }
    private void OnMatchList(ListMatchResponse matchListResponse)
     {
         if (matchListResponse.success && matchListResponse.matches != null)
         {
             matchList = matchListResponse.matches;
             if(GuiMode)
             {
                 GetComponentInParent<Logic_MatchOperation>().UpdateMatchData(matchListResponse.matches);
             }
             if(!matching)
             {
                 if(inroomcheck)
                 {
                     InRoomCheckR();
                     inroomcheck = false;
                     return;
                 }
                 GameObject root = GameObject.Find("Launcher UI Root");
                 if(root)
                 {
                     Match_Panel_Control mpc = root.GetComponent<UI_FunctionControl>().MatchPanel.GetComponentInChildren<Match_Panel_Control>();
                     if (mpc)
                         mpc.UpdateRooms(matchList);
                 }
                 root.GetComponent<UI_FunctionControl>().FinishWWWLoading();
             }
             else
             {
                 matching = false;
                 LogicMatchingSelect();
             }
         }
     }
    private void OnDisconnected(BasicResponse response)
    {
        if(response.success)
        {
            isConnected = false;
            GameObject root = GameObject.Find("Launcher UI Root");
            if(root)
            {
                root.GetComponent<UI_FunctionControl>().QuitRoom();
            }
        }
    }
    private void OnMatchDestoryed(BasicResponse response)
    {
        if(response.success)
        {
            matchCreated = false;
        }
    }
    private void AfterJoin()
     {
         if (Globe.gamestarted)
             return;
         if (matchCreated)
         {
             Globe.roomid = Network.player.guid;
         }
         GameObject.Find("Launcher UI Root").GetComponent<UI_FunctionControl>().EnterRoom();
     }
    private void InRoomCheckR()
    {
        foreach(MatchDesc md in matchList)
        {
            if(md.hostNodeId.ToString() == Globe.roomid)
            {
                JoinRoom(md);
                return;
            }
        }
        return;
    }

    private void OnConnected(NetworkMessage msg)
    {
        Debug.Log("Connected.");
        //SceneManager.LoadScene("Town");
    }

    private void LogicMatchingSelect()
    {
        UI_FunctionControl roots = GameObject.Find("Launcher UI Root").GetComponent<UI_FunctionControl>();
        Logic_LauncherGetInfo info = GetComponent<Logic_LauncherGetInfo>();
        if (matchList.Count == 0)
        {
            roots.FinishWWWLoading();
            CreateRoom(info.GetCharacterNameA() + "'s Room", "AutoMatches Created.");
            return;
        }
        foreach (MatchDesc md in matchList)
        {
            if (md.isPrivate)
                continue;
            if (md.currentSize >= md.maxSize)
                continue;
            roots.FinishWWWLoading();
            JoinRoom(md);
            return;
        }
        roots.FinishWWWLoading();
        CreateRoom(info.GetCharacterNameA() + "'s Room", "AutoMatches Created.");
        return;
    }


    // Start From Here Are Special Functions Called By Network
    public void SendChatMessage(ChatMessage msg)
    {
        networkMatch.SendMessage("RecievedChatMessage",msg);
    }

    public void SendStartGameInfo()
    {
        GetComponentInParent<NetworkManager>().StartServer(minfo);
        networkMatch.SendMessage("CilentStartGameMessage");
    }

    public void SendNewPlayerJoinInfo()
    {
        UserInfoMessage uim = new UserInfoMessage();
        uim.PlayerName = GetComponent<Logic_LauncherGetInfo>().GetCharacterNameA();
        uim.Ready = false;
        networkMatch.SendMessage("RecievedNewPlayerInfo", uim);
    }

    public void SendReadyInfo(string name)
    {
        networkMatch.SendMessage("RecievedReadyInfo",name);
    }
    private void RecievedChatMessage(ChatMessage msg)
    {
        Logic_Chat lc = GetComponentInChildren<Logic_Chat>();
        if(lc)
        {
            lc.MessageBoxUpdate(msg.Message, msg.PlayerName, msg.type);
        }
    }

    private void CilentStartGameMessage()
    {
        if(!Globe.gamestarted)
        {
            GetComponentInParent<NetworkManager>().client.Connect(minfo);
            Globe.gamestarted = true;
        }
        
    }

    private void RecievedReadyInfo(string name)
    {
        if(matchCreated)
        {
            foreach(UserInfoMessage uim in uimlist)
            {
                if(uim.PlayerName==name)
                {
                    UserInfoMessage newuim = uim;
                    newuim.Ready = true;
                    uimlist.Remove(uim);
                    uimlist.Add(newuim);
                    break;
                }
            }

            bool flag=true;
            for(int i=0;i<uimlist.Count && flag;i++)
            {
                if (uimlist[i].Ready == false)
                    flag = false;
            }
            if(flag)
            {
                GetComponentInChildren<Logic_Chat>().StartGameCountingToggerN();
            }
            else
            {

            }
        }
    }

    private void RecievedNewPlayerInfo(UserInfoMessage uim)
    {
        if(matchCreated)
        {
            if(Globe.gamestarted)
            {
                uim.Ready = true;
                uimlist.Add(uim);
                networkMatch.SendMessage("CilentStartGameMessage");
            }
            else
            {
                uimlist.Add(uim);
                ChatMessage msg = new ChatMessage();
                msg.PlayerName = "System";
                msg.Message = "Player " + uim.PlayerName + " joined the room.";
                msg.type = 1;
                networkMatch.SendMessage("RecievedChatMessage", msg);
            }
        }
    }


}
