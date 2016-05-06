using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kroulis.UI.Launcher
{
    public class Matches_Roomlist : MonoBehaviour
    {
        public Text[] ID = new Text[13];
        public Text[] Name = new Text[13];
        public Text[] Psw = new Text[13];
        public Text[] Enrolled = new Text[13];
        public Text[] Capicity = new Text[13];
        public ToggleGroup Select;
        public int CurrentID = -1;
        // Use this for initialization
        void Start()
        {
            for (int i = 0; i < 13; i++)
            {
                ID[i].text = Name[i].text = Psw[i].text = Enrolled[i].text = Capicity[i].text = "";
            }
            Select.SetAllTogglesOff();
            Select.gameObject.SetActive(false);
            CurrentID = -1;
        }

        // Update is called once per frame
        void Update()
        {
            CurrentID = Select.GetComponent<MatchesToggleControl>().GetCheckID();
        }
    }
}

