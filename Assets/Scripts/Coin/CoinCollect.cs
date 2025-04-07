using System.Threading;
using UnityEngine;
using System;
using System.Collections;
using System.Threading.Tasks;

public class CoinCollect : MonoBehaviour
{
    public AudioClip pickupSound;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Collider2D collider2d;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2d = GetComponent<Collider2D>();
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
        }
    }

    private IEnumerator DestroyAfterSound()
    {
        collider2d.enabled = false;
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(pickupSound.length);

        Destroy(gameObject);
    }
}