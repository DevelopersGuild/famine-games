using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kroulis.UI.Launcher
{
    public class RecentResult_Control : MonoBehaviour
    {

        public Text[] MID, Result, PlayerList, Rewards, Kill, Death, Assist = new Text[16];
        // Use this for initialization
        void Start()
        {
            for (int i = 0; i < 16; i++)
            {
                MID[i].text = "0";
                Result[i].text = "Loading";
                PlayerList[i].text = "Loading...";
                Rewards[i].text = "Loading...";
                Kill[i].text = "0";
                Death[i].text = "0";
                Assist[i].text = "0";
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}