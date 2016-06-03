using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Kroulis.Components;

namespace Kroulis.UI.MainGame
{
    public class MainPanelFullControl : MonoBehaviour
    {

        public Text T_Health, T_Sheild, T_WeaponName, T_Ammo, T_Bandage, T_Timer,T_Tips;
        public Image I_WeaponIcon;
        public GameObject G_PickUpTips, G_Compare;
        public GameObject RTKPrefab;
        public GameObject ChatBox;
        public GameObject LootBox;
        private GameObject local_player=null;
        private List<GameObject> rtk_list=new List<GameObject>();
        private float rtk_timer=0;
        private GameProcess gp;
        private bool menu_in_use = false;

        // Use this for initialization
        void Start()
        {
            G_PickUpTips.SetActive(false);
            G_Compare.SetActive(false);
            LootBox.SetActive(false);
            T_Tips.gameObject.SetActive(false);
            InvokeRepeating("GetLocalPlayer", 0, 1f);
        }

        // Update is called once per frame
        void Update()
        {
            if (!gp && GameObject.Find("GameCoreProcess"))
                gp = GameObject.Find("GameCoreProcess").GetComponent<GameProcess>();

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

                if(!menu_in_use && Input.GetKeyDown(KeyCode.Return))
                {
                    ChatBoxFullControl cbfc= ChatBox.GetComponent<ChatBoxFullControl>();
                    if(!cbfc)
                    {
                        ChatBox.SetActive(true);
                        cbfc = ChatBox.GetComponent<ChatBoxFullControl>();
                    }
                    if(cbfc.InputBarOn)
                    {
                        cbfc.HideAll();
                        GameObject.Find("CursorLocker").GetComponent<InGameCursorLocker>().LockMouse = true;
                        local_player.GetComponent<NetworkedPlayer>().fpsController.SetInput(true);
                    }
                    else
                    {
                        cbfc.ShowAll();
                        GameObject.Find("CursorLocker").GetComponent<InGameCursorLocker>().LockMouse = false;
                        local_player.GetComponent<NetworkedPlayer>().fpsController.SetInput(false);
                    }
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
            if(gp)
            {
                int ts = (int)gp.timestamp;
                int minutes = ts / 60;
                int seconds = ts % 60;
                T_Timer.text = minutes.ToString("D2") + ":" + seconds.ToString("D2") + " / 15:00";
            }
            else
            {
                T_Timer.text = "0:00 / 15:00";
            }
        }

        void OnGUI()
        {
            /*
            if(GUILayout.Button("Press to add a Killing tab."))
            {
                AddKillingTab(0, "test1", "test2");
            }*/
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

        public IEnumerator ShowLootWindow(Chest chest_object,float wait_time=0.1f)
        {
            if(ChatBox.activeInHierarchy)
            {
                ChatBoxFullControl cbfc = ChatBox.GetComponent<ChatBoxFullControl>();
                cbfc.HideAll();
            }
            menu_in_use = true;
            yield return new WaitForSeconds(wait_time);
            LootBox.SetActive(true);
            LootBox.GetComponent<LootBoxFullControl>().UpdateItems(chest_object);
            GameObject.Find("CursorLocker").GetComponent<InGameCursorLocker>().LockMouse = false;
            local_player.GetComponent<NetworkedPlayer>().fpsController.SetInput(false);
        }

        public void HideLootWindow()
        {
            menu_in_use = false;
            StopCoroutine("ShowLootWindow");
            LootBox.GetComponent<LootBoxFullControl>().ClearItems();
            LootBox.SetActive(false);
            GameObject.Find("CursorLocker").GetComponent<InGameCursorLocker>().LockMouse = true;
            local_player.GetComponent<NetworkedPlayer>().fpsController.SetInput(true);
        }

        public void UpdateTips(string tips)
        {
            CancelInvoke("HideTips");
            T_Tips.gameObject.SetActive(true);
            T_Tips.text = tips;
            Invoke("HideTips", 2.0f);
        }

        private void HideTips()
        {
            T_Tips.gameObject.SetActive(false);
        }

    }
}