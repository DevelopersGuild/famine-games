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
    public Weapon currentWeapon;
    private Weapon equipped;
    private bool isAttacking;
    private WeaponBarControl wbc;
    public GameObject weaponHolder;
    public Shader overlayShader;
    [SerializeField]
    private Weapon defaultWeapon;

    // Use this for initialization
    void Start()
    {
        isAttacking = false;
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
            if (wbc && currentWeapon.currentWeaponType == Weapon.WeaponType.Melee)
            {
                //Debug.Log(currentWeapon.attackCooldown);
                wbc.StartCooldown(currentWeapon.attackCooldown);
            }
            else if(wbc && currentWeapon.currentWeaponType == Weapon.WeaponType.Ranged)
            {
                
            }
            CmdAttack();
        }
    }

    [Command]
    void CmdAttack()
    {

        if(!currentWeapon || currentWeapon.netId != weaponId)
            currentWeapon = ClientScene.FindLocalObject(weaponId).GetComponent<Weapon>();

        if (currentWeapon.currentWeaponType == Weapon.WeaponType.Ranged)
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
                    transform.position - GetComponentInParent<NetworkedPlayer>().fpsCamera.transform.forward * -2,
                    GetComponentInParent<NetworkedPlayer>().fpsCamera.transform.rotation);

            attack.parentNetId = netId;
            attack.transform.parent = transform;
            attack.damage = currentWeapon.GetAttack();
            attack.transform.localScale = new Vector3(currentWeapon.xRange, currentWeapon.yRange, currentWeapon.zRange);
            Physics.IgnoreCollision(attack.GetComponent<Collider>(), transform.root.GetComponent<Collider>());

            StartCoroutine(StartAttackCoroutine(currentWeapon.attackCooldown));

            NetworkServer.Spawn(attack.gameObject);
            Destroy(attack.gameObject, .1f);

            equipped.animator.SetTrigger("Attack");
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
        if(ClientScene.FindLocalObject(weaponId).GetComponent<Weapon>())
            currentWeapon = ClientScene.FindLocalObject(weaponId).GetComponent<Weapon>();
        else if(ClientScene.FindLocalObject(weaponId).GetComponentInChildren<Weapon>())
            currentWeapon = ClientScene.FindLocalObject(weaponId).GetComponentInChildren<Weapon>();

        // Destroy current equipped weapon
        foreach (Transform child in weaponHolder.transform)
        {
            Destroy(child.gameObject);
        }

        // Instantiate the new weapon
        equipped = (Weapon)Instantiate(currentWeapon);
        Destroy(equipped.GetComponent<Collider>());
        equipped.transform.SetParent(weaponHolder.transform);
        equipped.transform.position = new Vector3(0, 0, 0);
        weaponHolder.transform.localPosition = new Vector3(-1.491f + equipped.positionOffset.x, -2.073f + equipped.positionOffset.y, 0.885f + equipped.positionOffset.z);
        weaponHolder.transform.localEulerAngles = new Vector3(296 + currentWeapon.rotationOffset.x, 353 + currentWeapon.rotationOffset.y, 291 + currentWeapon.rotationOffset.z);
        equipped.GetComponent<Renderer>().material.shader = overlayShader;
    }

    public void DropWeapon()
    {
        currentWeapon = null;
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
        if(currentWeapon != defaultWeapon)
        {
            currentWeapon.CmdMoveToPoint(transform.position);
            currentWeapon.CmdMakeVisible();
            currentWeapon= defaultWeapon;
            weaponId = currentWeapon.netId;
        }
    }

    [Command]
    public void CmdPickupWeaponsInChest(GameObject prefab)
    {
        if (!isServer)
            return;
        GameObject newitem = Instantiate(prefab);
        if (prefab.GetComponent<Weapon>() != null)
            PickedUpWeapon(prefab.GetComponent<Weapon>());
        else if(prefab.GetComponentInChildren<Weapon>()!=null)
            PickedUpWeapon(prefab.GetComponentInChildren<Weapon>());
    }
}
