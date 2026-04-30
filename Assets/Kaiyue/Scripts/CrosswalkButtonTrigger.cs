using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class CrosswalkButtonTrigger : MonoBehaviour
{
    public CarMover carMover;
    public CrossingTimer crossingTimer;
    public TMP_Text messageText;
    public SoundManager soundManager;
    public float stopDuration = 3f;

    private bool isActive = false;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.nKey.wasPressedThisFrame)
        {
            Debug.Log("N pressed: trying to activate crosswalk button.");
            TryActivateButton();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player touched crosswalk button trigger.");
            TryActivateButton();
        }
    }

    public void TryActivateButton()
    {
        if (!isActive)
        {
            StartCoroutine(ActivateCrosswalk());
        }
    }

    IEnumerator ActivateCrosswalk()
    {
        isActive = true;

        Debug.Log("Crosswalk button activated.");

        if (carMover != null)
        {
            carMover.StopCar();
        }
        
        EventManager.Instance.stopCars.Invoke();

        if (crossingTimer != null)
        {
            crossingTimer.PauseTimer();
        }

        if (soundManager != null)
        {
            soundManager.PlayCrosswalkTimer();
        }

        if (messageText != null)
        {
            messageText.text = "WALK!";
            messageText.color = new Color(0.1f, 1f, 0.2f);
            messageText.fontSize = 72;
            messageText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(stopDuration);

        if (soundManager != null)
        {
            soundManager.StopCrosswalkTimer();
        }

        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }

        if (carMover != null)
        {
            carMover.StartCar();
        }
        EventManager.Instance.startCars.Invoke();
        if (crossingTimer != null)
        {
            crossingTimer.ResumeTimer();
        }

        isActive = false;
    }
}