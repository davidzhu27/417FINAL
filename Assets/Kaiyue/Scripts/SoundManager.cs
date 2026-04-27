using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip carHornClip;
    public AudioClip crashClip;
    public AudioClip ouchClip;

    public void PlayHorn()
    {
        PlayClip(carHornClip);
    }

    public void PlayCrashAndOuch()
    {
        PlayClip(crashClip);
        PlayClip(ouchClip);
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