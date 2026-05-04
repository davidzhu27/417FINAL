using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CashierCheckout : MonoBehaviour
{
    public TrayMealTracker trayMealTracker;
    public GameObject cashierTextObject;
    public TextMeshProUGUI cashierText;

    public GameObject healthyDrink;
    public GameObject poisonDrink;

    public PlayerHUDController playerHUDController;

    public string failSceneName = "Lunch";
    public float resultDelay = 3f;

    public bool drinkStageUnlocked = false;

    private bool resultShown = false;
    private float resultTimer = -1f;
    private bool passedResult = false;

    private bool failTick1Done = false;
    private bool failTick2Done = false;
    private bool failTick3Done = false;

    public string nextSceneName;

    private void Start()
    {
        if (cashierTextObject != null)
            cashierTextObject.SetActive(false);

        if (healthyDrink != null)
            healthyDrink.SetActive(false);

        if (poisonDrink != null)
            poisonDrink.SetActive(false);
    }

    private void Update()
    {
        if (!drinkStageUnlocked) return;
        if (trayMealTracker == null) return;

        if (!resultShown && trayMealTracker.HasDrink())
        {
            resultShown = true;
            passedResult = trayMealTracker.IsMealSafe();

            if (cashierTextObject != null)
                cashierTextObject.SetActive(true);

            if (cashierText != null)
                cashierText.text = passedResult ? "Bon Appetit!" : "You feel sick...";
                if (!passedResult) {
                    SceneTransitionManager.Instance.TransitionToScene(nextSceneName);
                }
                else {
                    CutsceneManager.Instance.PlayCutscene("Cutscene_FoodPoisoning");
                }

            resultTimer = resultDelay;
        }

        if (resultShown && resultTimer > 0f)
        {
            if (!passedResult && playerHUDController != null)
            {
                if (!failTick1Done && resultTimer <= 3f)
                {
                    playerHUDController.ChangeHealth(-33);
                    failTick1Done = true;
                }

                if (!failTick2Done && resultTimer <= 2f)
                {
                    playerHUDController.ChangeHealth(-33);
                    failTick2Done = true;
                }

                if (!failTick3Done && resultTimer <= 1f)
                {
                    playerHUDController.ChangeHealth(-34);
                    failTick3Done = true;
                }
            }

            resultTimer -= Time.deltaTime;

            if (resultTimer <= 0f)
            {
                HandleMealResult();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (cashierTextObject != null)
            cashierTextObject.SetActive(true);

        if (trayMealTracker == null || !trayMealTracker.HasRequiredFood())
        {
            if (cashierText != null)
                cashierText.text = "Your tray isn't full yet.";
            return;
        }

        if (resultShown) return;

        drinkStageUnlocked = true;

        if (cashierText != null)
            cashierText.text = "Choose a drink.";

        if (healthyDrink != null)
            healthyDrink.SetActive(true);

        if (poisonDrink != null)
            poisonDrink.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!resultShown && cashierTextObject != null)
            cashierTextObject.SetActive(false);
    }

    private void HandleMealResult()
    {
        if (passedResult)
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.LogWarning("No next scene found in Build Settings.");
            }
        }
        else
        {
            SceneManager.LoadScene(failSceneName);
        }
    }
}