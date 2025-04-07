using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public int coins = 0;
    public TextMeshProUGUI coinText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Multiple instances of CoinManager detected. There should only be one instance.");
            Destroy(gameObject);
        }
    }

    public void AddCoin()
    {
        if (coinText != null)
        {
            coins++;
            coinText.text = "Coins: " + coins;
        }
    }
}