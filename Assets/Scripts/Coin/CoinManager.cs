using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public int coins = 0;
    public TextMeshProUGUI coinText;

    void Awake()
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

    void Start()
    {
        if (coinText == null)
        {
            Debug.LogError("CoinText is not assigned in the inspector.");
        }
    }

    public void AddCoin()
    {
        if (coinText != null)
        {
            coins++;
            coinText.text = "Coins: " + coins;
            Debug.Log(coins);
        }
        else
        {
            Debug.LogError("CoinText is not assigned.");
        }
    }
}