using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class AttackController : NetworkBehaviour
{

    public AttackCollider attackCollider;
    private bool isAttacking;

    // Weapons
    public int damage;
    public float attackCooldown;
    public float xRange;
    public float yRange;
    public float zRange;

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
        // create the bullet object from the bullet prefab
        if (!isAttacking)
        {
            isAttacking = true;
            SetAttackStats();

            AttackCollider attack = (AttackCollider)Instantiate(
                attackCollider,
                transform.position - transform.forward * -2,
                transform.rotation);

            //attack.transform.parent = transform;
            //RpcSyncAttackPostion(attack.transform.localPosition, attack.transform.localRotation, attack.gameObject, attack.transform.parent.gameObject);

            StartCoroutine(StartAttackCoroutine(attackCooldown));

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

    void SetAttackStats()
    {
        attackCollider.transform.localScale = new Vector3(xRange, yRange, zRange);
    }

    public void SetAttackStats(int damage, float xRange, float yRange, float zRange, float attackCooldown)
    {
        attackCollider.transform.localScale = new Vector3(xRange, yRange, zRange);
        this.damage = damage;
        this.attackCooldown = attackCooldown;
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
}
