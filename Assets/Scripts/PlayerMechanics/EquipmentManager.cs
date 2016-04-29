using UnityEngine;
using System.Collections;

public class EquipmentManager : MonoBehaviour {

    public GameObject mainhand;
    public GameObject helmet;
    public GameObject chestpiece;
    public GameObject offhand;
    //public Vector3 weaponOffset = new Vector3(0, 0, 0);
    public GameObject rightHand;
    public GameObject leftHand;
    public GameObject chest;
    public GameObject head;


    public bool equipMainHand = false;
    public bool equipOffHand = false;
    public bool equipHelmet = false;
    public bool equipChest = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (equipMainHand)
        {
            equipItem(mainhand, "Mainhand");
            equipMainHand = false;
        }

        if (equipOffHand) {
            equipItem(offhand, "Offhand");
            equipOffHand = false;
  
        }
        if (equipHelmet)
        {
            equipItem(helmet, "Head");
            equipHelmet = false;
        }
        if (equipChest)
        {
            equipItem(chestpiece, "Chest");
            equipChest = false;
        }


	}

    
    void equipItem(GameObject item, string itemSlot)
    {
        Debug.Log("Equipping weapon");
        GameObject bone;
        if (itemSlot == "Mainhand")
        {
            bone = rightHand;
        } else if (itemSlot == "Offhand")
        {
            bone = leftHand;
        } else if (itemSlot == "Head")
        {
            bone = head;
        } else if (itemSlot == "Chest")
        {
            bone = chest;
        }
        else
        {
            Debug.Log("Could not recognize itemslot: [" + itemSlot + "]  Please check case sensitivity");
            Debug.Log("Current supported itemslots include: [Mainhand] [Offhand] [Head] [Chest]");
            return;
        }

        item.transform.parent = bone.transform;
        item.transform.position = bone.transform.position;


    }
}
