using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class Ammo : NetworkBehaviour, IItem
{
    private int ammoAmount;

    public void Start()
    {
        ammoAmount = UnityEngine.Random.Range(0, 9) + 1;
    }

    public EItemType ItemType()
    {
        return EItemType.Ammo;
    }

    public void OnDrop()
    {
        throw new NotImplementedException();
    }

    public void OnPickup(GameObject owner)
    {
        BowAndArrow bow = owner.GetComponent<BowAndArrow>();
        bow.pickupAmmo(ammoAmount);
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
    public void OnPickupInChest(GameObject owner)
    {
        OnPickup(owner);
    }

    public void PrimaryUse(GameObject owner)
    {
        return;
    }

    public void SecondaryUse()
    {
        return;
    }

    public Sprite GetIcon() 
    {
        return null;
    }

    public string GetDescription()
    {
        return "Ammo for Bow. After picking up, you can get ramdomly 1~10 arrows.";
    }


}
