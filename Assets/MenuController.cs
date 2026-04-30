using UnityEngine;
using UnityEngine.UI; // Required for the Button component
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("References")]
    public AttemptManager attemptManager;

    [Header("UI Button Slots")]
    [SerializeField] private Button resetButton;
    [SerializeField] private Button quitButton;

    private void OnEnable()
    {
        // This "subscribes" the buttons to the functions automatically
        if (resetButton != null)
            resetButton.onClick.AddListener(ResetPlayer);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    private void OnDisable()
    {
        // Good practice to "unsubscribe" when the object is disabled
        if (resetButton != null)
            resetButton.onClick.RemoveListener(ResetPlayer);

        if (quitButton != null)
            quitButton.onClick.RemoveListener(QuitGame);
    }

    public void ResetPlayer()
    {
        Time.timeScale = 1f;
        if (attemptManager != null)
        {
            attemptManager.AddAttempt();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Button Pressed");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}