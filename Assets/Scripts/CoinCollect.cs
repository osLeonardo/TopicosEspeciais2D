using System;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    public AudioClip pickupSound;
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Log(audioSource != null);
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource não encontrado na moeda!");
        }
    }
    
    void Update()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other.CompareTag("Player"));
            if (other.CompareTag("Player")) // Verifica se o jogador tocou na moeda
            {
                CoinManager.instance.AddCoin(); // Atualiza o contador de moedas
                Debug.Log(audioSource != null && pickupSound != null);
                if (audioSource != null && pickupSound != null)
                {
                    audioSource.PlayOneShot(pickupSound);
                }
                Destroy(gameObject);
            }
        }
}
