using TMPro;
using UnityEngine;

public class SimpleInventoryUI : MonoBehaviour
{
    [Header("Hotswap UI")]
    public TMP_Text[] hotSlotTexts;

    [Header("Backpack UI")]
    public TMP_Text backpackText;

    private bool subscribed = false;

    private void OnEnable()
    {
        TrySubscribe();
        RefreshUI();
    }

    private void Start()
    {
        TrySubscribe();
        RefreshUI();
    }

    private void Update()
    {
        // This is not keyboard testing.
        // It only guarantees the UI connects once InventoryManager exists.
        if (!subscribed)
        {
            TrySubscribe();
        }
    }

    private void OnDisable()
    {
        if (InventoryManager.Instance != null && subscribed)
        {
            InventoryManager.Instance.OnInventoryChanged -= RefreshUI;
            subscribed = false;
        }
    }

    private void TrySubscribe()
    {
        if (subscribed) return;
        if (InventoryManager.Instance == null) return;

        InventoryManager.Instance.OnInventoryChanged += RefreshUI;
        subscribed = true;

        Debug.Log("SimpleInventoryUI subscribed to InventoryManager.");
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (InventoryManager.Instance == null)
        {
            return;
        }

        RefreshHotSwap();
        RefreshBackpack();
    }

    private void RefreshHotSwap()
    {
        if (hotSlotTexts == null || hotSlotTexts.Length == 0)
        {
            Debug.LogWarning("HotSlotTexts are not assigned on SimpleInventoryUI.");
            return;
        }

        for (int i = 0; i < hotSlotTexts.Length; i++)
        {
            if (hotSlotTexts[i] == null)
            {
                Debug.LogWarning("HotSlotTexts element " + i + " is not assigned.");
                continue;
            }

            string arrow = i == InventoryManager.Instance.selectedHotSwapIndex ? "> " : "  ";

            if (i < InventoryManager.Instance.hotSwapSlots.Length &&
                InventoryManager.Instance.hotSwapSlots[i] != null)
            {
                hotSlotTexts[i].text = arrow + InventoryManager.Instance.hotSwapSlots[i].itemName;
            }
            else
            {
                hotSlotTexts[i].text = arrow + "Empty";
            }
        }
    }

    private void RefreshBackpack()
    {
        if (backpackText == null)
        {
            Debug.LogWarning("BackpackText is not assigned on SimpleInventoryUI.");
            return;
        }

        string result = "Backpack:\n";

        for (int i = 0; i < InventoryManager.Instance.backpackSlots.Length; i++)
        {
            var item = InventoryManager.Instance.backpackSlots[i];

            string arrow = i == InventoryManager.Instance.selectedBackpackIndex ? "> " : "  ";

            if (item != null)
            {
                result += $"{arrow}Slot {i}: {item.itemName}\n";
            }
            else
            {
                result += $"{arrow}Slot {i}: Empty\n";
            }
        }

        backpackText.text = result;
    }
}