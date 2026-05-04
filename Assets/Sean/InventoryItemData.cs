using UnityEngine;

[CreateAssetMenu(fileName = "NewInventoryItemData", menuName = "Inventory Item Data")]
public class InventoryItemData : ScriptableObject
{
    public string itemName;
    public GameObject worldPrefab;
    public GameObject inventoryPrefab;
    public int slotSize = 1;
}