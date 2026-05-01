using UnityEngine;

public class AlarmClockImpact : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag the AudioSource you want to mute/stop here.")]
    public AudioSource targetAudioSource;

    [Header("Impact Settings")]
    [Range(0.1f, 1000f)] // Creates a slider in the Inspector
    [Tooltip("Minimum force required to stop the alarm. Lower = Sensitive, Higher = Needs a hard hit.")]
    public float minImpactForce = 5.0f;

    private bool _isMuted = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (_isMuted || targetAudioSource == null) return;

        // Calculate impact force based on impulse
        float force = collision.impulse.magnitude / Time.fixedDeltaTime;

        if (force >= minImpactForce)
        {
            MuteAlarm();
        }
    }

    private void MuteAlarm()
    {
        // You can use .Stop() to kill it entirely, 
        // or .mute = true if you want the track to keep 'playing' silently
        targetAudioSource.Stop(); 
        _isMuted = true;
        
        Debug.Log($"Alarm silenced! Impact Force: {minImpactForce}");
    }
    
    // Call this via a button if you want to 'reset' the clock
    public void ResetAlarm()
    {
        _isMuted = false;
        if (targetAudioSource != null) targetAudioSource.Play();
    }
}