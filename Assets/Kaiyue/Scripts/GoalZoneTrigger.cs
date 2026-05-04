using UnityEngine;
using TMPro;
using System.Collections;

public class GoalZoneTrigger : MonoBehaviour
{
    public TMP_Text messageText;
    public CrossingTimer crossingTimer;

    private bool reachedGoal = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Goal zone touched by: " + other.name + " tag: " + other.tag);

        bool isPlayer =
            other.CompareTag("Player") ||
            other.transform.root.CompareTag("Player") ||
            other.GetComponentInParent<CharacterController>() != null;

        if (!isPlayer)
        {
            return;
        }

        if (reachedGoal)
        {
            return;
        }

        reachedGoal = true;

        Debug.Log("Player reached the goal zone!");

        if (crossingTimer != null)
        {
            crossingTimer.CompleteLevel();
        }

        if (messageText != null)
        {
            StopAllCoroutines();
            StartCoroutine(ShowSuccessThenMoveToCorner());
        }
        else
        {
            Debug.LogWarning("Success MessageText is not assigned!");
        }
    }

    private IEnumerator ShowSuccessThenMoveToCorner()
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

        // Big text in the center first
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(900f, 250f);

        Debug.Log("Success shown in center.");

        yield return new WaitForSeconds(1.2f);

        // Then move to upper-left and keep it there
        messageText.text = "Success!";
        messageText.fontSize = 36;
        messageText.alignment = TextAlignmentOptions.Left;
        messageText.transform.localScale = Vector3.one;

        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(0f, 1f);
        rect.pivot = new Vector2(0f, 1f);
        rect.anchoredPosition = new Vector2(25f, -25f);
        rect.sizeDelta = new Vector2(350f, 100f);

        Debug.Log("Success moved to upper-left corner.");
    }
}