using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Weapon : NetworkBehaviour, IItem
{

    // Weapons
    public string name;
    public int damage;
    public float attackCooldown;
    public float xRange;
    public float yRange;
    public float zRange;
    public Sprite icon;
    public string description;

    public enum WeaponType
    {
        Melee,
        Ranged
    };

    public WeaponType currentWeaponType;

    public void PrimaryUse(GameObject owner)
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
        CmdMakeInvisible();
    }

    public void OnDrop()
    {
        Destroy(gameObject);
    }

    public EItemType ItemType()
    {
        return EItemType.Weapon;
    }

    [Command]
    public void CmdMakeInvisible()
    {
        if (!isServer) return;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public string GetDescription()
    {
        return description;
    }
}
