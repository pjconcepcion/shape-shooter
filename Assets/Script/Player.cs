using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int _lifePoints = 5;

    [SerializeField]
    private float _speed = 5.0f;

    [SerializeField]
    private float _jumpHeight = 5.5f;

    [SerializeField]
    private float _fallSpeed = 20.0f;

    [SerializeField]
    private float _floorFallSpeed = 15.0f;

    [SerializeField]
    private GameObject[] _bulletPrefab;

    [SerializeField]
    private AudioClip _audioJump;

    [SerializeField]
    private AudioClip _audioDamage;

    [SerializeField]
    private AudioClip _audioStomp;

    [SerializeField]
    private AudioClip _audioLife;

    [SerializeField]
    private AudioClip _audioPickup;

    [SerializeField]
    private GameObject _bulletContainer;

    [SerializeField]
    private float _reloadRate = 3.0f;

    [SerializeField]
    private GameObject[] _bullets = new GameObject[5];

    private Rigidbody2D _rb;
    private GameObject _playerGround;
    private GameManager _gameManager;
    private UIManager _uiManager;
    private AudioSource _audioSource;

    private bool _canSecondJump = false;
    private bool _isInAir = false;
    private bool _isStomping = false;
    private int _pickupCtr = 0;
    private int _maxAmmo = 5;
    private int _ammo = 0;
    private float _reloadTime = 0;
    private int _score = 10;
    private int _maxLifePoints = 5;

    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if(_rb == null)
        {
            Debug.LogError("Rigidbody2D not found.");
        }

        if(_audioSource == null)
        {
            Debug.LogError("Audio Source not found.");
        }

        if(_gameManager == null)
        {
            Debug.LogError("Game Manager not found.");
        }

        if(_uiManager == null)
        {
            Debug.LogError("UI Manager not found.");
        }

        Reload();
    }

    // Update is called once per frame
    private void Update()
    {
        if(_gameManager.GetIsGameReady() && !_gameManager.GetIsGamePause())
        {
            OnInput();
        }

        if(Time.time > _reloadTime)
        {
            _reloadTime = Time.time + _reloadRate;  
            if(_ammo == 0)
            {
                Reload();
            }
        }
    }

    private void FixedUpdate()
    {
        CheckCollision();
        Movement();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            if(_isStomping == true)
            {
                _uiManager.UpdateScore(_score);                
                _gameManager.AddKillCount();    
                Destroy(other.gameObject);
            }
            else
            {
                OnDamage();
            }
        }       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Pickup")
        {
            OnPickup();
            Destroy(other.gameObject);
        }
    }
    
    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");

        transform.Translate(new Vector3(horizontal, 0, 0) * _speed * Time.deltaTime);

        if(transform.position.y < -20f)
        {
            Destroy(this.gameObject);
            _gameManager.SetGameOver(true);
        }
    }

    private void CheckCollision()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f);
        if(hit.collider != null)
        {
            string tag = hit.transform.gameObject.tag;
            if(hit.distance <= 0.52f && (tag == "Floor" || tag == "Enemy"))
            {
                _canSecondJump = true;
                _isInAir = false;

                if (tag == "Floor")
                {
                    _isStomping = false;
                }
            }
        }
    }

    private void OnInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && _canSecondJump)
        {
            Jump();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isStomping = true;
            _audioSource.PlayOneShot(_audioStomp);
            OnFall(_fallSpeed);
        }
    }

    private void Jump()
    {
        _audioSource.PlayOneShot(_audioJump, 0.6f);
        _rb.velocity = new Vector3(0, _jumpHeight, 0);
        _rb.AddForce(_rb.velocity);
        
        _isInAir = true;
        if (_isInAir == true)
        {
            _canSecondJump = false;
        }
    }

    private void OnFall(float speed)
    {        
        _rb.velocity = Vector3.down * speed;
        _rb.AddForce(_rb.velocity);
    }

    private void Shoot()
    {
        if(_ammo > 0)
        {
            Vector3 mousePosition = Input.mousePosition - UnityEngine.Camera.main.WorldToScreenPoint(transform.position);
            mousePosition.z = 0;
            _ammo -= 1;

            float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
            GameObject newBullet = Instantiate(_bullets[_ammo], transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
            newBullet.transform.parent = _bulletContainer.transform;

            _uiManager.RemoveBullet(_ammo);
        }
    }

    private void Reload()
    {
        for(int ctr = 0; ctr < _maxAmmo; ctr++ )
        {
            int bulletNumber = Random.Range(0,5);
            _bullets[ctr] = _bulletPrefab[bulletNumber];
            _uiManager.AddBullet(bulletNumber, ctr);
        }

        _reloadTime = Time.time + _reloadRate;
        _ammo = _maxAmmo;
    }

    public void OnPickup()
    {
        _audioSource.PlayOneShot(_audioPickup);
        
        _pickupCtr += 1;

        if (_pickupCtr == 3)
        {
            _pickupCtr = 0;

            if (_lifePoints < _maxLifePoints)
            {
                _lifePoints += 1;
                _uiManager.UpdateLife(_lifePoints);
                _audioSource.PlayOneShot(_audioLife);
            }
        }
    }

    public void OnDamage()
    {
        _lifePoints -= 1;
        _uiManager.UpdateLife(_lifePoints);
        _audioSource.PlayOneShot(_audioDamage);
        
        if (_lifePoints < 1)
        {
            Destroy(this.gameObject);
            _gameManager.SetGameOver(true);
        }
    }
}
