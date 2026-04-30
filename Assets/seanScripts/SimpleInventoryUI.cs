using TMPro;
using UnityEngine;

public class SimpleInventoryUI : MonoBehaviour
{
    [Header("Hotswap UI")]
    public TMP_Text[] hotSlotTexts;

    [Header("Backpack UI")]
    public TMP_Text backpackText;

    private void OnEnable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged += RefreshUI;
        }

        RefreshUI();
    }

    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -= RefreshUI;
        }
    }

    public void RefreshUI()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogWarning("InventoryManager not found.");
            return;
        }

        RefreshHotSwap();
        RefreshBackpack();
    }

    private void RefreshHotSwap()
    {
        if (hotSlotTexts == null) return;

        for (int i = 0; i < hotSlotTexts.Length; i++)
        {
            if (hotSlotTexts[i] == null) continue;

            if (i < InventoryManager.Instance.hotSwapSlots.Length &&
                InventoryManager.Instance.hotSwapSlots[i] != null)
            {
                hotSlotTexts[i].text = InventoryManager.Instance.hotSwapSlots[i].itemName;
            }
            else
            {
                hotSlotTexts[i].text = "Empty";
            }
        }
    }

    private void RefreshBackpack()
    {
        if (backpackText == null) return;

        string result = "Backpack:\n";

        for (int i = 0; i < InventoryManager.Instance.backpackSlots.Length; i++)
        {
            var item = InventoryManager.Instance.backpackSlots[i];

            if (item != null)
            {
                result += $"Slot {i}: {item.itemName}\n";
            }
            else
            {
                result += $"Slot {i}: Empty\n";
            }
        }

        backpackText.text = result;
    }
}