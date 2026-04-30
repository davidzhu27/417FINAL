using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class IdleCollectibleAnimation : MonoBehaviour
{
    public float rotationSpeed = 60f;
    public float bobHeight = 0.08f;
    public float bobSpeed = 2f;

    private Vector3 startPosition;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool isGrabbed = false;

    private void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    private void Start()
    {
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }
    }

    private void OnDisable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }
    }

    private void Update()
    {
        if (isGrabbed) return;

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        float yOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = startPosition + new Vector3(0f, yOffset, 0f);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isGrabbed = true;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isGrabbed = false;
        startPosition = transform.position;
    }
}