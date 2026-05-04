using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRBaseInteractable))]
public class InventoryPickupByButton : MonoBehaviour
{
    [Header("Inventory")]
    public InventoryItemData itemData;

    [Header("Input")]
    public InputActionReference storeItemAction;

    [Header("Feedback")]
    public ParticleSystem collectParticlePrefab;
    public AudioClip collectSound;
    public float collectAnimationDuration = 0.25f;

    private XRBaseInteractable interactable;
    private AudioSource audioSource;
    private int hoverCount = 0;
    private bool collected = false;

    private void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f;
        }
    }

    private void OnEnable()
    {
        interactable.hoverEntered.AddListener(OnHoverEntered);
        interactable.hoverExited.AddListener(OnHoverExited);

        if (storeItemAction != null)
        {
            storeItemAction.action.performed += OnStoreButtonPressed;
            storeItemAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        interactable.hoverEntered.RemoveListener(OnHoverEntered);
        interactable.hoverExited.RemoveListener(OnHoverExited);

        if (storeItemAction != null)
        {
            storeItemAction.action.performed -= OnStoreButtonPressed;
        }
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        hoverCount++;
        Debug.Log("Hovering inventory item: " + itemData.itemName);
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        hoverCount = Mathf.Max(0, hoverCount - 1);
    }

    private void OnStoreButtonPressed(InputAction.CallbackContext context)
    {
        if (hoverCount <= 0) return;

        StoreInInventory();
    }

    private void StoreInInventory()
    {
        if (collected) return;
        if (itemData == null) return;
        if (InventoryManager.Instance == null) return;

        bool added = InventoryManager.Instance.TryAddItemAuto(itemData);

        if (!added)
        {
            Debug.Log("Could not store item. Inventory is full.");
            return;
        }

        collected = true;

        if (collectParticlePrefab != null)
        {
            Instantiate(collectParticlePrefab, transform.position, Quaternion.identity);
        }

        if (collectSound != null)
        {
            audioSource.PlayOneShot(collectSound);
        }

        StartCoroutine(StoreAnimation());
    }

    private IEnumerator StoreAnimation()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;

        float timer = 0f;

        while (timer < collectAnimationDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, timer / collectAnimationDuration);
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}