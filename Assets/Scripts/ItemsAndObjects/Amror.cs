using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Amror : NetworkBehaviour, IItem
{
    // Amrors
    public string name;
    public int defense;
    public int maxDurability;
    //public Sprite icon;

    public enum AmrorType
    {
        BodyAmror,Helmet
    };

    public AmrorType currentAmrorType;

    public void PrimaryUse(GameObject owner)
    {
        return;
    }

    public void SecondaryUse()
    {
        return;
    }

    public void OnPickup(GameObject player)
    {
        Defense df = player.GetComponent<Defense>();
        df.PickedUpAmror(this);
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void OnDrop()
    {
        Destroy(gameObject);
    }

    public EItemType ItemType()
    {
        return EItemType.Amror;
    }

}
