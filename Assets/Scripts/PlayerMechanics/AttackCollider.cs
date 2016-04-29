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
        transform.SetParent(parentObject.transform);
        owner = parentObject.GetComponent<Point>();
    }

    void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            if (health.TakeDamage(damage))
            { 
                    owner.AddPoints(10);
            };
        }
    }
}
