using UnityEngine;
using TMPro; // Required for TextMeshPro

public class AttemptManager : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI attemptDisplay; // Drag your UI text here
    public string prefix = "Attempts: ";

    [Header("Data Settings")]
    public string saveKey = "TotalPlayerAttempts";

    public int TotalAttempts { get; private set; }

    void Awake()
    {
        LoadData();
        UpdateUI();
    }

    public void AddAttempt()
    {
        TotalAttempts++;
        SaveData();
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (attemptDisplay != null)
        {
            attemptDisplay.text = prefix + TotalAttempts.ToString();
        }
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(saveKey, TotalAttempts);
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        TotalAttempts = PlayerPrefs.GetInt(saveKey, 0);
    }
}