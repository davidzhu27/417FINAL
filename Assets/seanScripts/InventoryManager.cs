using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Hotswap")]
    public InventoryItemData[] hotSwapSlots = new InventoryItemData[4];

    [Header("Backpack")]
    public InventoryItemData[] backpackSlots = new InventoryItemData[10];

    [Header("Selection")]
    public int selectedHotSwapIndex = 0;
    public int selectedBackpackIndex = 0;

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

            if (hasSpace) return i;
        }

        return -1;
    }

    public void SelectNextHotSwapSlot()
    {
        selectedHotSwapIndex++;

        if (selectedHotSwapIndex >= hotSwapSlots.Length)
        {
            selectedHotSwapIndex = 0;
        }

        Debug.Log("Selected hotswap slot " + selectedHotSwapIndex);
        OnInventoryChanged?.Invoke();
    }

    public void SelectNextBackpackSlot()
    {
        selectedBackpackIndex++;

        if (selectedBackpackIndex >= backpackSlots.Length)
        {
            selectedBackpackIndex = 0;
        }

        Debug.Log("Selected backpack slot " + selectedBackpackIndex);
        OnInventoryChanged?.Invoke();
    }

    public InventoryItemData GetSelectedHotSwapItem()
    {
        if (selectedHotSwapIndex < 0 || selectedHotSwapIndex >= hotSwapSlots.Length) return null;
        return hotSwapSlots[selectedHotSwapIndex];
    }

    public InventoryItemData GetSelectedBackpackItem()
    {
        if (selectedBackpackIndex < 0 || selectedBackpackIndex >= backpackSlots.Length) return null;
        return backpackSlots[selectedBackpackIndex];
    }

    public void SwapSelectedBackpackAndHotSwap()
    {
        if (selectedHotSwapIndex < 0 || selectedHotSwapIndex >= hotSwapSlots.Length) return;
        if (selectedBackpackIndex < 0 || selectedBackpackIndex >= backpackSlots.Length) return;

        InventoryItemData hotItem = hotSwapSlots[selectedHotSwapIndex];
        InventoryItemData bagItem = backpackSlots[selectedBackpackIndex];

        hotSwapSlots[selectedHotSwapIndex] = bagItem;
        backpackSlots[selectedBackpackIndex] = hotItem;

        Debug.Log($"Swapped hotswap slot {selectedHotSwapIndex} with backpack slot {selectedBackpackIndex}");
        OnInventoryChanged?.Invoke();
    }

    public void DropSelectedHotSwapItem(Vector3 dropPosition, Quaternion dropRotation)
    {
        DropHotSwapItem(selectedHotSwapIndex, dropPosition, dropRotation);
    }

    public void DropSelectedBackpackItem(Vector3 dropPosition, Quaternion dropRotation)
    {
        DropBackpackItem(selectedBackpackIndex, dropPosition, dropRotation);
    }

    public void DropHotSwapItem(int hotSwapIndex, Vector3 dropPosition, Quaternion dropRotation)
    {
        if (hotSwapIndex < 0 || hotSwapIndex >= hotSwapSlots.Length) return;

        InventoryItemData item = hotSwapSlots[hotSwapIndex];

        if (item == null)
        {
            Debug.Log("No hotswap item selected to drop.");
            return;
        }

        if (item.worldPrefab == null)
        {
            Debug.LogWarning(item.itemName + " has no worldPrefab assigned.");
            return;
        }

        Instantiate(item.worldPrefab, dropPosition, dropRotation);

        hotSwapSlots[hotSwapIndex] = null;

        Debug.Log($"Dropped {item.itemName} from hotswap into the world.");
        OnInventoryChanged?.Invoke();
    }

    public void DropBackpackItem(int backpackIndex, Vector3 dropPosition, Quaternion dropRotation)
    {
        if (backpackIndex < 0 || backpackIndex >= backpackSlots.Length) return;

        InventoryItemData item = backpackSlots[backpackIndex];

        if (item == null)
        {
            Debug.Log("No backpack item selected to drop.");
            return;
        }

        if (item.worldPrefab == null)
        {
            Debug.LogWarning(item.itemName + " has no worldPrefab assigned.");
            return;
        }

        Instantiate(item.worldPrefab, dropPosition, dropRotation);

        backpackSlots[backpackIndex] = null;

        Debug.Log($"Dropped {item.itemName} from backpack into the world.");
        OnInventoryChanged?.Invoke();
    }
}