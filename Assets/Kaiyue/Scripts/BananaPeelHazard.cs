using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class BananaPeelHazard : MonoBehaviour
{
    public Transform player;
    public Transform playerStartPoint;
    public Transform holdPoint;
    public TMP_Text messageText;
    public SoundManager soundManager;

    public float pickupDistance = 2f;
    public float throwDistance = 2.5f;
    public float fallDuration = 0.25f;
    public float stayDownDuration = 0.8f;

    private bool isHeld = false;
    private bool recentlySlipped = false;
    private Quaternion playerOriginalRotation;

    void Start()
    {
        if (player != null)
        {
            playerOriginalRotation = player.rotation;
        }
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Press G to pick up banana peel when close
        if (!isHeld && distanceToPlayer <= pickupDistance && Keyboard.current != null && Keyboard.current.gKey.wasPressedThisFrame)
        {
            PickUpBanana();
        }

        // Press T to throw/place banana peel forward
        if (isHeld && Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
        {
            ThrowBanana();
        }

        // Follow hold point while held
        if (isHeld && holdPoint != null)
        {
            transform.position = holdPoint.position;
            transform.rotation = holdPoint.rotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isHeld || recentlySlipped)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            StartCoroutine(SlipAndReset());
        }
    }

    void PickUpBanana()
    {
        isHeld = true;

        Debug.Log("Banana peel picked up. Press T to throw/place it.");

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

    IEnumerator SlipAndReset()
    {
        recentlySlipped = true;

        Debug.Log("Player slipped on banana peel!");

        if (soundManager != null)
        {
            soundManager.PlayCrashAndOuch();
        }

        if (messageText != null)
        {
            messageText.text = "Slipped!";
            messageText.color = new Color(1f, 0.85f, 0.05f);
            messageText.fontSize = 72;
            messageText.gameObject.SetActive(true);
        }

        Vector3 startPosition = player.position;
        Quaternion startRotation = player.rotation;

        Quaternion fallenRotation = Quaternion.Euler(
            startRotation.eulerAngles.x,
            startRotation.eulerAngles.y,
            startRotation.eulerAngles.z + 90f
        );

        Vector3 fallenPosition = startPosition + new Vector3(0f, -0.7f, 0f);

        float elapsed = 0f;

        while (elapsed < fallDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fallDuration;

            player.rotation = Quaternion.Lerp(startRotation, fallenRotation, t);
            player.position = Vector3.Lerp(startPosition, fallenPosition, t);

            yield return null;
        }

        player.rotation = fallenRotation;
        player.position = fallenPosition;

        yield return new WaitForSeconds(stayDownDuration);

        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
            messageText.transform.localScale = Vector3.one;
        }

        if (player != null && playerStartPoint != null)
        {
            player.position = playerStartPoint.position;
            player.rotation = playerOriginalRotation;
        }

        yield return new WaitForSeconds(0.3f);
        recentlySlipped = false;
    }
}