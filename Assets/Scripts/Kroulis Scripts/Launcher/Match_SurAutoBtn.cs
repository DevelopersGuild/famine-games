using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kroulis.UI.Launcher
{
    public class Match_SurAutoBtn : MonoBehaviour
    {
        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(delegate() { this.Action(); });
        }
        void Action()
        {
            GameObject.Find("Logic_Network").GetComponentInChildren<Logic_MatchOperation>().StartMatching();
        }
    }
}

