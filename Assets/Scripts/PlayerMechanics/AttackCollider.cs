using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class AttackCollider : NetworkBehaviour
{
    public Point owner;
    public int damage;
    [SyncVar]
    public NetworkInstanceId parentNetId;

    public override void OnStartClient()
    {
        GameObject parentObject = ClientScene.FindLocalObject(parentNetId);
        owner = parentObject.GetComponent<Point>();
        transform.SetParent(parentObject.transform);
    }

    void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();
        Defense defense = other.GetComponent<Defense>();
        if (health != null && defense!= null)
        {
            //if (health.TakeDamage(damage))
            //{ 
            //        owner.AddPoints(10);
            //};
            int amrordamage=defense.TakeDamage(damage);
            if(amrordamage!=-1)
            {
                int healthdamage = damage - amrordamage;
                if(health.TakeDamage(healthdamage))
                {
                    owner.AddPoints(10);
                }
            }
            
        }
    }
}
