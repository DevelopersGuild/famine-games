using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kroulis.UI.Launcher
{
    public class ChatBoxSet : MonoBehaviour
    {
        private Text cb;
        private Logic_Chat lc;
        // Use this for initialization
        void Start()
        {
            cb = GetComponent<Text>();
            lc = GameObject.Find("Logic_Network").GetComponentInChildren<Logic_Chat>();
            lc.SetChatBox(cb);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CleanChatBox()
        {
            cb.text = "";
        }
        public void ResetChatBox()
        {
            if (!cb)
                Start();
            cb.text = "<color=green><b>System</b>:Welcome!</color>\n";
        }
    }
}
