using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
    public Weapon weaponTest;


    void OnTriggerEnter(Collider other)
    {
        AttackController ac = other.GetComponent<AttackController>();
        if (ac != null)
        {
            ac.RpcPickedUpWeapon(weaponTest);
        }
    }
}
