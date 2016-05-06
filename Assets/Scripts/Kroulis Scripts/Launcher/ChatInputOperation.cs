using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kroulis.UI.Launcher
{
    public class ChatInputOperation : MonoBehaviour
    {
        private InputField inputfield;
        private Logic_Chat lc;
        // Use this for initialization
        void Start()
        {
            inputfield = GetComponent<InputField>();
            lc = GameObject.Find("Logic_Network").GetComponentInChildren<Logic_Chat>();
        }
        
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                if (inputfield.text != "")
                {
                    if(inputfield.text=="#cls")
                    {
                        GetComponentInParent<UI_FunctionControl>().GetComponent<ChatBoxSet>().CleanChatBox();
                        return;
                    }
                    lc.SendMessage(inputfield.text);
                    inputfield.text = "";
                }
            }
        }
    }
}
