using UnityEngine;
using TMPro;
using System.Collections;

public class GoalZoneTrigger : MonoBehaviour
{
    public TMP_Text messageText;
    public CrossingTimer crossingTimer;

    [Header("Scene Transition")]
    public string nextSceneName = "NolanSchoolScene";
    public float transitionDelay = 1.2f;

    private bool reachedGoal = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Goal zone touched by: " + other.name + " tag: " + other.tag);

        bool isPlayer =
            other.CompareTag("Player") ||
            other.transform.root.CompareTag("Player") ||
            other.GetComponentInParent<CharacterController>() != null;

        if (!isPlayer || reachedGoal)
        {
            return;
        }

        reachedGoal = true;

        Debug.Log("Player reached the goal zone!");

        if (crossingTimer != null)
        {
            crossingTimer.CompleteLevel();
        }
        else
        {
            Debug.LogWarning("CrossingTimer is not assigned on GoalZoneTrigger.");
        }

        StopAllCoroutines();
        StartCoroutine(ShowSuccessThenTransition());
    }

    private IEnumerator ShowSuccessThenTransition()
    {
        if (messageText != null)
        {
            if (messageText.transform.parent != null)
            {
                messageText.transform.parent.gameObject.SetActive(true);
            }

            RectTransform rect = messageText.rectTransform;

            messageText.gameObject.SetActive(true);
            messageText.enableAutoSizing = false;
            messageText.text = "Success!";
            messageText.color = Color.green;
            messageText.fontSize = 96;
            messageText.alignment = TextAlignmentOptions.Center;
            messageText.transform.localScale = Vector3.one;

            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(900f, 250f);

            Debug.Log("Success shown in center.");
        }
        else
        {
            Debug.LogWarning("Success MessageText is not assigned on GoalZoneTrigger.");
        }

        yield return new WaitForSeconds(transitionDelay);

        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogWarning("Next scene name is not assigned on GoalZoneTrigger.");
            yield break;
        }

        if (SceneTransitionManager.Instance != null)
        {
            Debug.Log("Transitioning with SceneTransitionManager to: " + nextSceneName);
            SceneTransitionManager.Instance.TransitionToScene(nextSceneName);
        }
        else
        {
            Debug.LogError("SceneTransitionManager.Instance is null. Please add SceneTransitionManager to Level 2 scene.");
        }
    }
}