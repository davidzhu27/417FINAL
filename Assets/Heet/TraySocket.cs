using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TraySocket : MonoBehaviour
{
    public FoodSlotType acceptedType;
    public TrayMealTracker trayMealTracker;
    public GameObject socketVisual;
    public float snapDistance = 0.12f;

    private FoodItemData currentItem;
    private FoodItemData hoveringValidItem;

    private void Start()
    {
        if (socketVisual != null)
            socketVisual.SetActive(false);
    }

    private void Update()
    {
        if (currentItem != null) return;
        if (hoveringValidItem == null) return;

        XRGrabInteractable grab = hoveringValidItem.GetComponent<XRGrabInteractable>();
        if (grab == null) return;

        bool isHeld = grab.isSelected;
        float distance = Vector3.Distance(hoveringValidItem.transform.position, transform.position);

        if (!isHeld && distance <= snapDistance)
        {
            SnapItem(hoveringValidItem);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentItem != null) return;

        FoodItemData foodItem = other.GetComponent<FoodItemData>();
        if (foodItem == null) return;
        if (foodItem.slotType != acceptedType) return;

        hoveringValidItem = foodItem;

        if (socketVisual != null)
            socketVisual.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        FoodItemData foodItem = other.GetComponent<FoodItemData>();
        if (foodItem == null) return;
        if (foodItem != hoveringValidItem) return;

        hoveringValidItem = null;

        if (socketVisual != null && currentItem == null)
            socketVisual.SetActive(false);
    }

    private void SnapItem(FoodItemData foodItem)
    {
        currentItem = foodItem;
        hoveringValidItem = null;

        Rigidbody rb = foodItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        foodItem.transform.position = transform.position;
        foodItem.transform.rotation = transform.rotation;
        foodItem.transform.SetParent(transform);

        XRGrabInteractable grab = foodItem.GetComponent<XRGrabInteractable>();
        if (grab != null)
        {
            grab.enabled = false;
        }

        if (socketVisual != null)
            socketVisual.SetActive(false);

        if (trayMealTracker != null)
            trayMealTracker.RegisterItem(acceptedType, foodItem);
    }
}