using UnityEngine;
using System.Collections;

public class CarHazard : MonoBehaviour
{
    public Transform player;
    public Transform playerStartPoint;

    public float hornDistance = 4f;
    public float hitDistance = 1.5f;

    public SoundManager soundManager;
    public Vehicle vehicle;

    public CrossingTimer crossingTimer;
    public CrossingTimerZone timerZone;

    public float fallDuration = 0.25f;
    public float stayDownDuration = 0.8f;

    private bool hornPlayed = false;
    private bool recentlyHit = false;
    private Quaternion playerOriginalRotation;

    void Start()
    {
        if (player != null)
        {
            playerOriginalRotation = player.rotation;
        }

        if (vehicle == null)
        {
            vehicle = GetComponent<Vehicle>();
        }
    }
    public void Setup(Transform n_player, Transform n_playerStartPoint, SoundManager s_manager, CrossingTimer ct, CrossingTimerZone tz) {
        player = n_player;
        playerStartPoint = n_playerStartPoint;
        soundManager = s_manager;
        crossingTimer = ct;
        timerZone = tz;
        playerOriginalRotation = player.rotation;
    }

    void Update()
    {
        if (player == null || playerStartPoint == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < hornDistance && !hornPlayed)
        {
            if (soundManager != null)
            {
                soundManager.PlayHorn();
            }

            hornPlayed = true;
        }

        if (distance >= hornDistance)
        {
            hornPlayed = false;
        }

        if (distance < hitDistance && !recentlyHit)
        {
            StartCoroutine(HitPlayer());
        }
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

        // Reset player back to sidewalk
        player.position = playerStartPoint.position;
        player.rotation = playerOriginalRotation;

        // Reset timer so it hides again and can restart when player enters StartTimerZone
        if (crossingTimer != null)
        {
            crossingTimer.ResetTimerHidden();
        }

        if (timerZone != null)
        {
            timerZone.ResetZone();
        }

        GameObject[] all_cars = GameObject.FindGameObjectsWithTag("Car NPCs");
        for (int i = 0; i < all_cars.Length; i++) {
            Destroy(all_cars[i]);
        }
        EventManager.Instance.startCars.Invoke();
        // if (vehicle != null)
        // {
        //     vehicle.StartCar();
        // }

        // yield return new WaitForSeconds(0.3f);
        // recentlyHit = false;
    }
}