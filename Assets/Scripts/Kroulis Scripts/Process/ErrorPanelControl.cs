using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Kroulis.UI.Process
{
    public class ErrorPanelControl : MonoBehaviour
    {
        public GameObject LostConnection;
        public GameObject HostQuitted;
        public GameObject FatalError;
        private int type = 0;
        // Use this for initialization
        void Start()
        {
            CheckErrorType();
            LostConnection.SetActive(false);
            HostQuitted.SetActive(false);
            FatalError.SetActive(false);
            switch(type)
            {
                case 1:
                    LostConnection.SetActive(true);
                    break;
                case 2:
                    HostQuitted.SetActive(true);
                    break;
                case 3:
                    FatalError.SetActive(true);
                    break;
                default:
                    GameObject net = GameObject.Find("Logic_Network");
                    if (net)
                    {
                        net.GetComponentInChildren<Logic_MatchOperation>().QuitMathces();
                        DestroyObject(net);
                    }
                    //Application.LoadLevel("Launcher");
                    SceneManager.LoadScene("Launcher");
                    break;
            }

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Query()
        {
            switch(type)
            {
                case 1: //Lost Connection
                    IEnumerator<Toggle> ien= LostConnection.GetComponentInChildren<ToggleGroup>().ActiveToggles().GetEnumerator();
                    ien.MoveNext();
                    int QueryIndex = ien.Current.GetComponent<ERROptionalIndex>().index;
                    GameObject net = GameObject.Find("Logic_Network");    
                    switch(QueryIndex)
                    {
                        case 1:
                            
                            if(net)
                            {
                                if(net.GetComponentInChildren<Logic_MatchOperation>().IsUsingNewNetworkSystem())
                                {
                                    net.GetComponentInChildren<Logic_UNetConfig>().RejoinMatch();
                                }
                                else
                                {
                                    net.GetComponent<NetworkManager>().StartClient();
                                }
                                
                            }
                            break;
                        case 2:
                            Debug.Log("Choose Report.");
                            break;
                        case 3:
                            if (net)
                            {
                                net.GetComponentInChildren<Logic_MatchOperation>().QuitMathces();
                                DestroyObject(net);
                            }
                            Globe.roomid = "";
                            SceneManager.LoadScene("Launcher");
                            break;
                        default:
                            break;
                    }
                    break;
                case 2:

                    break;
                case 3:

                    break;
                default:
                    Debug.LogWarning("Unknown Error.");
                    return;
            }
        }

        private void CheckErrorType()
        {
            if(Globe.errorid=="EHCONQ")
            {
                type = 2;
                return;
            }
            if(Globe.errorid=="EFATAL")
            {
                type = 3;
                return;
            }
            if(Globe.errorid!="0")
            {
                type = 0;
                return;
            }
            type = 1;
        }
    }
}
