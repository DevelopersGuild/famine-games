using UnityEngine;
using System.Collections;

/// <summary>
/// Handles all of the the intereaction between picking up a item or interacting with a trigger or button.
/// </summary>
public class Interact : MonoBehaviour
{
    public float ArmReach = 1f;
    private LayerMask layerMask;
    private Camera playerCamera;
    private Inventory inventory;
    public float timeToHeal;
    private float currentHealTimer;
    public bool canHeal;
    private Health health;
    private FirstPersonController fpc;

    void Start()
    {
        layerMask = 1 << 8;
        playerCamera = gameObject.GetComponentInChildren<Camera>();
        inventory = gameObject.GetComponent<Inventory>();
        health = GetComponent<Health>();
        fpc = GetComponent<FirstPersonController>();

        canHeal = true;
    }

    void Update()
    {
        //+++++++++++ Should probably be moved to the player script
        if (Input.GetButtonDown("Use"))
        {
            GameObject item = InteractWithObject();
            if (item != null)
            {
                if(item.tag == "Item")
                {
                    inventory.PickupItem(item.GetComponent<IItem>());
                }
                else if (item.tag == "Interactive")
                {
                    item.GetComponent<IInteractive>().InteractWith();
                }
            }
        }

        //+++++++++++ Should probably be moved to the player script
        if(Input.GetButton("Heal") && health.bandagesAmount > 0 && canHeal && !fpc.m_isRunning)
        {
            currentHealTimer += Time.deltaTime;
            Debug.Log(currentHealTimer);
            if(currentHealTimer > timeToHeal)
            {
                GetComponent<Health>().UseBandage(30);
                currentHealTimer = 0;
                canHeal = false;
            }
        }
        else
        {
            currentHealTimer = 0;
        }

        if (Input.GetButtonUp("Heal"))
        {
            canHeal = true;
        }

        //+++++++++++ Needs to be moved to the player script
        //TODO Move To Players Script
            if (Input.GetButtonDown("SelectItem1"))
        {
            inventory.SelectItem(EItemType.None);
        }
        if (Input.GetButtonDown("SelectItem2"))
        {
            inventory.SelectItem(EItemType.Weapon);
        }
        if (Input.GetButtonDown("SelectItem3"))
        {
            inventory.SelectItem(EItemType.Health);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            inventory.UseItemPrimary();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            inventory.UseItemSecondary();
        }
        //+++++++++++ Needs to be moved to the player script



    }


    /// <summary>
    /// Performs a check to see if a Item is on the correct layer and is with in the players arm reach.
    /// </summary>
    /// <returns>The game object that was hit or null if it did not hit a game object</returns>
    public GameObject InteractWithObject()
    {
        RaycastHit hit;
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * ArmReach);
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, ArmReach, layerMask))
        {
            Debug.Log("Hit Object " + hit);
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }
}
