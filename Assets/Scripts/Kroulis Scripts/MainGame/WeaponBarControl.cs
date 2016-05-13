using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kroulis.UI.MainGame
{
    public class WeaponBarControl : MonoBehaviour
    {
        public Image CoolDownBar, ChargeBar;
        private bool isCoolingdown = false;
        private bool isCharging = false;
        private float cooldowntime = 0;
        private float currenttime = 0;
        private float currentcharge = 0;
        private float maximumcharge = 0;
        // Use this for initialization
        void Start()
        {
            CoolDownBar.fillAmount = 0;
            ChargeBar.fillAmount = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if(isCoolingdown)
            {
                currenttime += Time.deltaTime;
                ChargeBar.fillAmount = 0;
                CoolDownBar.fillAmount = currenttime / cooldowntime;
                if(currenttime>=cooldowntime)
                {
                    isCoolingdown = false;
                    cooldowntime = 0;
                    currenttime = 0;
                }
            }
            else
            {
                CoolDownBar.fillAmount = 0;
                if(isCharging)
                {
                    currentcharge += Time.deltaTime;
                    ChargeBar.fillAmount = currentcharge / maximumcharge;
                }
                else
                {
                    ChargeBar.fillAmount = 0;
                }
                
            }
        }

        public void StartCooldown(float cooldownt)
        {
            if (isCoolingdown)
                return;
            cooldowntime = cooldownt;
            isCoolingdown = true;
        }

        public void StartCharge(float maximumc)
        {
            maximumcharge = maximumc;
            isCharging = true;
        }

        public void StopCharge()
        {
            isCharging = false;
            maximumcharge = 0;
            currentcharge = 0;
        }
    }
}