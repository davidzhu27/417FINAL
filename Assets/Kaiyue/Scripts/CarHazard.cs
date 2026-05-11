using UnityEngine;
using Unity.XR.CoreUtils;
using System.Collections;

public class CarHazard : MonoBehaviour
{
    [Header("Player References")]
    public Transform player;              // XR Origin, used for reset
    public Transform detectionTarget;     // Main Camera, used for detecting headset position
    public Transform playerStartPoint;

    [Header("Detection Distances")]
    public float hornDistance = 10f;
    public float hitDistance = 8f;

    [Header("References")]
    public SoundManager soundManager;
    public Vehicle vehicle;
    public CrossingTimer crossingTimer;
    public CrossingTimerZone timerZone;
    public PlayerHUDController hudController;

    [Header("Feedback")]
    public float stayDownDuration = 0.8f;

    private bool hornPlayed = false;
    private bool recentlyHit = false;
    private Quaternion playerOriginalRotation;

    private float nextDebugTime = 0f;

    void Start()
    {
        AutoAssignReferences();

        if (player != null)
        {
            playerOriginalRotation = player.rotation;
        }
    }

    public void Setup(
        Transform n_player,
        Transform n_playerStartPoint,
        SoundManager s_manager,
        CrossingTimer ct,
        CrossingTimerZone tz,
        PlayerHUDController hc
    )
    {
        player = n_player;
        playerStartPoint = n_playerStartPoint;
        soundManager = s_manager;
        crossingTimer = ct;
        timerZone = tz;
        hudController = hc;

        AutoAssignReferences();

        if (player != null)
        {
            playerOriginalRotation = player.rotation;
        }

        Debug.Log("CarHazard setup complete. Player = " +
            (player != null ? player.name : "NULL") +
            ", DetectionTarget = " +
            (detectionTarget != null ? detectionTarget.name : "NULL") +
            ", StartPoint = " +
            (playerStartPoint != null ? playerStartPoint.name : "NULL"));
    }

    private void AutoAssignReferences()
    {
        if (vehicle == null)
        {
            vehicle = GetComponent<Vehicle>();
            if (vehicle == null)
            {
                vehicle = GetComponentInParent<Vehicle>();
            }
            if (vehicle == null)
            {
                vehicle = GetComponentInChildren<Vehicle>();
            }
        }

        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");

            if (playerObj == null)
            {
                playerObj = GameObject.Find("XR Origin (XR Rig)");
            }

            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        if (detectionTarget == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                detectionTarget = mainCamera.transform;
            }
        }

        if (playerStartPoint == null)
        {
            GameObject startObj = GameObject.Find("PlayerStartPoint");

            if (startObj != null)
            {
                playerStartPoint = startObj.transform;
            }
        }

        if (soundManager == null)
        {
            soundManager = Object.FindFirstObjectByType<SoundManager>();
        }

        if (crossingTimer == null)
        {
            crossingTimer = Object.FindFirstObjectByType<CrossingTimer>();
        }

        if (timerZone == null)
        {
            timerZone = Object.FindFirstObjectByType<CrossingTimerZone>();
        }
    }

    void Update()
    {
        if (player == null || detectionTarget == null || playerStartPoint == null)
        {
            AutoAssignReferences();

            if (Time.time >= nextDebugTime)
            {
                Debug.LogWarning("CarHazard missing references. Player = " +
                    (player != null ? player.name : "NULL") +
                    ", DetectionTarget = " +
                    (detectionTarget != null ? detectionTarget.name : "NULL") +
                    ", StartPoint = " +
                    (playerStartPoint != null ? playerStartPoint.name : "NULL"));
                nextDebugTime = Time.time + 1f;
            }

            return;
        }

        float distance = GetHorizontalDistanceToDetectionTarget();

        if (distance < hornDistance && !hornPlayed)
        {
            Debug.Log("Car horn triggered. Distance: " + distance);

            if (soundManager != null)
            {
                soundManager.PlayHorn();
            }
            else
            {
                Debug.LogWarning("SoundManager is not assigned on CarHazard.");
            }

            hornPlayed = true;
        }

        if (distance >= hornDistance)
        {
            hornPlayed = false;
        }

        if (distance < hitDistance && !recentlyHit)
        {
            Debug.Log("Car hit triggered. Distance: " + distance);
            StartCoroutine(HitPlayer());
        }
    }

    private float GetHorizontalDistanceToDetectionTarget()
    {
        Vector3 carPos = transform.position;
        Vector3 targetPos = detectionTarget.position;

        carPos.y = 0f;
        targetPos.y = 0f;

        return Vector3.Distance(carPos, targetPos);
    }

    private bool IsPlayer(Collider other)
    {
        return other.CompareTag("Player") ||
               other.transform.root.CompareTag("Player") ||
               other.GetComponentInParent<CharacterController>() != null;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Car touched: " + other.name + " tag: " + other.tag);

        if (IsPlayer(other) && !recentlyHit)
        {
            Debug.Log("Car trigger detected player.");
            StartCoroutine(HitPlayer());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsPlayer(other) && !recentlyHit)
        {
            Debug.Log("Car trigger stayed on player.");
            StartCoroutine(HitPlayer());
        }
    }

    private void ResetPlayerToStart()
    {
        if (player == null || playerStartPoint == null)
        {
            Debug.LogWarning("Player or PlayerStartPoint is not assigned on CarHazard.");
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

        Debug.Log("Player reset after car hit. New player position: " + player.position);
    }

    IEnumerator HitPlayer()
    {
        recentlyHit = true;

        Debug.Log("Player was hit by car!");

        if (vehicle != null)
        {
            vehicle.StopCar();
        }

        if (soundManager != null)
        {
            soundManager.PlayCrashAndOuch();
        }
        else
        {
            Debug.LogWarning("SoundManager is not assigned on CarHazard.");
        }

        hudController.ChangeHealth(-100);

        yield return new WaitForSeconds(stayDownDuration);

        ResetPlayerToStart();

        if (crossingTimer != null)
        {
            crossingTimer.ResetTimerHidden();
        }

        if (timerZone != null)
        {
            timerZone.ResetZone();
        }

        GameObject[] allCars = GameObject.FindGameObjectsWithTag("Car NPCs");

        for (int i = 0; i < allCars.Length; i++)
        {
            Destroy(allCars[i]);
        }

        if (EventManager.Instance != null)
        {
            EventManager.Instance.startCars.Invoke();
        }
        else
        {
            Debug.LogWarning("EventManager.Instance is null. Cars were not restarted.");
        }

        yield return new WaitForSeconds(0.3f);
        recentlyHit = false;
    }
}