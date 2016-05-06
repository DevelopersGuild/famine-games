using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kroulis.UI.Launcher
{
    public class MatchesToggleControl : MonoBehaviour
    {
        public Toggle[] toggle = new Toggle[13];
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public int GetCheckID()
        {
            for(int i=0;i<13;i++)
            {
                if (toggle[i].isOn)
                    return i;
            }
            return -1;
        }

        public void SetShowItems(int number)
        {
            if(number>13)
            {
                number = 13;
            } 
            for(int i=0;i<number;i++)
            {
                toggle[i].gameObject.SetActive(true);
            }
            for(int i=number;i<13;i++)
            {
                toggle[i].gameObject.SetActive(false);
            }
        }
    }
}