using UnityEngine;
using TMPro;
using System.Collections;

public class CrossingTimer : MonoBehaviour
{
    public float timeLimit = 5f;
    public TMP_Text timerText;
    public TMP_Text messageText;
    public Transform player;
    public Transform playerStartPoint;

    private float currentTime;
    private bool isRunning = false;
    private bool isResetting = false;
    private bool isPaused = false;
    private bool levelCompleted = false;

    void Start()
    {
        HideTimer();

        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
            messageText.transform.localScale = Vector3.one;
            messageText.rectTransform.anchoredPosition = Vector2.zero;
        }
    }

    void Update()
    {
        if (!isRunning || isResetting || isPaused || levelCompleted)
        {
            return;
        }

        currentTime -= Time.deltaTime;

        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
            timerText.text = "Time: " + Mathf.Ceil(currentTime).ToString();
        }

        if (currentTime <= 0f)
        {
            StartCoroutine(TimeUpReset());
        }
    }

    public void StartTimer()
    {
        Debug.Log("StartTimer was called.");

        if (levelCompleted)
        {
            Debug.Log("Level already completed. Timer will not restart.");
            return;
        }

        if (isRunning || isResetting)
        {
            Debug.Log("Timer did not start because it is already running or resetting.");
            return;
        }

        currentTime = timeLimit;
        isRunning = true;
        isPaused = false;

        if (timerText != null)
        {
            Debug.Log("TimerText found. Showing timer.");

            if (timerText.transform.parent != null)
            {
                timerText.transform.parent.gameObject.SetActive(true);
            }

            timerText.gameObject.SetActive(true);
            timerText.color = Color.white;
            timerText.fontSize = 72;
            timerText.text = "Time: " + Mathf.Ceil(currentTime).ToString();
        }
        else
        {
            Debug.LogWarning("TimerText is NOT assigned on CrossingTimer!");
        }
    }

    public void StopTimer()
    {
        isRunning = false;
        isPaused = false;
        HideTimer();
    }

    public void CompleteLevel()
    {
        levelCompleted = true;
        isRunning = false;
        isPaused = false;
        isResetting = false;

        StopAllCoroutines();
        HideTimer();

        Debug.Log("Level completed. Timer permanently stopped.");
    }

    public bool IsLevelCompleted()
    {
        return levelCompleted;
    }

    public void ResetTimerHidden()
    {
        StopAllCoroutines();

        currentTime = timeLimit;
        isRunning = false;
        isResetting = false;
        isPaused = false;

        HideTimer();

        if (messageText != null)
        {
            if (messageText.transform.parent != null)
            {
                messageText.transform.parent.gameObject.SetActive(true);
            }

            messageText.gameObject.SetActive(false);
            messageText.transform.localScale = Vector3.one;
            messageText.rectTransform.anchoredPosition = Vector2.zero;
        }
    }

    public void ResetTimerForRetry()
    {
        if (levelCompleted)
        {
            return;
        }

        ResetTimerHidden();
    }

    private void HideTimer()
    {
        if (timerText != null)
        {
            timerText.text = "";
            timerText.gameObject.SetActive(false);
        }
    }

    IEnumerator TimeUpReset()
    {
        if (levelCompleted)
        {
            yield break;
        }

        isResetting = true;
        isRunning = false;

        HideTimer();

        if (messageText != null)
        {
            if (messageText.transform.parent != null)
            {
                messageText.transform.parent.gameObject.SetActive(true);
            }

            messageText.text = "Too slow!\nTry again!";
            messageText.color = new Color(1f, 0.35f, 0.05f);
            messageText.fontSize = 64;
            messageText.gameObject.SetActive(true);

            yield return StartCoroutine(PopAndBounceMessage());
        }
        else
        {
            yield return new WaitForSeconds(1.2f);
        }

        if (levelCompleted)
        {
            yield break;
        }

        if (player != null && playerStartPoint != null)
        {
            player.position = playerStartPoint.position;
        }

        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
            messageText.transform.localScale = Vector3.one;
            messageText.rectTransform.anchoredPosition = Vector2.zero;
        }

        if (EventManager.Instance != null)
        {
            EventManager.Instance.startCars.Invoke();
        }
        else
        {
            Debug.LogWarning("EventManager.Instance is null. Cars were not restarted.");
        }

        ResetTimerHidden();
    }

    IEnumerator PopAndBounceMessage()
    {
        if (messageText == null)
        {
            yield break;
        }

        float duration = 1.4f;
        float elapsed = 0f;

        Vector3 smallScale = Vector3.one * 0.2f;
        Vector3 bigScale = Vector3.one * 1.45f;
        Vector3 normalScale = Vector3.one;

        RectTransform rect = messageText.rectTransform;
        Vector2 originalPosition = rect.anchoredPosition;

        messageText.transform.localScale = smallScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            if (t < 0.3f)
            {
                float popT = t / 0.3f;
                messageText.transform.localScale = Vector3.Lerp(smallScale, bigScale, popT);
            }
            else if (t < 0.5f)
            {
                float settleT = (t - 0.3f) / 0.2f;
                messageText.transform.localScale = Vector3.Lerp(bigScale, normalScale, settleT);
            }
            else
            {
                float bounce = Mathf.Abs(Mathf.Sin((t - 0.5f) * Mathf.PI * 5f));
                rect.anchoredPosition = originalPosition + new Vector2(0f, bounce * 25f);

                float scaleBounce = 1f + bounce * 0.08f;
                messageText.transform.localScale = Vector3.one * scaleBounce;
            }

            yield return null;
        }

        rect.anchoredPosition = originalPosition;
        messageText.transform.localScale = Vector3.one;
    }

    public void PauseTimer()
    {
        if (isRunning && !levelCompleted)
        {
            isPaused = true;
        }
    }

    public void ResumeTimer()
    {
        if (isRunning && !levelCompleted)
        {
            isPaused = false;
        }
    }
}