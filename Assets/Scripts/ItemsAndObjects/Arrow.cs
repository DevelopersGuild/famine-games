using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Kroulis.Components;

public class Arrow : NetworkBehaviour {


    [SyncVar]
    public NetworkInstanceId parentNetId;
    public Point owner;
    private int damage;
    private Rigidbody rb;

    public override void OnStartClient()
    {
        GameObject parentObject = ClientScene.FindLocalObject(parentNetId);
        owner = parentObject.GetComponent<Point>();
        rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        //transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        Health health = other.GetComponent<Health>();
        Point target = other.GetComponent<Point>();
        Defense defense = other.GetComponent<Defense>();


        if (health != null && defense != null)
        {

            int armordamage = defense.TakeDamage(damage);
            if (armordamage != -1)
            {
                int healthdamage = damage - armordamage;
                if (health.TakeDamage(healthdamage))
                {
                    owner.AddPoints(10);
                    owner.incKills();
                    target.incDeaths();
                    GameObject.Find("GameCoreProcess").GetComponent<GameProcess>().CmdAddingKillingTab(owner.GetComponent<ContestInfomation>().player_name, target.GetComponent<ContestInfomation>().player_name, 2);
                }
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("IgnoreCollision"))
        {
            return;
        }
        else { 
            Destroy(this.gameObject);
        }
    }

    public void SetDamage(int damageAmount)
    {
        damage = damageAmount;
    }
}
