using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text[] coinTexts;

    [Header("Settings")]
    [SerializeField] private int startCoins = 0;

    public int coins;

    private void Awake()
    {
        coins = Mathf.Max(0, startCoins);
        UpdateUI();
    }

    // ─────────────── PUBLIC API ───────────────

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;

        coins += amount;
        UpdateUI();
    }

    /// <summary>
    /// Пытается потратить монеты
    /// </summary>
    /// <returns>true если хватило</returns>
    public bool SpendCoins(int amount)
    {
        if (amount <= 0) return true;

        if (coins < amount)
            return false;

        coins -= amount;
        UpdateUI();
        return true;
    }

    public void SetCoins(int amount)
    {
        coins = Mathf.Max(0, amount);
        UpdateUI();
    }

    public int GetCoins()
    {
        return coins;
    }

    // ─────────────── UI ───────────────

    private void UpdateUI()
    {
        for (int i = 0; i < coinTexts.Length; i++)
        {
            if (coinTexts[i] != null)
                coinTexts[i].text = coins.ToString();
        }
    }
}
