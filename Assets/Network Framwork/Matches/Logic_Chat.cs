using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Kroulis.UI.Launcher;

public struct ChatMessage
{
    public string PlayerName;
    public string Message;
    public int type;
}

public struct UserInfoMessage
{
    public string PlayerName;
    public string Guid;
    public bool Ready;

    public void SetReady(bool value)
    {
        Ready = value;
    }
}

public class Logic_Chat : MonoBehaviour {

    private NetworkView networkView;
    private List<UserInfoMessage> userlist=new List<UserInfoMessage>();
    private Text chatbox;
    private int timer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake()
    {
        networkView=GetComponent<NetworkView>();
        if(!networkView)
        {
            networkView = gameObject.AddComponent<NetworkView>();
        }
    }

    public void SetChatBox(Text cb)
    {
        chatbox = cb;
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Recieved Connection Ping from Master Server. Waiting for calling.");
    }

    void OnConnectedToServer()
    {
        UserInfoMessage uim=new UserInfoMessage();
        uim.PlayerName=GetComponentInParent<Logic_LauncherGetInfo>().GetCharacterNameA();
        uim.Guid = Network.player.guid;
        networkView.RPC("NewUserRegister",RPCMode.Server,uim.PlayerName,uim.Guid);
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        if (Network.peerType == NetworkPeerType.Server)
        {
            for(int i=0;i<userlist.Count;i++)
            {
                if(userlist[i].Guid==player.guid)
                {
                    ChatMessage msg=new ChatMessage();
                    msg.PlayerName="System";
                    msg.Message="Player "+userlist[i].PlayerName+" left the room.";
                    msg.type = 1;
                    networkView.RPC("MessageBoxUpdate", RPCMode.All, msg.Message, msg.PlayerName, msg.type);
                    userlist.Remove(userlist[i]);
                    break;
                }
            }
            Network.RemoveRPCs(player);
        }
    }

    void OnDisconnectedFromServer()
    {
        GameObject root = GameObject.Find("Launcher UI Root");
        if(root)
        {
            root.GetComponent<UI_FunctionControl>().InRoomPanel.SetActive(true);
            root.GetComponentInChildren<ChatBoxSet>().ResetChatBox();
            root.GetComponent<UI_FunctionControl>().QuitRoom();
        }
    }

    [RPC]
    void MessageBoxUpdate(string Message,string PlayerName,int Type,NetworkMessageInfo info)
    {
        //Debug.Log("Get New Message-> " + PlayerName + ":" + Message);
        if (Globe.gamestarted)
            return;
        if(!chatbox)
        {
            Debug.LogWarning("ChatBox Not Initialized.");
        }
        string chatmsg = "";
        switch (Type)
        {
            case 1:
                chatmsg += "<color=red>";
                break;
            case 2:
                chatmsg += "<color=yellow>";
                break;
            default:
                chatmsg += "<color=white>";
                break;
        }
        chatmsg += "<b>" + PlayerName + "</b>:" + Message + "</color>\n";
        chatbox.text += chatmsg;
    }

    public void MessageBoxUpdate(string Message, string PlayerName, int Type)
    {
        //Debug.Log("Get New Message-> " + PlayerName + ":" + Message);
        if (Globe.gamestarted)
            return;
        if (!chatbox)
        {
            Debug.LogWarning("ChatBox Not Initialized.");
        }
        string chatmsg = "";
        switch (Type)
        {
            case 1:
                chatmsg += "<color=red>";
                break;
            case 2:
                chatmsg += "<color=yellow>";
                break;
            default:
                chatmsg += "<color=white>";
                break;
        }
        chatmsg += "<b>" + PlayerName + "</b>:" + Message + "</color>\n";
        chatbox.text += chatmsg;
    }

    [RPC]
    void NewUserRegister(string PlayerName,string GUID,NetworkMessageInfo info)
    {
        UserInfoMessage uim = new UserInfoMessage();
        uim.PlayerName = PlayerName;
        uim.Guid = GUID;
        userlist.Add(uim);

        if(Globe.gamestarted)
        {
            ChatMessage msg = new ChatMessage();
            msg.PlayerName = "System";
            msg.Message = "The game already started. Press Ready to join the game.";
            msg.type = 2;
            networkView.RPC("MessageBoxUpdate", info.sender, msg.Message, msg.PlayerName, msg.type);
        }
        else
        {
            ChatMessage msg = new ChatMessage();
            msg.PlayerName = "System";
            msg.Message = "Player " + uim.PlayerName + " joined the room.";
            msg.type = 1;
            networkView.RPC("MessageBoxUpdate", RPCMode.All, msg.Message, msg.PlayerName, msg.type);
        }     
    }

    public void SendMessage(string Message)
    {
        ChatMessage msg=new ChatMessage();
        msg.Message = Message;
        msg.PlayerName = GetComponentInParent<Logic_LauncherGetInfo>().GetCharacterNameA();
        msg.type = 0;
        if (GetComponentInParent<Logic_MatchOperation>().UsingNewNetworkSystem)
            GetComponentInParent<Logic_UNetConfig>().SendChatMessage(msg);
        else
            networkView.RPC("MessageBoxUpdate", RPCMode.All, msg.Message, msg.PlayerName, msg.type);
    }

    [RPC]
    private void ReadyReceived(string GUID,NetworkMessageInfo info)
    {
        Debug.Log("One Player ready.");
        foreach(UserInfoMessage uim in userlist)
        {
            if(uim.Guid==GUID)
            {
                UserInfoMessage newuim = new UserInfoMessage();
                newuim.Guid = uim.Guid;
                newuim.PlayerName = uim.PlayerName;
                newuim.Ready = true;
                userlist.Remove(uim);
                userlist.Add(newuim);
                break;
            }
        }
        if(Globe.gamestarted)
        {
            networkView.RPC("CilentStartGame", info.sender);
        }
        else
        {
            if (userlist.Count >= 1)
            {
                Debug.Log("Start All Ready Checklist.");
                bool AllReadyFlag = true;
                for (int i = 0; i < userlist.Count && AllReadyFlag; i++)
                {
                    if (!userlist[i].Ready)
                    {
                        AllReadyFlag = false;
                    }
                }
                if (AllReadyFlag)
                {
                    Debug.Log("All Ready. Start start-game checklist.");
                    timer = 5;
                    InvokeRepeating("StartGameCounting", 1.0f, 1.0f);
                }
                else
                {
                    Debug.Log("Not All Ready.");
                }
            }
        }

    }

    public void SendReadyInfo()
    {
        string guid = Network.player.guid;
        networkView.RPC("ReadyReceived", RPCMode.Server, guid);
    }

    private void StartGameCounting()
    {
        if(timer>0)
        {
            ChatMessage cm = new ChatMessage();
            cm.Message = "The game will start in " + timer.ToString() + " seconds.";
            cm.PlayerName = "System";
            cm.type = 2;
            timer--;
            networkView.RPC("MessageBoxUpdate", RPCMode.All, cm.Message, cm.PlayerName, cm.type);
        }
        else
        {
            CancelInvoke("StartGameCounting");
            GetComponentInParent<Logic_MatchOperation>().StartGame();
            //gameObject.SetActive(false);
        }
    }

    public void StartGameCountingToggerN()
    {
        timer = 5;
        InvokeRepeating("StartGameCountingN", 1.0f, 1.0f);
    }

    private void StartGameCountingN()
    {
        if (timer > 0)
        {
            ChatMessage cm = new ChatMessage();
            cm.Message = "The game will start in " + timer.ToString() + " seconds.";
            cm.PlayerName = "System";
            cm.type = 2;
            timer--;
            GetComponentInParent<Logic_UNetConfig>().SendChatMessage(cm);
        }
        else
        {
            CancelInvoke("StartGameCounting");
            GetComponentInParent<Logic_UNetConfig>().SendStartGameInfo();
            //gameObject.SetActive(false);
        }
    }

    public void SendCilentStartGameInfo()
    {
        networkView.RPC("CilentStartGame", RPCMode.Others);
    }

    [RPC]
    void CilentStartGame(NetworkMessageInfo info)
    {
        GetComponentInParent<Logic_MatchOperation>().StartGame(info.sender.ipAddress);
        //gameObject.SetActive(false);
    }
    public void ReconnectChatBoxFix()
    {
        ChatMessage msg = new ChatMessage();
        msg.PlayerName = "System";
        msg.Message = "You are disconnected from the server. Press Ready to reconnect.";
        msg.type = 2;
        //networkView.RPC("MessageBoxUpdate", Network.player, msg.Message, msg.PlayerName, msg.type);
    }
}
