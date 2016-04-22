using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class AttackController : NetworkBehaviour
{

    public AttackCollider attackCollider;
    public Weapon currentWeapon;
    private bool isAttacking;

    // Use this for initialization
    void Start()
    {
        isAttacking = false;
        currentWeapon = null;
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
        // create the bullet object from the bullet prefab
        if (!isAttacking)
        {
            isAttacking = true;
            AttackCollider attack;

            if (currentWeapon == null)
            {
                attackCollider.transform.localScale = new Vector3(1, 1, 1);
                attackCollider.damage = 5;
            }
            else
            { 
                attackCollider.transform.localScale = new Vector3(currentWeapon.xRange, currentWeapon.yRange, currentWeapon.zRange);
                attackCollider.damage = currentWeapon.damage;
            }

            attack = (AttackCollider)Instantiate(
                    attackCollider,
                    transform.position - transform.forward * -2,
                    transform.rotation);

            //attack.transform.parent = transform;
            //RpcSyncAttackPostion(attack.transform.localPosition, attack.transform.localRotation, attack.gameObject, attack.transform.parent.gameObject);

            if (currentWeapon == null)
                StartCoroutine(StartAttackCoroutine(2));
            else
                StartCoroutine(StartAttackCoroutine(currentWeapon.attackCooldown));

            NetworkServer.Spawn(attack.gameObject);
            Destroy(attack.gameObject, .1f);
        }
    }

    [ClientRpc]
    public void RpcSyncAttackPostion(Vector3 localPos, Quaternion localRot, GameObject attackCol, GameObject parent)
    {
        Transform test = parent.transform;

        attackCol.transform.parent = parent.transform;
        attackCol.transform.localPosition = localPos;
        attackCol.transform.localRotation = localRot;
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
        currentWeapon = weapon;
    }

    public void DropWeapon()
    {
        currentWeapon = null;
    }
}
