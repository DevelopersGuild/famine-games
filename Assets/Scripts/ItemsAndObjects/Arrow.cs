using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

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
        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    void OnTriggerEnter(Collider other)
    {
        //Health health = other.GetComponent<Health>();
        //if (health != null)
        //{
        //    if (health.TakeDamage(damage))
        //        owner.AddPoints(10);
        //}
        //else
        //    Destroy(this.gameObject);

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
                }
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SetDamage(int damageAmount)
    {
        damage = damageAmount;
    }
}
