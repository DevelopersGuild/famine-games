using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kroulis.UI.MainGame
{
    public class MainPanelFullControl : MonoBehaviour
    {

        public Text T_Health, T_Sheild, T_WeaponName, T_Ammo, T_Bandage, T_Timer;
        public Image I_WeaponIcon;
        public GameObject G_PickUpTips, G_Compare;
        private GameObject local_player=null;

        // Use this for initialization
        void Start()
        {
            G_PickUpTips.SetActive(false);
            G_Compare.SetActive(false);
            InvokeRepeating("GetLocalPlayer", 0, 1f);
        }

        // Update is called once per frame
        void Update()
        {
            if(local_player)
            {
                Health health = local_player.GetComponent<Health>();
                //Sheild sheild = local_player.GetComponent<Sheild>();
                //Timer
                T_Health.text = health.currentHealth.ToString();
                T_Sheild.text = "0";
                T_WeaponName.text = local_player.GetComponent<NetworkedPlayer>().attackController.currentWeapon.name;
                I_WeaponIcon.sprite = local_player.GetComponent<NetworkedPlayer>().attackController.currentWeapon.icon;
                //T_Ammo
            }
        }

        private void GetLocalPlayer()
        {
            if(!local_player)
            {
                local_player = GameObject.Find("LOCAL Player");
            }
        }

        public void ShowPickupTips()
        {
            G_PickUpTips.SetActive(true);
            CancelInvoke("HidePickupTips");
            Invoke("HidePickupTips", 0.5f);

        }

        private void HidePickupTips()
        {
            G_PickUpTips.SetActive(false);
        }

        public void ShowComparePanel(Weapon weapon)
        {
            G_Compare.SetActive(true);
            G_Compare.GetComponent<CompareControl>().ChangeData(local_player.GetComponent<NetworkedPlayer>().attackController.currentWeapon,weapon);
        }

        public void HideComparePanel()
        {
            G_Compare.SetActive(false);
        }

    }
}