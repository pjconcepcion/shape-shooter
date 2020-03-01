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
    private GameObject _bulletContainer;

    [SerializeField]
    private float _reloadRate = 3.0f;

    private Rigidbody2D _rb;
    private GameObject _playerGround;
    private GameManager _gameManager;

    private bool _canSecondJump = false;
    private bool _isInAir = false;
    private bool _isStomping = false;
    private int _pickupCtr = 0;
    private int ammo = 5;
    private float _reloadTime = 0;

    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.LogError("Rigidbody2D not found.");
        }

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager not found.");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        OnInput();
        if(Time.time > _reloadTime)
        {
            ammo = 5;
            _reloadTime = Time.time + _reloadRate;
        }
    }

    private void FixedUpdate()
    {
        Movement();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f);
        if (hit.collider != null)
        {
            string tag = hit.transform.gameObject.tag;
            if (hit.distance <= 0.52f && (tag == "Beam" || tag == "Floor" || tag == "Enemy"))
            {
                _canSecondJump = true;
                _isInAir = false;

                if (tag == "Beam" || tag == "Floor")
                {
                    _isStomping = false;
                }
            }

            if (tag == "Beam")
            {
                _playerGround = hit.transform.gameObject;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (_isStomping == true)
            {
                Destroy(other.gameObject);
            }
            else
            {
                OnDamage();
            }
        }
    }
    
    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");

        transform.Translate(new Vector3(horizontal, 0, 0) * _speed * Time.deltaTime);

        if(_isInAir == false)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -12f, 14f), transform.position.y, 0);
        }

        if(transform.position.y < -20f)
        {
            Destroy(this.gameObject);
            // _gameManager.SetGameOver(true);
        }
    }

    private void OnInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && _canSecondJump)
        {
            _isInAir = true;
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.S) && _playerGround != null)
        {
            Beam beam = _playerGround.GetComponent<Beam>();

            if (beam == null)
            {
                Debug.Log("Beam not found.");
            }

            beam.OnBeamDisabled();
            OnFall(_floorFallSpeed);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isStomping = true;
            OnFall(_fallSpeed);
        }
    }

    private void Jump()
    {
        if (_isInAir == true)
        {
            _canSecondJump = false;
        }

        _rb.velocity = new Vector3(0, _jumpHeight, 0);
        _rb.AddForce(_rb.velocity);
    }

    private void OnFall(float speed)
    {        
        _rb.velocity = Vector3.down * speed;
        _rb.AddForce(_rb.velocity);
    }

    private void Shoot()
    {
        if(ammo > 0)
        {
            Vector3 mousePosition = Input.mousePosition - UnityEngine.Camera.main.WorldToScreenPoint(transform.position);
            mousePosition.z = 0;

            float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
            int bulletNumber = Random.Range(0,5);
            GameObject newBullet = Instantiate(_bulletPrefab[bulletNumber], transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
            newBullet.transform.parent = _bulletContainer.transform;
            ammo -= 1;
        }
    }

    public void SetGround(GameObject beam)
    {
        _playerGround = beam;
    }

    public void OnPickup()
    {
        _pickupCtr += 1;

        if (_pickupCtr > 5)
        {
            _pickupCtr = 0;

            if (_lifePoints < 5)
            {
                _lifePoints += 1;
            }
        }
    }

    public void OnDamage()
    {
        _lifePoints -= 1;

        if (_lifePoints < 1)
        {
            Destroy(this.gameObject);
        }
    }
}
