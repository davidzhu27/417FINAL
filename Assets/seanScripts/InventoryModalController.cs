using UnityEngine;

public class InventoryModalController : MonoBehaviour
{
    [Header("UI Roots")]
    public GameObject inventoryUIRoot;
    public GameObject hotswapPanel;
    public GameObject backpackPanel;

    public bool IsBackpackOpen { get; private set; }

    private void Start()
    {
        if (inventoryUIRoot != null)
        {
            inventoryUIRoot.SetActive(true);
        }

        if (hotswapPanel != null)
        {
            hotswapPanel.SetActive(true);
        }

        SetBackpackOpen(false);

        Debug.Log("Inventory UI started. Hotswap is always visible.");
    }

    public void ToggleBackpack()
    {
        SetBackpackOpen(!IsBackpackOpen);
    }

    public void SetBackpackOpen(bool open)
    {
        IsBackpackOpen = open;

        if (backpackPanel != null)
        {
            backpackPanel.SetActive(open);
        }

        Debug.Log("Backpack open: " + open);
    }
}