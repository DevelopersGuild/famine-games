using UnityEngine;


public class WeaponTest : MonoBehaviour
{

    public Weapon weaponTest;



    public void OnTriggerEnter(Collider other)
    {
        AttackController ac = other.GetComponent<AttackController>();
        if (ac != null)
        {
            ac.PickedUpWeapon(weaponTest);
        }
    }
}

