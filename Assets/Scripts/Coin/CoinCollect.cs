using UnityEngine;
using System.Collections;

public class CoinCollect : MonoBehaviour
{
    public AudioClip pickupSound;

    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider2d;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2d = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CoinManager.Instance.AddCoin();
            if (_audioSource is not null && pickupSound is not null)
            {
                _audioSource.PlayOneShot(pickupSound);
                StartCoroutine(DestroyAfterSound());
            }
        }
    }

    private IEnumerator DestroyAfterSound()
    {
        _collider2d.enabled = false;
        _spriteRenderer.enabled = false;

        yield return new WaitForSeconds(pickupSound.length);

        Destroy(gameObject);
    }
}