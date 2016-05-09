using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace Kroulis.UI.MainGame
{
    public class PickupableItem : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerStay(Collider collider)
        {
            if(collider.GetComponent<NetworkedPlayer>()!=null && collider.GetComponent<NetworkIdentity>()!=null)
            {
                if(collider.GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    MainPanelFullControl mpfc = GameObject.Find("Main_UI").GetComponentInChildren<MainPanelFullControl>();
                    if(mpfc)
                    {
                        mpfc.ShowPickupTips();
                    }
                }
            }
        }
    }
}
