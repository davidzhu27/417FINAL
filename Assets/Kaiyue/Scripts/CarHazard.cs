using UnityEngine;
using System.Collections;

public class CarHazard : MonoBehaviour
{
    public Transform player;
    public Transform playerStartPoint;

    public float hornDistance = 4f;
    public float hitDistance = 1.5f;

    public SoundManager soundManager;

    private bool hornPlayed = false;
    private bool recentlyHit = false;

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

        if (soundManager != null)
        {
            soundManager.PlayCrashAndOuch();
        }
        else
        {
            Debug.LogWarning("SoundManager is missing on CarHazard.");
        }

        yield return new WaitForSeconds(0.2f);

        if (player != null && playerStartPoint != null)
        {
            player.position = playerStartPoint.position;
        }

        yield return new WaitForSeconds(0.8f);
        recentlyHit = false;
    }
}