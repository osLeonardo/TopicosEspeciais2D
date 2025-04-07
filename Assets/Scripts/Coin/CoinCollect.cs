using System;
using System.Threading;
using UnityEngine;
using System;
using System.Collections;
using System.Threading.Tasks;

public class CoinCollect : MonoBehaviour
{
    public AudioClip pickupSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing from this game object");
        }
        if (pickupSound == null)
        {
            Debug.LogError("Pickup sound not assigned in the inspector");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CoinManager.instance.AddCoin();
            if (audioSource != null && pickupSound != null)
            {
                audioSource.PlayOneShot(pickupSound);
                StartCoroutine(DestroyAfterSound());
            }
            else
            {
                Debug.LogWarning("AudioSource or pickupSound is missing");
            }
        }
    }
    private IEnumerator DestroyAfterSound()
    {
        yield return new WaitForSeconds(pickupSound.length);
        Destroy(gameObject);
    }
}