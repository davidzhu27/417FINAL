using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRBaseInteractable))]
public class MoneyCollectible : MonoBehaviour
{
    [Header("Collectible Info")]
    public string itemName = "Money";
    public int moneyValue = 100;

    [Header("Input")]
    public InputActionReference rightTriggerAction;

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

        if (rightTriggerAction != null)
        {
            rightTriggerAction.action.performed += OnRightTriggerPressed;
            rightTriggerAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        interactable.hoverEntered.RemoveListener(OnHoverEntered);
        interactable.hoverExited.RemoveListener(OnHoverExited);

        if (rightTriggerAction != null)
        {
            rightTriggerAction.action.performed -= OnRightTriggerPressed;
        }
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        hoverCount++;
        Debug.Log("Hovering money collectible: " + itemName);
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        hoverCount = Mathf.Max(0, hoverCount - 1);
    }

    private void OnRightTriggerPressed(InputAction.CallbackContext context)
    {
        if (hoverCount <= 0) return;

        Collect();
    }

    private void Collect()
    {
        if (collected) return;
        collected = true;

        if (MoneyManager.Instance != null)
        {
            MoneyManager.Instance.AddMoney(itemName, moneyValue);
        }
        else
        {
            Debug.LogWarning("MoneyManager not found in scene.");
        }

        Debug.Log($"Collected money object: {itemName}, value: {moneyValue}");

        if (collectParticlePrefab != null)
        {
            Instantiate(collectParticlePrefab, transform.position, Quaternion.identity);
        }

        if (collectSound != null)
        {
            audioSource.PlayOneShot(collectSound);
        }

        StartCoroutine(CollectAnimation());
    }

    private IEnumerator CollectAnimation()
    {
        Vector3 startScale = transform.localScale;
        Vector3 bigScale = startScale * 1.25f;

        float timer = 0f;

        while (timer < collectAnimationDuration)
        {
            timer += Time.deltaTime;
            float t = timer / collectAnimationDuration;

            if (t < 0.5f)
            {
                float eased = Mathf.SmoothStep(0f, 1f, t / 0.5f);
                transform.localScale = Vector3.Lerp(startScale, bigScale, eased);
            }
            else
            {
                float eased = Mathf.SmoothStep(0f, 1f, (t - 0.5f) / 0.5f);
                transform.localScale = Vector3.Lerp(bigScale, Vector3.zero, eased);
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }
}