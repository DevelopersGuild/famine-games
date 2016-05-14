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
        Point target = other.GetComponent<Point>();
        if (health != null)
        {
            if (health.TakeDamage(damage))
            {
                owner.AddPoints(10);
                owner.incKills();
                target.incDeaths();
            }
        }
    }
}
