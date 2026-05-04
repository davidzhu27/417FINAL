using UnityEngine;
using TMPro;

public class CashierCheckout : MonoBehaviour
{
    public TrayMealTracker trayMealTracker;
    public GameObject cashierTextObject;
    public TextMeshProUGUI cashierText;

    public GameObject healthyDrink;
    public GameObject poisonDrink;

    public bool drinkStageUnlocked = false;
    private bool resultShown = false;
    
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
        if (resultShown) return;
        if (trayMealTracker == null) return;
        if (!trayMealTracker.HasDrink()) return;

        resultShown = true;

        if (cashierTextObject != null)
            cashierTextObject.SetActive(true);

        if (cashierText != null)
        {
            if (trayMealTracker.IsMealSafe())
                cashierText.text = "PASS";
            else
                cashierText.text = "FAIL";
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

        if (cashierTextObject != null)
            cashierTextObject.SetActive(false);
    }
}