using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kroulis.UI.Launcher
{
    public class Matches_CreateBtn : MonoBehaviour
    {
        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(delegate() { this.Action(); });
        }
        void Action()
        {
            GameObject.Find("Logic_Network").GetComponentInChildren<Logic_MatchOperation>().CreateRoom();
        }
    }
}

