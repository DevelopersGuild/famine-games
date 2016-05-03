using UnityEngine;
using System.Collections;

public class Bandage : MonoBehaviour, IItem
{
    public EItemType ItemType()
    {
        return EItemType.Health;
    }

    public void PrimaryUse()
    {
        Debug.Log("Added Health");
    }

    public void SecondaryUse()
    {
        Debug.Log("No Secondary use");
    }

    public void OnPickup()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void OnDrop()
    {
        Destroy(gameObject);
    }
}
