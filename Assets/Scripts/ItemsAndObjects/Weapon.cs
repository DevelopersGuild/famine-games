using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour, IItem
{

    // Weapons
    public int damage;
    public float attackCooldown;
    public float xRange;
    public float yRange;
    public float zRange;

    public void PrimaryUse()
    {
        throw new System.NotImplementedException();
    }

    public void SecondaryUse()
    {
        throw new System.NotImplementedException();
    }

    public void OnPickup(GameObject player)
    {
        AttackController ac = player.GetComponent<AttackController>();
        ac.PickedUpWeapon(this);
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void OnDrop()
    {
        Destroy(gameObject);
    }

    public EItemType ItemType()
    {
        return EItemType.Weapon;
    }
}
