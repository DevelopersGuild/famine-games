using UnityEngine;
using System.Collections;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking;
using Kroulis.UI.MainGame;

public class UnetChat : NetworkBehaviour {
    
    
    const short CHAT_MSG = MsgType.Highest + 1; // Unique message ID
    private NetworkClient client;
    private bool Ready;
	// Use this for initialization
	void Start () {
        Ready = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if(client==null)
        {
            NetworkManager netManager = GameObject.FindObjectOfType<NetworkManager>();
            client = netManager.client;
        }
        if(!Ready && client.isConnected)
        {
            if(isServer)
            {
                NetworkServer.RegisterHandler(CHAT_MSG, ServerReceiveChatMessage);
            }
            client.RegisterHandler(CHAT_MSG, ClientReceiveChatMessage);
            Ready = true;
        }
	}

    public void SendChat(string msg)
    {
        StringMessage strMsg = new StringMessage(msg);
        if (isServer)
        {
            NetworkServer.SendToAll(CHAT_MSG, strMsg); // Send to all clients
        }
        else if (client.isConnected)
        {
            client.Send(CHAT_MSG, strMsg); // Sending message from client to server
        }
    }

    public void ServerReceiveChatMessage(NetworkMessage netMsg)
    {
        string str = netMsg.ReadMessage<StringMessage>().value;
        if (isServer)
        {
            SendChat(str); // Send the chat message to all clients
        }
    }

    public void ClientReceiveChatMessage(NetworkMessage netMsg)
    {
        string str = netMsg.ReadMessage<StringMessage>().value;
        MainPanelFullControl mpfc = GameObject.Find("Main_UI").GetComponent<MainUIFullControl>().MainPanel.GetComponent<MainPanelFullControl>();
        if (mpfc)
        {
            ChatBoxFullControl cbfc = mpfc.ChatBox.GetComponent<ChatBoxFullControl>();
            if (!cbfc)
            {
                mpfc.ChatBox.SetActive(true);
                cbfc = mpfc.ChatBox.GetComponent<ChatBoxFullControl>();
            }
            cbfc.UpdateChatMsg(str);
        }
    }
}
