using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public int coins = 0;
    public TextMeshProUGUI coinText;
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
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
        coins++;
        coinText.text = "Coins: " + coins;
        Debug.Log(coins);
    }
}