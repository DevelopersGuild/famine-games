using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kroulis.UI.Launcher
{
    public class UI_FunctionControl : MonoBehaviour
    {


        public GameObject PopupBG;
        public GameObject RDP;
        public GameObject IndexPanel;
        public GameObject MatchPanel;
        public GameObject InRoomPanel;
        //private GameObject chatbox;
        // Use this for initialization
        void Start()
        {
            //IndexPanel.SetActive(true);
            //PopupBG.SetActive(true);
            //RDP.SetActive(true);
            //chatbox = InRoomPanel.GetComponentInChildren<ChatBoxSet>().gameObject;
            Globe.gamestarted = false;
            MatchPanel.SetActive(false);
            InRoomPanel.SetActive(false);
            Invoke("inituserdata", 0.5f);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void FinishWWWLoading()
        {
            WaitForSeconds wt = new WaitForSeconds(1.0f);
            //Debug.Log("Finished Waiting");
            RDP.SetActive(false);
            PopupBG.SetActive(false);
        }

        public void StartWWWLoading()
        {
            RDP.SetActive(true);
            PopupBG.SetActive(true);
        }

        private void inituserdata()
        {
            if (!Globe.initeduser)
            {
                GameObject gmo = GameObject.Find("GameManagerSet");
                if (gmo)
                {
                    GameManagerSetTools GM = gmo.GetComponent<GameManagerSetTools>();
                    if (GM.testing == true && GM.load_from_internet == false)
                    {
                        return;
                    }
                }
                IndexPanel_FullControl IdxCont = IndexPanel.GetComponent<IndexPanel_FullControl>();
                Logic_LauncherGetInfo getinfo = GameObject.Find("Logic_Network").GetComponentInChildren<Logic_LauncherGetInfo>();
                getinfo.GetInfo(IdxCont.name, IdxCont.level, IdxCont.expt, IdxCont.expi, IdxCont.gold, IdxCont.diamond);
                getinfo.GetRecentMatch(GetComponentInChildren<RecentResult_Control>());
            }
            if(Globe.roomid!="")
            {
                EnterRoom();
                if (GameObject.Find("Logic_Network").GetComponentInChildren<Logic_MatchOperation>().UsingNewNetworkSystem == false)
                    GameObject.Find("Logic_Network").GetComponentInChildren<Logic_Chat>().ReconnectChatBoxFix();
            }
        }
        public void EnterRoom()
        {
            InRoomPanel.SetActive(true);
        }

        public void QuitRoom()
        {
            InRoomPanel.SetActive(false);
        }
    }
}
