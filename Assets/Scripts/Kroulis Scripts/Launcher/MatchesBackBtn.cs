using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kroulis.UI.Launcher
{
    public class MatchesBackBtn : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(delegate() { this.Action(); });
        }
        void Action()
        {
            UI_MatchesControl uim=GetComponentInParent<UI_MatchesControl>();
            uim.GetComponentInChildren<Match_Panel_Control>().StopUpdating();
            uim.UI.SetActive(false);
            uim.gameObject.GetComponentInChildren<MatchPanalBGMovControl>().StartLoopOut();
        }
    }
}
