using UnityEngine;
using System.Collections;

namespace Kroulis.UI.MainGame
{
    public class ComparableWeapon : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnMouseEnter()
        {
            MainPanelFullControl mpfc = GameObject.Find("Main_UI").GetComponentInChildren<MainPanelFullControl>();
            if (mpfc)
            {
                mpfc.ShowComparePanel(GetComponent<Weapon>());
            }
        }

        void OnMouseExit()
        {
            MainPanelFullControl mpfc = GameObject.Find("Main_UI").GetComponentInChildren<MainPanelFullControl>();
            if (mpfc)
            {
                mpfc.HideComparePanel();
            }
        }
    }
}