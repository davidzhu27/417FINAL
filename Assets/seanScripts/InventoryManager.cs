using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Hotswap")]
    public InventoryItemData[] hotSwapSlots = new InventoryItemData[4];

    [Header("Backpack")]
    public InventoryItemData[] backpackSlots = new InventoryItemData[10];

    public event Action OnInventoryChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool TryAddItemAuto(InventoryItemData itemData)
    {
        if (itemData == null) return false;

        // First try to put item into hotswap.
        for (int i = 0; i < hotSwapSlots.Length; i++)
        {
            if (hotSwapSlots[i] == null)
            {
                hotSwapSlots[i] = itemData;
                Debug.Log($"Added {itemData.itemName} to hotswap slot {i}");
                OnInventoryChanged?.Invoke();
                return true;
            }
        }

        // If hotswap is full, try backpack.
        int startIndex = FindBackpackSpace(itemData.slotSize);

        if (startIndex == -1)
        {
            Debug.Log("Inventory full. Could not add " + itemData.itemName);
            return false;
        }

        for (int i = 0; i < itemData.slotSize; i++)
        {
            backpackSlots[startIndex + i] = itemData;
        }

        Debug.Log($"Added {itemData.itemName} to backpack slot {startIndex}");
        OnInventoryChanged?.Invoke();
        return true;
    }

    private int FindBackpackSpace(int sizeNeeded)
    {
        for (int i = 0; i <= backpackSlots.Length - sizeNeeded; i++)
        {
            bool hasSpace = true;

            for (int j = 0; j < sizeNeeded; j++)
            {
                if (backpackSlots[i + j] != null)
                {
                    hasSpace = false;
                    break;
                }
            }

            if (hasSpace)
            {
                return i;
            }
        }

        return -1;
    }

    public void MoveBackpackItemToHotSwap(int backpackIndex, int hotSwapIndex)
    {
        if (backpackIndex < 0 || backpackIndex >= backpackSlots.Length) return;
        if (hotSwapIndex < 0 || hotSwapIndex >= hotSwapSlots.Length) return;

        InventoryItemData item = backpackSlots[backpackIndex];
        if (item == null) return;

        hotSwapSlots[hotSwapIndex] = item;
        ClearBackpackItem(item);

        Debug.Log($"Moved {item.itemName} to hotswap slot {hotSwapIndex}");
        OnInventoryChanged?.Invoke();
    }

    public void DropHotSwapItem(int hotSwapIndex, Vector3 dropPosition, Quaternion dropRotation)
    {
        if (hotSwapIndex < 0 || hotSwapIndex >= hotSwapSlots.Length) return;

        InventoryItemData item = hotSwapSlots[hotSwapIndex];
        if (item == null || item.worldPrefab == null) return;

        Instantiate(item.worldPrefab, dropPosition, dropRotation);

        hotSwapSlots[hotSwapIndex] = null;

        Debug.Log($"Dropped {item.itemName} back into the world.");
        OnInventoryChanged?.Invoke();
    }

    private void ClearBackpackItem(InventoryItemData item)
    {
        for (int i = 0; i < backpackSlots.Length; i++)
        {
            if (backpackSlots[i] == item)
            {
                backpackSlots[i] = null;
            }
        }
    }
}