using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kroulis.UI.Launcher
{
    public class Match_SurCusBtn : MonoBehaviour
    {

        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(delegate() { this.Action(); });
        }
        void Action()
        {
            GetComponentInParent<Match_Panel_Control>().StartUpdating();
        }
    }
}

