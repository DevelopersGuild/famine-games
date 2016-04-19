using UnityEngine;
using System.Collections;

public class AttackCollider : MonoBehaviour
{

    public int damage;

    void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }
}
