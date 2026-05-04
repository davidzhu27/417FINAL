using UnityEngine;

public class TraySocket : MonoBehaviour
{
    public FoodSlotType acceptedType;
    public TrayMealTracker trayMealTracker;
    public GameObject socketVisual;

    private FoodItemData currentItem;
    private FoodItemData hoveringValidItem;

    private void Start()
    {
        if (socketVisual != null)
            socketVisual.SetActive(false);
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

    private void OnTriggerStay(Collider other)
    {
        if (currentItem != null) return;

        FoodItemData foodItem = other.GetComponent<FoodItemData>();
        if (foodItem == null) return;
        if (foodItem.slotType != acceptedType) return;

        SnapItem(foodItem);
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
        }

        foodItem.transform.position = transform.position;
        foodItem.transform.rotation = transform.rotation;
        foodItem.transform.SetParent(transform);

        if (socketVisual != null)
            socketVisual.SetActive(false);

        if (trayMealTracker != null)
            trayMealTracker.RegisterItem(acceptedType, foodItem);
    }
}