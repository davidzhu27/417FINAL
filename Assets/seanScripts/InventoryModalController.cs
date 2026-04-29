using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryModalController : MonoBehaviour
{
    [Header("UI Roots")]
    public GameObject inventoryUIRoot;
    public GameObject hotswapPanel;
    public GameObject backpackPanel;

    [Header("Optional Oculus Input Actions")]
    public InputActionReference toggleInventoryAction;
    public InputActionReference toggleBackpackAction;

    private bool inventoryOpen = false;
    private bool backpackOpen = false;

    private void Start()
    {
        SetInventoryOpen(false);
        SetBackpackOpen(false);

        Debug.Log("InventoryModalController started.");
    }

    private void OnEnable()
    {
        if (toggleInventoryAction != null)
        {
            toggleInventoryAction.action.performed += OnToggleInventory;
            toggleInventoryAction.action.Enable();
        }

        if (toggleBackpackAction != null)
        {
            toggleBackpackAction.action.performed += OnToggleBackpack;
            toggleBackpackAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (toggleInventoryAction != null)
        {
            toggleInventoryAction.action.performed -= OnToggleInventory;
        }

        if (toggleBackpackAction != null)
        {
            toggleBackpackAction.action.performed -= OnToggleBackpack;
        }
    }

    private void Update()
    {
        // Keyboard testing first.
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            Debug.Log("Pressed I: toggle hotswap inventory.");
            SetInventoryOpen(!inventoryOpen);
        }

        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            Debug.Log("Pressed B: toggle backpack.");

            if (inventoryOpen)
            {
                SetBackpackOpen(!backpackOpen);
            }
            else
            {
                Debug.Log("Open hotswap inventory first before opening backpack.");
            }
        }
    }

    private void OnToggleInventory(InputAction.CallbackContext context)
    {
        SetInventoryOpen(!inventoryOpen);
    }

    private void OnToggleBackpack(InputAction.CallbackContext context)
    {
        if (inventoryOpen)
        {
            SetBackpackOpen(!backpackOpen);
        }
    }

    public void SetInventoryOpen(bool open)
    {
        inventoryOpen = open;

        if (inventoryUIRoot != null)
        {
            inventoryUIRoot.SetActive(open);
        }
        else
        {
            Debug.LogWarning("InventoryUIRoot is not assigned.");
        }

        if (hotswapPanel != null)
        {
            hotswapPanel.SetActive(open);
        }

        if (!open)
        {
            SetBackpackOpen(false);
        }

        Debug.Log("Inventory open: " + open);
    }

    public void SetBackpackOpen(bool open)
    {
        backpackOpen = open;

        if (backpackPanel != null)
        {
            backpackPanel.SetActive(open);
        }
        else
        {
            Debug.LogWarning("BackpackPanel is not assigned.");
        }

        Debug.Log("Backpack open: " + open);
    }
}