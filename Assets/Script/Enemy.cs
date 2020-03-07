using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;

    [SerializeField]
    private int _lifePoints = 3;

    [SerializeField]
    private bool _isPlayerLocated = false;

    [SerializeField]
    private GameObject _breakPrefab;

    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _lookDirection = Vector3.down;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private UIManager _uiManager;
    private AudioSource _audioSource;

    private float _destPosition = 0;
    private float _canChangeLookTime = 0.0f;
    private float _lookTime = 2.0f;
    private float _jumpHeight = 5.0f;
    private bool _isInAir = false;
    
    // Start is called before the first frame update
    private void Start()
    {   
        _rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        // null checking component
        if (_rb == null)
        {
            Debug.LogError("RigidBody is null");
        } 

        if(_audioSource == null)
        {
            Debug.LogError("Audio Source is null");
        }

        if (_spriteRenderer == null)
        {
            Debug.LogError("Sprite Renderer is null");
        }

        if (Random.Range(0,2) == 1) // 1 = Left; 0 = Right
        {
            _lookDirection = Vector3.left;
            _moveDirection = Vector3.left;
        }
        else
        {
            _lookDirection = Vector3.right;
            _moveDirection = Vector3.right;
        }

        if(_uiManager == null)
        {
            Debug.LogError("UI Manager not found.");
        }

        StartCoroutine(JumpRoutine());
    }

    // Update is called once per frame
    private void Update()
    {
        if (Time.time > _canChangeLookTime)
        {
            ChangeDirection();
        }
    }

    private void FixedUpdate()
    {
        Movement();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _lookDirection);
        if (hit.collider != null)
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                _destPosition = hit.transform.position.x;
                _isPlayerLocated = true;

                if (hit.transform.position.y > transform.position.y)
                {
                    Jump();
                }
            }

            if (hit.transform.gameObject.tag == "Bullet")
            {
                Jump();
            }
        }
        else
        {
            _isPlayerLocated = false;

            if ((int) transform.position.x == (int) _destPosition)
            {
                ChangeDirection();
            }
            
            if (transform.position.x >= 12f || transform.position.x <= -12f)
            {
                ChangeDirection();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            ChangeDirection();
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Floor")
        {
            _isInAir = false;
        }
    }

    private void Movement()
    {        
        transform.Translate(_moveDirection * _speed * Time.deltaTime);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -12f, 14f), transform.position.y ,0);
    }

    private void ChangeDirection()
    {
        if (_isPlayerLocated == false)
        {
            _lookDirection *= -1;
            _moveDirection *= -1;
            _canChangeLookTime = Time.time + _lookTime;
        }
    }

    private void Jump()
    {
        if (_isInAir == false)
        {
            _rb.velocity = new Vector3(0, _jumpHeight, 0);
            _rb.AddForce(_rb.velocity);
            _audioSource.Play();
        }
        _isInAir = true;
    }

    IEnumerator JumpRoutine()
    {
        while(true)
        {   
            if(_isPlayerLocated == false)
            {
                Jump();
            }
            yield return new WaitForSeconds(Random.Range(3f, 5f));
        }
    }

    public void OnDamage()
    {
        _lifePoints -= 1;
        if (_lifePoints < 1)
        {
            int score = Random.Range(20,30);
            _uiManager.UpdateScore(score);
            Destroy(this.gameObject);
            GameObject newBreak = Instantiate(_breakPrefab, transform.position, Quaternion.identity);
            newBreak.transform.parent = transform.parent;
        }
    }
}
