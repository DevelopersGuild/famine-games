using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Kroulis.UI.Launcher;
using UnityEngine.SceneManagement;

public class Logic_MasterServerConf : MonoBehaviour {

    public bool localhost = false;
    private bool isHost = false;
    private bool isConnected = false;
    private bool gui_mode = false;
    private bool matching = false;
    private bool inroomcheck = false;
	// Use this for initialization
	void Start () {
        if(!localhost)
        {
            MasterServer.ipAddress = "107.170.226.18";
            MasterServer.port = 23466;
            Network.natFacilitatorIP = "107.170.226.18";
            Network.natFacilitatorPort = 50005;
        }
        else
        {
            MasterServer.ipAddress = "127.0.0.1";
            MasterServer.port = 23466;
            Network.natFacilitatorIP = "127.0.0.1";
            Network.natFacilitatorPort = 50005;
        }
        
	}
	
    public void GetHosts(bool show_gui)
    {
        MasterServer.ClearHostList();
        gui_mode = show_gui;
        MasterServer.RequestHostList("Survive");
    }
    public void GetHosts()
    {
        MasterServer.ClearHostList();
        MasterServer.RequestHostList("Survive");
    }

    public void CreateRoom(string room_name,string comment,bool psw_protected=false, string password="")
    {
        if(psw_protected)
            Network.incomingPassword = password;
        bool useNat = !Network.HavePublicAddress();
        Network.InitializeServer(32, 25000, useNat);
        MasterServer.RegisterHost("Survive", room_name, comment);
    }

    public void JoinRoom(HostData hd)
    {
        if(Network.Connect(hd)==NetworkConnectionError.NoError)
            isConnected = true;
        if(isConnected)
        {
            Globe.roomid = hd.guid;
            AfterJoinWork();
        }
    }

    public void JoinRoom(HostData hd, string password)
    {
        if (Network.Connect(hd, password) == NetworkConnectionError.NoError)
        {
            isConnected = true;
        }
        if (isConnected)
        {
            AfterJoinWork();
        }
    }

    void OnMasterServerEvent(MasterServerEvent e)
    {
        if(e==MasterServerEvent.RegistrationFailedNoServer)
        {
            Debug.LogError("Cannot Find Master Server");
            SceneManager.LoadScene("Login");
        }
        else if(e==MasterServerEvent.RegistrationSucceeded)
        {
            isHost = true;
            Debug.Log("Host Successfully registered.");
            AfterJoinWork();
        }
        else if(e==MasterServerEvent.HostListReceived)
        {
            if(gui_mode)
            {
                GetComponent<Logic_MatchOperation>().UpdateHostData(MasterServer.PollHostList());
            }
            if(!matching)
            {
                if(inroomcheck)
                {
                    InRoomCheckR();
                }
                else
                {
                    UI_FunctionControl roots = GameObject.Find("Launcher UI Root").GetComponent<UI_FunctionControl>();
                    roots.MatchPanel.GetComponent<UI_MatchesControl>().UI.GetComponent<Match_Panel_Control>().UpdateRooms(MasterServer.PollHostList());
                    roots.FinishWWWLoading();
                }     
            }
            else
            {
                matching = false;
                LogicMatchingSelect();
            }
            
        }
        else
        {
            Debug.Log("An Unknown Message was sent from the Master Server");
        }
    }

    void OnFailedToConnect(NetworkConnectionError e)
    {
        if(e == NetworkConnectionError.NoError)
        {
            Debug.Log("Login Success.");
        }
        else if(e == NetworkConnectionError.ConnectionFailed)
        {
            Debug.LogError("Failed To Connect.");
        }
        else if(e == NetworkConnectionError.InvalidPassword)
        {
            Debug.LogError("Password Error.");
        }
        else
        {
            Debug.LogError("Unknown Error occur when connect to the host.");
        }
    }

    public void UnHost()
    {
        if(isHost)
        {
            Network.Disconnect();
            MasterServer.UnregisterHost();
            isHost = false;
        }
    }

    public bool GetHostStatus()
    {
        return isHost;
    }
    public bool GetConnectStatus()
    {
        return isConnected;
    }
    public void Disconnect()
    {
        Network.Disconnect();
        isConnected = false;
    }

    public void SetDisconnect()
    {
        isHost = false;
        isConnected = false;
    }

    public void StartMatching()
    {
        if(!isHost && !isConnected)
        {
            matching = true;
            UI_FunctionControl roots = GameObject.Find("Launcher UI Root").GetComponent<UI_FunctionControl>();
            roots.StartWWWLoading();
            GetHosts();
        }
    }

    private void LogicMatchingSelect()
    {
        HostData[] current_list = MasterServer.PollHostList();
        UI_FunctionControl roots = GameObject.Find("Launcher UI Root").GetComponent<UI_FunctionControl>();
        Logic_LauncherGetInfo info = GetComponent<Logic_LauncherGetInfo>();
        if(current_list.Length==0)
        {
            roots.FinishWWWLoading();
            CreateRoom(info.GetCharacterNameA() + "'s Room", "AutoMatches Created.");
            return;
        }
        foreach(HostData hd in current_list)
        {
            if (hd.passwordProtected)
                continue;
            if (hd.connectedPlayers >= 4)
                continue;
            JoinRoom(hd);
            roots.FinishWWWLoading();
            return;
        }
        roots.FinishWWWLoading();
        CreateRoom(info.GetCharacterNameA() + "'s Room", "AutoMatches Created.");
        return;
    }

    public void InRoomCheck()
    {
        inroomcheck = true;
        MasterServer.ClearHostList();
        MasterServer.RequestHostList("Survive");
    }

    private void InRoomCheckR()
    {
        HostData[] HD_list = MasterServer.PollHostList();
        foreach(HostData hd in HD_list)
        {
            if(Globe.roomid==hd.guid)
            {
                JoinRoom(hd);
                return;
            }
        }
        return;
    }

    private void AfterJoinWork()
    {
        if (Globe.gamestarted)
            return;
        if(isHost)
        {
            Globe.roomid = Network.player.guid;
        }
        GameObject.Find("Launcher UI Root").GetComponent<UI_FunctionControl>().EnterRoom();
    }


}
