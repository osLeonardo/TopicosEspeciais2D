using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public int coins;
    public TextMeshProUGUI coinText;
    public static CoinManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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