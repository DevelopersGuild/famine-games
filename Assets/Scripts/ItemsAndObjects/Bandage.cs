using UnityEngine;
using System.Collections;

public class Bandage : MonoBehaviour, IItem
{

    private int healAmount;

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
        Health health = owner.GetComponent<Health>();
        health.Heal(healAmount);
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
    }

    public void OnDrop()
    {
        Destroy(gameObject);
    }

    public Sprite GetIcon()
    {
        return null;
    }

    public string GetDescription()
    {
        return "Bandage: Heal 50% of your health at anytime.";
    }

    public void OnPickupInChest(GameObject owner)
    {
        OnPickup(owner);
    }
}
