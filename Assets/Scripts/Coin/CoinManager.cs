using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    public int coins;
    public GameObject coinsParent;
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
            if (coins < 10)
            {
                coinText.text = "Coins: 0" + coins;
            }
            else
            {
                coinText.text = "Coins: " + coins;
            }

            if (coinsParent.transform.childCount == 1)
            {
                StartCoroutine(LoadWinScreenAfterDelay());
            }
        }
    }

    private IEnumerator LoadWinScreenAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("WinScreen");
    }
}