using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float _speed = 11.5f;

    [SerializeField]
    private AudioClip _audioShoot;

    [SerializeField]
    private AudioClip _audioHit;

    private AudioSource _audioSource;
    private bool _isAlreadyHit = false;
    // Start is called before the first frame update
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if(_audioSource == null)
        {
            Debug.LogError("Audio Source not found.");
        }

        _audioSource.PlayOneShot(_audioShoot);
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector3.right * _speed * Time.deltaTime);

        if (transform.position.x >= 23.0f || transform.position.x <= -23.0f || transform.position.y >= 15.0f || transform.position.y <= -10.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if (enemy == null)
            {
                Debug.LogError("Enemy not found");
            }

            enemy.OnDamage();
            OnHit();
        }

        if (other.gameObject.tag == "Floor" && !_isAlreadyHit)
        {
            _isAlreadyHit = true;
            OnHit();
        }
    }

    private void OnHit()
    {
        _audioSource.PlayOneShot(_audioHit);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Collider2D collider = GetComponent<Collider2D>();

        if(spriteRenderer == null)
        {
            Debug.LogError("Sprite Renderer not found.");
        }

        if(collider == null)
        {
            Debug.LogError("Collider not found.");
        }

        collider.enabled = false;
        spriteRenderer.enabled = false;
        Destroy(this.gameObject,1.0f);
    }
}
