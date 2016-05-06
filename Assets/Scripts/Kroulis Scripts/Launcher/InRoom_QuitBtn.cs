using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kroulis.UI.Launcher
{
    public class InRoom_QuitBtn : MonoBehaviour
    {

        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(delegate() { this.Action(); });
        }
        void Action()
        {
            GameObject.Find("Logic_Network").GetComponentInChildren<Logic_MatchOperation>().QuitMathces();
            GameObject.Find("Launcher UI Root").GetComponent<UI_FunctionControl>().QuitRoom();
        }
    }
}