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
                tips.text = CurrentChestObj.items[CurrentSelectID].GetComponent<IItem>().GetDescription();
                tips.text += "\n Press F to pick up current Item.";
            }
            if(Input.GetKeyDown(KeyCode.F))
            {
                if(CurrentChestObj && CurrentSelectID!=-1 && CurrentSelectID<CurrentChestObj.items.Count)
                {
                    CurrentChestObj.items[CurrentSelectID].GetComponent<IItem>().OnPickup(GameObject.Find("LOCAL Player"));
                    CurrentChestObj.removeItem(CurrentChestObj.items[CurrentSelectID]);
                }
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
