using UnityEngine;
using System.Collections;

public class Bandage : MonoBehaviour, IItem
{

    private int healAmount;
    public Sprite icon;

    public void Start()
    {
        healAmount = 50;
    }

    public EItemType ItemType()
    {
        return EItemType.Health;
    }

    public void PrimaryUse(GameObject owner)
    {

    }

    public void SecondaryUse()
    {
        Debug.Log("No Secondary use");
    }

    public void OnPickup(GameObject owner)
    {
        gameObject.transform.parent = owner.transform;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        Health health = owner.GetComponent<Health>();
        health.pickupBandage();
    }

    public void OnDrop()
    {
        Destroy(gameObject);
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public string GetDescription()
    {
        return "Bandage: Recover 30 HP after applying the bandage for 3 seconds";
    }

    public void OnPickupInChest(GameObject owner)
    {
        OnPickup(owner);
    }
}
