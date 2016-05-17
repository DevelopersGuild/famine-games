using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Kroulis.Components;

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
                    GameObject.Find("GameCoreProcess").GetComponent<GameProcess>().CmdAddingKillingTab(owner.GetComponent<ContestInfomation>().player_name, target.GetComponent<ContestInfomation>().player_name, 1);
                }
            }

        }
    }
}
