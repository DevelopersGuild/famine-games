using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kroulis.UI.MainGame
{
    public class LootBoxFullControl : MonoBehaviour
    {

        private Chest CurrentChestObj;
        private int CurrentSelectID=-1;
        public Text tips;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(CurrentSelectID==-1)
            {
                tips.text = "Click on one item to show information.";
            }
            else if(CurrentSelectID>=CurrentChestObj.items.Count)
            {
                tips.text = "This slot is empty";
            }
            else
            {
                if (CurrentChestObj.items[CurrentSelectID].GetComponent<IItem>() != null)
                    tips.text = CurrentChestObj.items[CurrentSelectID].GetComponent<IItem>().GetDescription();
                else if (CurrentChestObj.items[CurrentSelectID].GetComponentInChildren<IItem>() != null)
                    tips.text = CurrentChestObj.items[CurrentSelectID].GetComponentInChildren<IItem>().GetDescription();
                tips.text += "\n Press F to pick up current Item.";
            }
            if(Input.GetKeyDown(KeyCode.F))
            {
                if(CurrentChestObj && CurrentSelectID!=-1 && CurrentSelectID<CurrentChestObj.items.Count)
                {
                    GameObject newitem = Instantiate(CurrentChestObj.items[CurrentSelectID]);
                    if(CurrentChestObj.items[CurrentSelectID].GetComponent<IItem>()!=null)
                        CurrentChestObj.items[CurrentSelectID].GetComponent<IItem>().OnPickupInChest(GameObject.Find("LOCAL Player"));
                    else if(CurrentChestObj.items[CurrentSelectID].GetComponentInChildren<IItem>() != null)
                        CurrentChestObj.items[CurrentSelectID].GetComponentInChildren<IItem>().OnPickupInChest(GameObject.Find("LOCAL Player"));
                    CurrentChestObj.removeItem(CurrentChestObj.items[CurrentSelectID]);

                }
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                gameObject.SetActive(false);
            }
        }

        public void UpdateItems(Chest chest_object)
        {
            CurrentChestObj = chest_object;
            CurrentSelectID = -1;
        }

        public void ClearItems()
        {
            CurrentChestObj = null;
            CurrentSelectID = -1;
        }

        public void ClickOn(int id)
        {
            CurrentSelectID = id;
        }

        public Chest GetCurrentChest()
        {
            return CurrentChestObj;
        }
    }
}
