using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Weapon : NetworkBehaviour, IItem
{

    // Weapons
    public string name;
    public int[] damage=new int[1];
    [SyncVar]
    public int CurrentLevel;
    public float attackCooldown;
    public float xRange;
    public float yRange;
    public float zRange;
    public Sprite icon;
    public string description;
    

    // Offsets for when the weapon is equipped
    public Vector3 positionOffset;
    public Vector3 rotationOffset;

    public Animator animator;

    public enum WeaponType
    {
        Melee,
        Ranged
    };

    public WeaponType currentWeaponType;

    void Awake()
    {
        if(isServer)
        {

        }
    }

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PrimaryUse(GameObject owner)
    {
        //throw new System.NotImplementedException();
        return;
    }

    public void SecondaryUse()
    {
        //throw new System.NotImplementedException();
        return;
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

    [Command]
    public void CmdMakeVisible()
    {
        if (!isServer)
            return;
        gameObject.GetComponent<Collider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    [Command]
    public void CmdMoveToPoint(Vector3 target)
    {
        if (!isServer)
            return;
        gameObject.transform.position.Set(target.x, target.y, target.z);
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
    public void CmdRandomWeaponLevel()
    {
        CurrentLevel = Random.Range(0, damage.Length - 1);
    }

    public int GetAttack()
    {
        return damage[CurrentLevel];
    }
}
