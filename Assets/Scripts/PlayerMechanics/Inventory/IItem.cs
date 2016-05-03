/// <summary>
/// IItem works as a interface to all items that can be picked up and used.
/// </summary>
public interface IItem
{
    void PrimaryUse();
    void SecondaryUse();
    void OnPickup();
    void OnDrop();
    EItemType ItemType();
}
