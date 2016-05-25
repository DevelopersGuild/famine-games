using UnityEngine;
/// <summary>
/// IItem works as a interface to all items that can be picked up and used.
/// </summary>
public interface IItem
{
    void PrimaryUse(GameObject owner);
    void SecondaryUse();
    void OnPickup(GameObject owner);
    void OnDrop();

    Sprite GetIcon();

    string GetDescription();

    EItemType ItemType();
}
