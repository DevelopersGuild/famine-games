using UnityEngine;
using System.Collections;

public class AttackCollider : MonoBehaviour
{
    public Point owner;
    public int damage;

    void Start()
    {
        owner = GameObject.Find("LOCAL Player").GetComponent<Point>();
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
