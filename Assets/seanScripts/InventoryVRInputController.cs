using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryVRInputController : MonoBehaviour
{
    [Header("References")]
    public InventoryModalController modalController;
    public Transform dropPoint;

    [Header("Left Hand Inputs")]
    public InputActionReference leftPrimaryAction;
    public InputActionReference leftSecondaryAction;

    [Header("Right Hand Inputs")]
    public InputActionReference rightTriggerAction;
    public InputActionReference rightSecondaryAction; // New: use this for swapping

    [Header("Drop Settings")]
    public float dropHoldTime = 0.8f;

    private float rightTriggerPressedTime = -1f;
    private bool rightTriggerHolding = false;

    private void OnEnable()
    {
        EnableAction(leftPrimaryAction);
        EnableAction(leftSecondaryAction);
        EnableAction(rightTriggerAction);
        EnableAction(rightSecondaryAction);
    }

    private void Update()
    {
        if (InventoryManager.Instance == null) return;

        HandleLeftSecondary();
        HandleLeftPrimary();
        HandleRightSecondarySwap();
        HandleRightTriggerDrop();
    }

    private void EnableAction(InputActionReference actionRef)
    {
        if (actionRef != null)
        {
            actionRef.action.Enable();
        }
    }

    private bool WasPressedThisFrame(InputActionReference actionRef)
    {
        return actionRef != null && actionRef.action.WasPressedThisFrame();
    }

    private bool WasReleasedThisFrame(InputActionReference actionRef)
    {
        return actionRef != null && actionRef.action.WasReleasedThisFrame();
    }

    private void HandleLeftSecondary()
    {
        if (WasPressedThisFrame(leftSecondaryAction))
        {
            if (modalController != null)
            {
                modalController.ToggleBackpack();
            }
        }
    }

    private void HandleLeftPrimary()
    {
        if (WasPressedThisFrame(leftPrimaryAction))
        {
            bool bagOpen = modalController != null && modalController.IsBackpackOpen;

            if (bagOpen)
            {
                InventoryManager.Instance.SelectNextBackpackSlot();
            }
            else
            {
                InventoryManager.Instance.SelectNextHotSwapSlot();
            }
        }
    }

    private void HandleRightSecondarySwap()
    {
        if (WasPressedThisFrame(rightSecondaryAction))
        {
            bool bagOpen = modalController != null && modalController.IsBackpackOpen;

            if (bagOpen)
            {
                InventoryManager.Instance.SwapSelectedBackpackAndHotSwap();
            }
            else
            {
                Debug.Log("Open backpack first before swapping.");
            }
        }
    }

    private void HandleRightTriggerDrop()
    {
        if (WasPressedThisFrame(rightTriggerAction))
        {
            rightTriggerPressedTime = Time.time;
            rightTriggerHolding = true;
        }

        if (rightTriggerHolding && WasReleasedThisFrame(rightTriggerAction))
        {
            float heldTime = Time.time - rightTriggerPressedTime;

            if (heldTime >= dropHoldTime)
            {
                DropSelectedItem();
            }

            rightTriggerHolding = false;
            rightTriggerPressedTime = -1f;
        }
    }

    private void DropSelectedItem()
    {
        Vector3 position;
        Quaternion rotation;

        if (dropPoint != null)
        {
            position = dropPoint.position;
            rotation = dropPoint.rotation;
        }
        else
        {
            position = transform.position + transform.forward * 1.0f;
            rotation = transform.rotation;
        }

        bool bagOpen = modalController != null && modalController.IsBackpackOpen;

        if (bagOpen)
        {
            InventoryManager.Instance.DropSelectedBackpackItem(position, rotation);
        }
        else
        {
            InventoryManager.Instance.DropSelectedHotSwapItem(position, rotation);
        }
    }
}