using UnityEngine;


public class WeaponTest : MonoBehaviour
{
    public Weapon weaponTest;

    public void OnTriggerEnter(Collider other)
    {
        FirstPersonController test = other.GetComponent<FirstPersonController>();
        //test.ToggleInput();
    }
}

