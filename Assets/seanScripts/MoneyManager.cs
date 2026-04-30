using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    public int TotalMoney { get; private set; }

    private Dictionary<string, int> collectedCounts = new Dictionary<string, int>();
    private Dictionary<string, int> collectedValues = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddMoney(string itemName, int value)
    {
        TotalMoney += value;

        if (!collectedCounts.ContainsKey(itemName))
        {
            collectedCounts[itemName] = 0;
            collectedValues[itemName] = 0;
        }

        collectedCounts[itemName] += 1;
        collectedValues[itemName] += value;

        Debug.Log($"Collected {itemName}: +${value}. Total Money: ${TotalMoney}");
    }

    public string GetSummary()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Collected Items:");

        foreach (var pair in collectedCounts)
        {
            string itemName = pair.Key;
            int count = pair.Value;
            int value = collectedValues[itemName];

            sb.AppendLine($"{itemName}: x{count} = ${value}");
        }

        sb.AppendLine();
        sb.AppendLine($"Total Money: ${TotalMoney}");

        return sb.ToString();
    }

    public void ResetMoney()
    {
        TotalMoney = 0;
        collectedCounts.Clear();
        collectedValues.Clear();
    }
}