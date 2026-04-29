using TMPro;
using UnityEngine;

public class MoneyHUD : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    private void Update()
    {
        if (moneyText == null) return;

        if (MoneyManager.Instance == null)
        {
            moneyText.text = "MoneyManager not found";
            return;
        }

        moneyText.text = "Money: $" + MoneyManager.Instance.TotalMoney;
    }
}