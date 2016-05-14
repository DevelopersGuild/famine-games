using UnityEngine;

/// <summary>
/// Contains all of the items a player is carrying and handles using the items.
/// </summary>
public class Inventory : MonoBehaviour
{
    private static int inventorySize = 6;
    private IItem[] inventory;
    private IItem selectedItem;

    void Start()
    {
        inventory = new IItem[inventorySize];
        SelectItem(EItemType.None);
    }

    /// <summary>
    /// Drops the current Item from the inventory and calls the OnPickup() function on the item then stores
    /// it in the inventory.
    /// </summary>
    /// <param name="item"></param>
    public void PickupItem(IItem item)
    {
        DropItem(item);
        item.OnPickup(gameObject);
        inventory[(int)item.ItemType()] = item;
    }

    /// <summary>
    /// Drops the item from the spot on the item that is passed in making room for the new item.
    /// </summary>
    /// <param name="itemToDrop"></param>
    public void DropItem(IItem itemToDrop)
    {
        //TODO Add ability to spawn current Item.
    }

    /// <summary>
    /// Switch the players item to the selected one.
    /// </summary>
    /// <param name="item"></param>
    public void SelectItem(EItemType item)
    {
        if(inventory[(int)item] != null)
        {
            selectedItem = inventory[(int)item];
        }
    }

    /// <summary>
    /// Calls the interface method for PrimaryUse() on whichever item is currently selected.
    /// </summary>
    public void UseItemPrimary()
    {
        selectedItem.PrimaryUse(gameObject);
    }

    /// <summary>
    /// Calls the interface method for SecondaryUse() on whichever item is currently selected.
    /// </summary>
    public void UseItemSecondary()
    {
        selectedItem.SecondaryUse();
    }

    /// <summary>
    /// Called on the player respawn to remove all items they have.
    /// </summary>
    public void LoseItems()
    {
        for(int i = 1; i < inventory.Length; i++)
        {
            inventory[i] = null;
        }
    }

}
