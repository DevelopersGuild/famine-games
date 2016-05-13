using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace Kroulis.UI.MainGame
{
    public class CompareControl : MonoBehaviour
    {
        public Text[] T_Name, T_Range, T_Ammo, T_Damage, T_Special, T_Cooldown;
        public Image[] I_Icon;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ChangeData(Weapon current,Weapon compare)
        {
            //name
            T_Name[0].text = current.name;
            T_Name[1].text = compare.name;
            //icon
            I_Icon[0].sprite = current.icon;
            I_Icon[1].sprite = compare.icon;

            BowAndArrow baa = new BowAndArrow();
            //range & ammo & damage & special & cooldown
            if(current.currentWeaponType==Weapon.WeaponType.Melee)
            {
                T_Range[0].text = "<color=red>Melee</color>";
                T_Ammo[0].text = "Infinity";
                T_Damage[0].text = current.damage.ToString() + " <color=orange>(+0)</color>";
                T_Special[0].text = "";
                T_Cooldown[0].text = current.attackCooldown.ToString() + "s";
            }
            else
            {
                T_Range[0].text = "<color=green>Ranged</color>";
                T_Ammo[0].text = "1 / Unlimited";
                T_Damage[0].text = current.damage.ToString() + " <color=orange>(+5)</color>";
                T_Special[0].text = "Chargable:"+baa.maxStrengthPullTime.ToString()+"s";
                T_Cooldown[0].text = baa.fireRate.ToString() + "s";
            }

            if(compare.currentWeaponType==Weapon.WeaponType.Melee)
            {
                T_Range[1].text = "<color=red>Melee</color>";
                T_Ammo[1].text = "Infinity";
                T_Damage[1].text = compare.damage.ToString() + " <color=orange>(+0)</color>";
                T_Special[1].text = "";
                T_Cooldown[1].text = compare.attackCooldown.ToString() + "s";
            }
            else
            {
                T_Range[1].text = "<color=green>Ranged</color>";
                T_Ammo[1].text = "1 / Unlimited";
                T_Damage[1].text = compare.damage.ToString() + " <color=orange>(+5)</color>";
                T_Special[1].text = "Chargable:" + baa.maxStrengthPullTime.ToString() + "s";
                T_Cooldown[1].text = baa.fireRate.ToString() + "s";
            }

        }
    }
}