using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using Kroulis.Components;

namespace Kroulis.UI.MainGame
{
    public class ChatBoxFullControl : MonoBehaviour
    {
        public bool InputBarOn = false;
        public bool LogBarOn = false;
        public Image ChatLog;
        public Image ChatInput;
        public Text UserTab;
        void Awake()
        {
            ChatInput.gameObject.SetActive(false);
            ChatLog.gameObject.SetActive(false);
            InputBarOn = false;
            LogBarOn = false;
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateChatMsg(string msg)
        {
            CancelInvoke("HideChatBox");
            LogBarOn = true;
            ChatLog.gameObject.SetActive(true);
            Text tb = ChatLog.GetComponentInChildren<ScrollRect>().viewport.GetComponentInChildren<Text>();
            tb.text = tb.text + msg + "\n" ;
            Invoke("HideChatBox", 3f);
        }

        private void HideChatBox()
        {
            if(!InputBarOn)
            {
                LogBarOn = false;
                ChatLog.gameObject.SetActive(false);
            }
        }

        public void ShowAll()
        {
            ChatInput.gameObject.SetActive(true);
            UserTab.text = GameObject.Find("LOCAL Player").GetComponent<ContestInfomation>().player_name + ":";
            InputBarOn = true;
            ChatLog.gameObject.SetActive(true);
            LogBarOn = true;
            ChatInput.GetComponent<InputField>().ActivateInputField();
        }

        public void HideAll()
        {
            CancelInvoke("HideChatBox");
            if(InputBarOn && ChatInput.GetComponent<InputField>().text!="")
            {
                //GameObject.Find("ChatSystem").GetComponent<UnetChat>().SendChat(GameObject.Find("LOCAL Player").GetComponent<NetworkIdentity>().netId, ChatInput.GetComponent<InputField>().text);
                GameObject localplayer = GameObject.Find("LOCAL Player");
                GameObject.Find("ChatSystem").GetComponent<UnetChat>().SendChat(localplayer.GetComponent<ContestInfomation>().player_name + " : " + ChatInput.GetComponent<InputField>().text);
                ChatInput.GetComponent<InputField>().text = "";
            }
            ChatInput.GetComponent<InputField>().DeactivateInputField();
            ChatInput.gameObject.SetActive(false);
            InputBarOn = false;
            ChatLog.gameObject.SetActive(false);
            LogBarOn = false;
        }
    }
}