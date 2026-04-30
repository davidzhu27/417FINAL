using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip carHornClip;
    public AudioClip crashClip;
    public AudioClip ouchClip;
    public AudioClip crosswalkTimerClip;

    public void PlayHorn()
    {
        PlayClip(carHornClip);
    }

    public void PlayCrashAndOuch()
    {
        PlayClip(crashClip);
        PlayClip(ouchClip);
    }

    public void PlayCrosswalkTimer()
    {
        if (audioSource != null && crosswalkTimerClip != null)
        {
            audioSource.clip = crosswalkTimerClip;
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Missing audio source or crosswalk timer clip.");
        }
    }

    public void StopCrosswalkTimer()
    {
        if (audioSource != null && audioSource.clip == crosswalkTimerClip)
        {
            audioSource.Stop();
            audioSource.clip = null;
            audioSource.loop = false;
        }
    }

    private void PlayClip(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip, 1f);
        }
        else
        {
            Debug.LogWarning("Missing audio source or audio clip.");
        }
    }
}