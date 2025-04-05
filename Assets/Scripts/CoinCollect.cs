using System;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    public AudioClip pickupSound;
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other.CompareTag("Player"));
            if (other.CompareTag("Player"))
            {
                CoinManager.instance.AddCoin();
                if (audioSource != null && pickupSound != null)
                {
                    audioSource.PlayOneShot(pickupSound);
                }
                Destroy(gameObject);
            }
        }
}
