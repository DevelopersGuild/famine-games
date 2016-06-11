using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kroulis.UI.MainGame
{
    public class LoopButton : MonoBehaviour
    {

        public int id;
        private LootBoxFullControl lbfc;
        private Image icon;
        // Use this for initialization
        void Start()
        {
            Button btn = GetComponent<Button>();
            icon = GetComponent<Image>();
            btn.onClick.AddListener(delegate() { this.Action(); });
        }

        // Update is called once per frame
        void Update()
        {
            if(!lbfc)
            {
                lbfc = GetComponentInParent<LootBoxFullControl>();
            }
            else
            {
                Chest chestobj=lbfc.GetCurrentChest();
                if(chestobj.items.Count>id)
                {
                    if(chestobj.items[id].GetComponent<IItem>()!=null)
                        icon.sprite = chestobj.items[id].GetComponent<IItem>().GetIcon();
                    else if(chestobj.items[id].GetComponentInChildren<IItem>() != null)
                        icon.sprite = chestobj.items[id].GetComponentInChildren<IItem>().GetIcon();
                }
                else
                {
                    icon.sprite = null;
                }
            }
        }

        void Action()
        {
            GetComponentInParent<LootBoxFullControl>().ClickOn(id);
        }
    }
}