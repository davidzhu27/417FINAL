using UnityEngine;
using System.Collections;

public class CarHazard : MonoBehaviour
{
    public Transform player;
    public Transform playerStartPoint;

    public float hornDistance = 4f;
    public float hitDistance = 1.5f;

    public AudioClip hornClip;
    public AudioClip crashClip;
    public AudioClip ouchClip;

    public float soundVolume = 1f;

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
            if (hornClip != null)
            {
                AudioSource.PlayClipAtPoint(hornClip, Camera.main.transform.position, soundVolume);
                Debug.Log("Horn sound played");
            }
            else
            {
                Debug.LogWarning("Horn clip is missing!");
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

        if (crashClip != null)
        {
            AudioSource.PlayClipAtPoint(crashClip, Camera.main.transform.position, soundVolume);
            Debug.Log("Crash sound played");
        }
        else
        {
            Debug.LogWarning("Crash clip is missing!");
        }

        if (ouchClip != null)
        {
            AudioSource.PlayClipAtPoint(ouchClip, Camera.main.transform.position, soundVolume);
            Debug.Log("Ouch sound played");
        }
        else
        {
            Debug.LogWarning("Ouch clip is missing!");
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