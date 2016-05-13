using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.Networking;
using Kroulis.UI.MainGame;

public class AttackController : NetworkBehaviour
{

    public AttackCollider attackCollider;
    public Weapon currentWeapon;
    private bool isAttacking;


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

        if (Input.GetMouseButtonDown(0)) 
        {
            CmdAttack();
        }
    }

    [Command]
    void CmdAttack()
    {
        if (currentWeapon.currentWeaponType == Weapon.WeaponType.Ranged)
            return;

          // create the bullet object from the bullet prefab
        if (!isAttacking)
        {
            isAttacking = true;
            AttackCollider attack;

            attackCollider.transform.localScale = new Vector3(currentWeapon.xRange, currentWeapon.yRange,currentWeapon.zRange);
            attackCollider.damage = currentWeapon.damage;


            attack = (AttackCollider)Instantiate(
                    attackCollider,
                    transform.position - transform.forward * -2,
                    transform.rotation);

            attack.parentNetId = netId;
            attack.transform.parent = transform;
            Physics.IgnoreCollision(attack.GetComponent<Collider>(), transform.root.GetComponent<Collider>());

            StartCoroutine(StartAttackCoroutine(currentWeapon.attackCooldown));

            NetworkServer.Spawn(attack.gameObject);
            Destroy(attack.gameObject, .1f);
        }
    }

    IEnumerator StartAttackCoroutine(float attackCooldown)
    {
        WeaponBarControl wbc = GameObject.Find("Main_UI").GetComponentInChildren<WeaponBarControl>();
        if(wbc)
        {
            wbc.StartCooldown(attackCooldown);
        }
        yield return new WaitForSeconds(attackCooldown);
        FinishedAttack();
    }

    public void FinishedAttack()
    {
        isAttacking = false;
    }


    public void PickedUpWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
    }

    public void DropWeapon()
    {
        currentWeapon = null;
    }

    public bool GetAttackingStatus()
    {
        return isAttacking;
    }
}
