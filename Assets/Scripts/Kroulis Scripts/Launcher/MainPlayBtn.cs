using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kroulis.UI.Launcher
{
    public class MainPlayBtn : MonoBehaviour
    {

        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(delegate() { this.Action(); });
        }
        void Action()
        {
            UI_FunctionControl root = GetComponentInParent<UI_FunctionControl>();
            root.MatchPanel.GetComponent<UI_MatchesControl>().UI.SetActive(false);
            root.MatchPanel.SetActive(true);
            root.MatchPanel.GetComponentInChildren<MatchPanalBGMovControl>().StartLoopIn();
        }
    }
}

