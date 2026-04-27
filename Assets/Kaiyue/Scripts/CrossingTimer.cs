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
    private bool isResetting = false;
    private bool isPaused = false;

    void Start()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }

        ResetTimer();
    }

    void Update()
    {
        if (isResetting || isPaused)
        {
            return;
        }

        currentTime -= Time.deltaTime;

        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.Ceil(currentTime).ToString();
        }

        if (currentTime <= 0f)
        {
            StartCoroutine(TimeUpReset());
        }
    }

    IEnumerator TimeUpReset()
    {
        isResetting = true;

        if (messageText != null)
        {
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

        ResetTimer();
        isResetting = false;
    }

    IEnumerator PopAndBounceMessage()
    {
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

    public void ResetTimer()
    {
        currentTime = timeLimit;

        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.Ceil(currentTime).ToString();
        }
    }

    public void PauseTimer()
    {
        isPaused = true;
    }

    public void ResumeTimer()
    {
        isPaused = false;
    }
}