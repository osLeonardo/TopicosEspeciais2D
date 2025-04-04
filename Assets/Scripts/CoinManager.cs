using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public int coins = 0;
    public TextMeshProUGUI coinText; // Texto na UI para mostrar as moedas
    void Start()
    {

    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void AddCoin()
    {
        Debug.Log('a');

        coins++;
        coinText.text = "Coins: " + coins;
        Debug.Log(coins);
    }
}