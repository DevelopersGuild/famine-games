using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Kroulis.UI.MainGame
{
    public class MainPanelFullControl : MonoBehaviour
    {

        public Text T_Health, T_Sheild, T_WeaponName, T_Ammo, T_Bandage, T_Timer;
        public Image I_WeaponIcon;
        public GameObject G_PickUpTips, G_Compare;
        public GameObject RTKPrefab;
        private GameObject local_player=null;
        private List<GameObject> rtk_list=new List<GameObject>();
        private float rtk_timer=0;
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
                Defense sheild = local_player.GetComponent<Defense>();
                //Timer
                T_Health.text = health.currentHealth.ToString();
                T_Bandage.text = health.bandagesAmount.ToString();
                T_Sheild.text = sheild.GetCurrentAmror().ToString();
                if (local_player.GetComponent<NetworkedPlayer>().attackController.currentWeapon)
                {
                    T_WeaponName.text = local_player.GetComponent<NetworkedPlayer>().attackController.currentWeapon.name;
                    I_WeaponIcon.sprite = local_player.GetComponent<NetworkedPlayer>().attackController.currentWeapon.icon;
                    if(local_player.GetComponent<NetworkedPlayer>().attackController.currentWeapon.currentWeaponType==Weapon.WeaponType.Melee)
                    {
                        T_Ammo.text = "Infinity";
                    }
                    else
                    {
                        int currentammo=local_player.GetComponent<BowAndArrow>().currentAmmo;
                        if (currentammo > 0)
                            T_Ammo.text = "1 / " + currentammo.ToString();
                        else
                            T_Ammo.text = "<color=red>Out of Ammo</color>";
                    }
                    
                }
                else
                {
                    T_WeaponName.text = "Hands";
                    I_WeaponIcon.sprite = null;
                    T_Ammo.text = "Infinity";
                }
            }

            if(rtk_list.Count!=0)
            {
                rtk_timer+=Time.deltaTime;
                //Remove Realtime Killing Tab
                if(rtk_timer>=2.0f)
                {
                    GameObject rtk = rtk_list[0];
                    rtk_list.RemoveAt(0);
                    if(rtk)
                    {
                        Destroy(rtk);
                    }
                    rtk_timer -= 2.0f;
                }
                //Showing Realtime Killing Tab
                for(int i=0;i<rtk_list.Count && i<8;i++)
                {
                    GameObject currentrtk = rtk_list[i];
                    currentrtk.SetActive(true);
                    Vector3 posi = currentrtk.transform.localPosition;
                    posi.y=285-42*i;
                    //Debug.Log(posi);
                    currentrtk.transform.localPosition=posi;
                    rtk_list[i] = currentrtk;
                }
                for(int i=8;i<rtk_list.Count;i++)
                {
                    rtk_list[i].SetActive(false);
                }
            }
            else
            {
                rtk_timer=0;
            }
        }

        void OnGUI()
        {
            if(GUILayout.Button("Press to add a Killing tab."))
            {
                AddKillingTab(0, "test1", "test2");
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

        public void AddKillingTab(int weapontype, string name1,string name2)
        {
            GameObject ptk = Instantiate<GameObject>(RTKPrefab);
            ptk.transform.parent = gameObject.transform;
            ptk.transform.position = RTKPrefab.transform.position;
            ptk.transform.localPosition = RTKPrefab.transform.localPosition;
            ptk.transform.localScale = RTKPrefab.transform.localScale;
            Sprite icon=null;
            ptk.SetActive(true);
            ptk.GetComponent<RealtimeKillingTabControl>().UpdateInfo(icon, name1, name2);
            ptk.SetActive(false);
            rtk_list.Add(ptk);
        }
    }
}