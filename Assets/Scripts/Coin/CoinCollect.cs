using UnityEngine;
using System.Collections;

public class CoinCollect : MonoBehaviour
{
    public AudioClip pickupSound;

    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider2d;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2d = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        CoinManager.Instance.AddCoin();
        if (_audioSource is null || pickupSound is null) return;

        _audioSource.PlayOneShot(pickupSound);
        StartCoroutine(DestroyAfterSound());
    }

    private IEnumerator DestroyAfterSound()
    {
        _collider2d.enabled = false;
        _spriteRenderer.enabled = false;

        yield return new WaitForSeconds(pickupSound.length);

        Destroy(gameObject);
    }
}