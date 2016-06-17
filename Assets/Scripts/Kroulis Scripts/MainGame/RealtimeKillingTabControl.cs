using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kroulis.UI.MainGame
{
    public class RealtimeKillingTabControl : MonoBehaviour
    {
        public Image KillingType;
        public Text[] PlayerName = new Text[2];
        public Sprite[] weapontype = new Sprite[2];
        
        void Awake()
        {
            if(!KillingType)
            {
                KillingType = GetComponentInChildren<Image>();
            }


            Text[] pnl = GetComponentsInChildren<Text>();
            foreach(Text t in pnl)
            {
                if(t.name=="NameTab1")
                {
                    if (!PlayerName[0])
                    {
                        PlayerName[0] = t;
                    }
                    continue;
                }

                if(t.name=="NameTab2")
                {
                    if(!PlayerName[1])
                    {
                        PlayerName[1] = t;
                    }
                    continue;
                }
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

        public void UpdateInfo(int type,string name1,string name2)
        {
            KillingType.sprite = weapontype[type-1];
            PlayerName[0].text = name1;
            PlayerName[1].text = name2;
        }

    }
}