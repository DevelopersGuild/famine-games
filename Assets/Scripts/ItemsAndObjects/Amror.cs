using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Amror : NetworkBehaviour, IItem
{
    // Amrors
    public string name;
    public int[] defense;
    [SyncVar]
    public int CurrentLevel;
    public int maxDurability;
    public Sprite icon;
    public string description;
    //public Sprite icon;

    void Awake()
    {
        if(isServer)
        {
            CmdRandomAmrorLevel();
        }
    }

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
        CmdMakeInvisible();
    }


    [Command]
    public void CmdMakeInvisible()
    {
        if (!isServer) return;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = false;
    }

    [Command]
    public void CmdMakeVisible()
    {
        if (!isServer)
            return;
        gameObject.GetComponent<Collider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    public void CmdMoveToPoint(Vector3 target)
    {
        if (!isServer)
            return;
        gameObject.transform.position.Set(target.x, target.y, target.z);
    }

    public void OnDrop()
    {
        Destroy(gameObject);
    }

    public EItemType ItemType()
    {
        return EItemType.Amror;
    }

    public Sprite GetIcon() 
    {
        return icon;
    }

    public string GetDescription()
    {
        return description;
    }

    [Command]
    public void CmdSetMaxDurability(int amount)
    {
        if (!isServer)
            return;
        maxDurability = amount;
    }

    [Command]
    public void CmdRandomAmrorLevel()
    {
        CurrentLevel = Random.Range(0, defense.Length - 1);
    }

    public int GetDefense()
    {
        return defense[CurrentLevel];
    }
}
