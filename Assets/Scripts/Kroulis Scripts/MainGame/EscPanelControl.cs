using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace Kroulis.UI.MainGame
{
    public class EscPanelControl : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Y))
            {
                NetworkManager nm= GameObject.FindObjectOfType<NetworkManager>();
                nm.client.Disconnect();
                if(NetworkServer.active)
                {
                    NetworkServer.Shutdown();
                }
            }
            else if(Input.GetKeyDown(KeyCode.N))
            {
                gameObject.SetActive(false);
                GetComponentInParent<MainPanelFullControl>().ContinueGaming();
            }
        }
    }
}
