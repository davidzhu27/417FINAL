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
    public InputActionReference leftTriggerAction;

    [Header("Right Hand Inputs")]
    public InputActionReference rightTriggerAction;

    [Header("Drop Settings")]
    public float dropHoldTime = 0.8f;

    private float rightTriggerPressedTime = -1f;
    private bool rightTriggerHolding = false;

    private void OnEnable()
    {
        EnableAction(leftPrimaryAction);
        EnableAction(leftSecondaryAction);
        EnableAction(leftTriggerAction);
        EnableAction(rightTriggerAction);
    }

    private void Update()
    {
        if (InventoryManager.Instance == null) return;

        HandleLeftSecondary();
        HandleLeftPrimary();
        HandleLeftTrigger();
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

    private bool IsPressed(InputActionReference actionRef)
    {
        return actionRef != null && actionRef.action.IsPressed();
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

    private void HandleLeftTrigger()
    {
        if (WasPressedThisFrame(leftTriggerAction))
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