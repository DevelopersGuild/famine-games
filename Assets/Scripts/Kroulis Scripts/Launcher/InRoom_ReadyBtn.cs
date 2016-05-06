using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kroulis.UI.Launcher
{
    public class InRoom_ReadyBtn : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(delegate() { this.Action(); });
        }
        void Action()
        {
            GameObject.Find("Logic_Network").GetComponentInChildren<Logic_Chat>().SendReadyInfo();
        }
    }
}
