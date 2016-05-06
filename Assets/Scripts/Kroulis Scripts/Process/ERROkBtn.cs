using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kroulis.UI.Process
{
    public class ERROkBtn : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(delegate { this.Action(); });
        }

        // Update is called once per frame
        void Update()
        {

        }

        void Action()
        {
            GetComponentInParent<ErrorPanelControl>().Query();
        }
    }
}
