using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kroulis.UI.Process
{
    public class ProcessUIControl : MonoBehaviour
    {
        public GameObject ErrorPanel;
        public GameObject ResultPanel;
        //private bool test = true;
        void Awake()
        {
            /*if(test)
            {
                Globe.resultid = "1";
            }*/
            if(Globe.resultid=="")
            {
                ErrorPanel.SetActive(true);
                ResultPanel.SetActive(false);
            }
            else
            {
                ErrorPanel.SetActive(false);
                ResultPanel.SetActive(true);
            }
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void GoesToError()
        {
            ErrorPanel.SetActive(true);
            ResultPanel.SetActive(false);
        }
    }
}