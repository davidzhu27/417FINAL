using UnityEngine;
using UnityEngine.InputSystem;
using Unity.XR.CoreUtils;
using TMPro;
using System.Collections;

public class BananaPeelHazard : MonoBehaviour
{
    [Header("Player References")]
    public Transform player;              // XR Origin, used for reset
    public Transform detectionTarget;     // Main Camera, used for detecting headset position
    public Transform playerStartPoint;
    public Transform holdPoint;

    [Header("UI and Audio")]
    public TMP_Text messageText;
    public SoundManager soundManager;

    [Header("Timer References")]
    public CrossingTimer crossingTimer;
    public CrossingTimerZone timerZone;

    [Header("Distances")]
    public float pickupDistance = 5f;
    public float throwDistance = 2.5f;
    public float slipDistance = 3f;

    [Header("Feedback")]
    public float stayDownDuration = 0.8f;

    private bool isHeld = false;
    private bool recentlySlipped = false;
    private Quaternion playerOriginalRotation;

    void Start()
    {
        AutoAssignDetectionTarget();

        if (player != null)
        {
            playerOriginalRotation = player.rotation;
        }
    }

    void Update()
    {
        AutoAssignDetectionTarget();

        if (player == null || detectionTarget == null)
        {
            return;
        }

        float distanceToPlayer = GetHorizontalDistanceToDetectionTarget();

        // Distance-based slip check. More reliable for Meta headset.
        if (!isHeld && !recentlySlipped && distanceToPlayer <= slipDistance)
        {
            StartCoroutine(SlipAndReset());
            return;
        }

        // Press G to pick up banana peel when close.
        if (!isHeld && distanceToPlayer <= pickupDistance && Keyboard.current != null && Keyboard.current.gKey.wasPressedThisFrame)
        {
            PickUpBanana();
        }

        // Press H to throw/place banana peel forward.
        // Changed from T because T conflicts with XR Device Simulator.
        if (isHeld && Keyboard.current != null && Keyboard.current.hKey.wasPressedThisFrame)
        {
            ThrowBanana();
        }

        // Follow hold point while held.
        if (isHeld && holdPoint != null)
        {
            transform.position = holdPoint.position;
            transform.rotation = holdPoint.rotation;
        }
    }

    private void AutoAssignDetectionTarget()
    {
        if (detectionTarget != null)
        {
            return;
        }

        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            detectionTarget = mainCamera.transform;
        }
    }

    private float GetHorizontalDistanceToDetectionTarget()
    {
        Vector3 bananaPos = transform.position;
        Vector3 targetPos = detectionTarget.position;

        bananaPos.y = 0f;
        targetPos.y = 0f;

        return Vector3.Distance(bananaPos, targetPos);
    }

    private bool IsPlayer(Collider other)
    {
        return other.CompareTag("Player") ||
               other.transform.root.CompareTag("Player") ||
               other.GetComponentInParent<CharacterController>() != null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isHeld || recentlySlipped)
        {
            return;
        }

        Debug.Log("Banana touched by: " + other.name + " tag: " + other.tag);

        if (IsPlayer(other))
        {
            StartCoroutine(SlipAndReset());
        }
    }

    void PickUpBanana()
    {
        isHeld = true;

        Debug.Log("Banana peel picked up. Press H to throw/place it.");

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        if (holdPoint != null)
        {
            transform.position = holdPoint.position;
            transform.rotation = holdPoint.rotation;
        }
    }

    void ThrowBanana()
    {
        isHeld = false;

        Debug.Log("Banana peel thrown/placed forward.");

        if (holdPoint != null)
        {
            Vector3 forwardPosition = holdPoint.position + holdPoint.forward * throwDistance;
            transform.position = new Vector3(forwardPosition.x, 0.05f, forwardPosition.z);
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    private void ResetPlayerToStart()
    {
        if (player == null || playerStartPoint == null)
        {
            Debug.LogWarning("Player or PlayerStartPoint is not assigned on BananaPeelHazard.");
            return;
        }

        CharacterController cc = player.GetComponent<CharacterController>();
        XROrigin xrOrigin = player.GetComponent<XROrigin>();

        if (cc != null)
        {
            cc.enabled = false;
        }

        if (xrOrigin != null)
        {
            xrOrigin.MoveCameraToWorldLocation(playerStartPoint.position);

            Vector3 euler = player.rotation.eulerAngles;
            euler.y = playerStartPoint.rotation.eulerAngles.y;
            player.rotation = Quaternion.Euler(euler);
        }
        else
        {
            player.position = playerStartPoint.position;
            player.rotation = playerStartPoint.rotation;
        }

        if (cc != null)
        {
            cc.enabled = true;
        }

        Debug.Log("Player reset after slipping. New player position: " + player.position);
    }

    IEnumerator SlipAndReset()
    {
        recentlySlipped = true;

        Debug.Log("Player slipped on banana peel!");

        if (soundManager != null)
        {
            soundManager.PlayCrashAndOuch();
        }
        else
        {
            Debug.LogWarning("SoundManager is not assigned on BananaPeelHazard.");
        }

        if (messageText != null)
        {
            messageText.text = "Slipped!";
            messageText.color = new Color(1f, 0.85f, 0.05f);
            messageText.fontSize = 72;
            messageText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(stayDownDuration);

        ResetPlayerToStart();

        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
            messageText.transform.localScale = Vector3.one;
        }

        if (crossingTimer != null)
        {
            crossingTimer.ResetTimerHidden();
        }

        if (timerZone != null)
        {
            timerZone.ResetZone();
        }

        yield return new WaitForSeconds(0.3f);
        recentlySlipped = false;
    }
}