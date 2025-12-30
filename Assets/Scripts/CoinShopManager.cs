using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ShopButton
{
    public Button button;
    public int price;
}

public class CoinShopManager : MonoBehaviour
{
    public AudioSource ui;
    [Header("Refs")]
    public CoinManager coinManager; // внешний CoinManager

    [Header("Shop Buttons")]
    public ShopButton[] shopButtons;

    [Header("Colors")]
    public Color canBuyColor = Color.white;       // когда хватает денег
    public Color cannotBuyColor = new Color(1, 1, 1, 0.4f); // если не хватает

    private void OnEnable()
    {
        UpdateShopButtons();
    }

    // ─────────────── BUTTON CLICK ───────────────
    public void OnShopButtonPressed(int index)
    {
        ui.Play();
        if (index < 0 || index >= shopButtons.Length)
            return;

        ShopButton b = shopButtons[index];

        if (coinManager.SpendCoins(b.price))
        {
            Debug.Log($"Куплено за {b.price} монет!");
            // тут можно выдать предмет / апгрейд
        }
        else
        {
            Debug.Log("Не хватает монет");
        }

        UpdateShopButtons();
    }

    // ─────────────── UPDATE UI ───────────────
    public void UpdateShopButtons()
    {
        int coins = coinManager.GetCoins();

        foreach (ShopButton b in shopButtons)
        {
            if (b.button == null) continue;

            Image img = b.button.GetComponent<Image>();
            if (img != null)
            {
                img.color = coins >= b.price ? canBuyColor : cannotBuyColor;
            }

            b.button.interactable = coins >= b.price;
        }
    }
}
