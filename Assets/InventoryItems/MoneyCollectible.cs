using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class MoneyCollectible : MonoBehaviour
{
    [Header("Collectible Info")]
    public string itemName = "Money";
    public int moneyValue = 100;

    [Header("Feedback")]
    public ParticleSystem collectParticlePrefab;
    public AudioClip collectSound;
    public float collectAnimationDuration = 0.25f;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private AudioSource audioSource;
    private bool collected = false;

    private void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f;
        }
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.hoverEntered.AddListener(OnHoverEntered);
    }

    private void OnDisable()
    {
        grabInteractable.hoverEntered.RemoveListener(OnHoverEntered);
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        Debug.Log("Selected / Grabbed: " + itemName);
        Collect();
    }
    

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        Debug.Log("Hovering over " + itemName);
    }

    private void Collect()
    {
        if (collected) return;
        collected = true;

        MoneyManager.Instance.AddMoney(itemName, moneyValue);

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