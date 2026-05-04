using UnityEngine;

public class HotSwapHandDisplay : MonoBehaviour
{
    public Transform heldItemAnchor;

    private GameObject currentHeldObject;

    private void OnEnable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged += RefreshHeldItem;
        }

        RefreshHeldItem();
    }

    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -= RefreshHeldItem;
        }
    }

    public void RefreshHeldItem()
    {
        if (currentHeldObject != null)
        {
            Destroy(currentHeldObject);
        }

        if (InventoryManager.Instance == null) return;
        if (heldItemAnchor == null) return;

        InventoryItemData item = InventoryManager.Instance.GetSelectedHotSwapItem();

        if (item == null)
        {
            return;
        }

        GameObject prefabToShow = item.inventoryPrefab != null ? item.inventoryPrefab : item.worldPrefab;

        if (prefabToShow == null)
        {
            Debug.LogWarning(item.itemName + " has no inventoryPrefab or worldPrefab assigned.");
            return;
        }

        currentHeldObject = Instantiate(prefabToShow, heldItemAnchor);
        currentHeldObject.transform.localPosition = Vector3.zero;
        currentHeldObject.transform.localRotation = Quaternion.identity;
        currentHeldObject.transform.localScale = Vector3.one * 0.2f;

        // Make hand display visual only.
        Rigidbody[] rigidbodies = currentHeldObject.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Collider[] colliders = currentHeldObject.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        MonoBehaviour[] scripts = currentHeldObject.GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this)
            {
                script.enabled = false;
            }
        }

        Debug.Log("Showing in hand: " + item.itemName);
    }
}