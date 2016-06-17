using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.Networking;
using Kroulis.UI.MainGame;

public class AttackController : NetworkBehaviour
{

    public AttackCollider attackCollider;
    [SyncVar]
    public NetworkInstanceId weaponId;
    //public Weapon currentWeapon;
    public Weapon equipped;
    private bool isAttacking;
    private WeaponBarControl wbc;
    public GameObject weaponHolder;
    public Weapon holdObject;
    public Shader overlayShader;
    [SerializeField]
    private Weapon defaultWeapon;

    // Use this for initialization
    void Start()
    {
        isAttacking = false;
        PickedUpWeapon(defaultWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;
        if(!wbc)
        {
            wbc = GameObject.Find("Main_UI").GetComponentInChildren<WeaponBarControl>();
        }

        if (GetComponent<FirstPersonController>().GetInput() && Input.GetMouseButtonDown(0)) 
        {
            if (wbc && equipped.currentWeaponType == Weapon.WeaponType.Melee)
            {
                //Debug.Log(currentWeapon.attackCooldown);
                wbc.StartCooldown(equipped.attackCooldown);
            }
            else if (wbc && equipped.currentWeaponType == Weapon.WeaponType.Ranged)
            {
                
            }
            CmdAttack();
        }
    }

    [Command]
    void CmdAttack()
    {

        if (!equipped || equipped.netId != weaponId)
        {
            //Debug.Log()
            if (ClientScene.FindLocalObject(weaponId).GetComponent<Weapon>())
                equipped = ClientScene.FindLocalObject(weaponId).GetComponent<Weapon>();
            else
                equipped = ClientScene.FindLocalObject(weaponId).GetComponentInChildren<Weapon>();
        }


        if (equipped.currentWeaponType == Weapon.WeaponType.Ranged)
            return;

          // create the bullet object from the bullet prefab
        if (!isAttacking)
        {
            isAttacking = true;
            AttackCollider attack;

            //attackCollider.transform.localScale = new Vector3(currentWeapon.xRange, currentWeapon.yRange,currentWeapon.zRange);
            //attackCollider.damage = currentWeapon.damage;


            /*attack = (AttackCollider)Instantiate(
                    attackCollider,
                    transform.position - Camera.main.transform.forward * -2,
                    Camera.main.transform.rotation);*/
            attack = (AttackCollider)Instantiate(
                    attackCollider,
                    transform.position + GetComponentInParent<NetworkedPlayer>().fpsCamera.transform.forward * equipped.yRange,
                    GetComponentInParent<NetworkedPlayer>().fpsCamera.transform.rotation);

            attack.parentNetId = netId;
            attack.transform.parent = transform;
            attack.damage = equipped.GetAttack();
            attack.transform.localScale = new Vector3(equipped.xRange, equipped.yRange, equipped.zRange);
            Physics.IgnoreCollision(attack.GetComponent<Collider>(), transform.root.GetComponent<Collider>());

            StartCoroutine(StartAttackCoroutine(equipped.attackCooldown));

            NetworkServer.Spawn(attack.gameObject);
            Destroy(attack.gameObject, 1f);

            equipped.animator.SetTrigger("Attack");
            holdObject.animator.SetTrigger("Attack");
        }
    }

    IEnumerator StartAttackCoroutine(float attackCooldown)
    {
        yield return new WaitForSeconds(attackCooldown);
        FinishedAttack();
    }

    public void FinishedAttack()
    {
        isAttacking = false;
    }


    public void PickedUpWeapon(Weapon weapon)
    {
        //weaponId = weapon.netId;
        CmdUpdateWeapon(weapon.netId);
        if (weapon != defaultWeapon)
        {
            if (ClientScene.FindLocalObject(weaponId).GetComponent<Weapon>())
                equipped = ClientScene.FindLocalObject(weaponId).GetComponent<Weapon>();
            else if (ClientScene.FindLocalObject(weaponId).GetComponentInChildren<Weapon>())
                equipped = ClientScene.FindLocalObject(weaponId).GetComponentInChildren<Weapon>();
        }

        // Destroy current equipped weapon
        foreach (Transform child in weaponHolder.transform)
        {
            Destroy(child.gameObject);
        }

        // Instantiate the new weapon
        CmdInstantiateNewWeapon(weapon.gameObject);

        CreateWeaponHold();
    }

    public void CreateWeaponHold()
    {
        holdObject = (Weapon) Instantiate(equipped);

        Destroy(holdObject.GetComponent<Collider>());
        holdObject.transform.SetParent(weaponHolder.transform);
        holdObject.transform.position = new Vector3(0, 0, 0);
        weaponHolder.transform.localPosition = new Vector3(-1.491f + holdObject.positionOffset.x, -2.073f + holdObject.positionOffset.y, 0.885f + holdObject.positionOffset.z);
        weaponHolder.transform.localEulerAngles = new Vector3(296 + holdObject.rotationOffset.x, 353 + holdObject.rotationOffset.y, 291 + holdObject.rotationOffset.z);
        holdObject.GetComponent<Renderer>().material.shader = overlayShader;
    }

    public void DropWeapon()
    {
        equipped = null;
    }

    public bool GetAttackingStatus()
    {
        return isAttacking;
    }

    [Command]
    public void CmdUpdateWeapon(NetworkInstanceId wid)
    {
        if (!isServer)
            return;
        weaponId = wid;
    }

    [Command]
    public void CmdDeadWeaponDrop()
    {
        if (equipped != defaultWeapon)
        {
            equipped.CmdMoveToPoint(transform.position);
            equipped.CmdMakeVisible();
            equipped = defaultWeapon;
            weaponId = equipped.netId;
        }
    }

    [Command]
    public void CmdPickupWeaponsInChest(GameObject prefab)
    {
        if (!isServer)
            return;
        /*GameObject newitem = Instantiate(prefab);
        if (prefab.GetComponent<Weapon>() != null)
            PickedUpWeapon(prefab.GetComponent<Weapon>());
        else if(prefab.GetComponentInChildren<Weapon>()!=null)
            PickedUpWeapon(prefab.GetComponentInChildren<Weapon>());*/
        CmdInstantiateNewWeapon(prefab);
        CmdUpdateWeapon(equipped.netId);
    }

    [Command]
    public void CmdInstantiateNewWeapon(GameObject prefab)
    {
        if (!isServer)
            return;
        GameObject newobject=Instantiate(prefab);
        if (newobject.GetComponent<Weapon>())
            equipped = newobject.GetComponent<Weapon>();
        else
            equipped = newobject.GetComponentInChildren<Weapon>();
        Destroy(equipped.GetComponent<Collider>());
        equipped.transform.SetParent(weaponHolder.transform);
        equipped.transform.position = new Vector3(0, 0, 0);
        weaponHolder.transform.localPosition = new Vector3(-1.491f + equipped.positionOffset.x, -2.073f + equipped.positionOffset.y, 0.885f + equipped.positionOffset.z);
        weaponHolder.transform.localEulerAngles = new Vector3(296 + equipped.rotationOffset.x, 353 + equipped.rotationOffset.y, 291 + equipped.rotationOffset.z);
        equipped.GetComponent<Renderer>().material.shader = overlayShader;
    }
}
