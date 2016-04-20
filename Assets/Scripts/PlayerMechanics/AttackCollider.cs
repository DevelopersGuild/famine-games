using UnityEngine;
using System.Collections;

public class AttackCollider : MonoBehaviour
{
    public AttackController ac;

    void Start()
    {
        ac = GetComponentInParent<AttackController>();
    }

    void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(ac.damage);
        }
    }
}
