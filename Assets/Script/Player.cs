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
    private GameObject _bulletPrefab;

    [SerializeField]
    private GameObject _bulletContainer;

    private Rigidbody _rb;
    private GameObject _playerGround;
    private RaycastHit _hit;

    private bool _canSecondJump = true;
    private bool _isInAir = false;    

    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        if (_rb == null)
        {
            Debug.LogError("Rigidbody not found.");
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void FixedUpdate()
    {
        Movement();

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out _hit))
        {
            if (_hit.distance == 0.5f && (_hit.transform.gameObject.tag != "Beam" || _hit.transform.gameObject.tag != "Floor"))
            {
                _canSecondJump = true;
                _isInAir = false;
            }

            if (_hit.transform.gameObject.tag == "Beam")
            {
                _playerGround = _hit.transform.gameObject;
            }
        }

        if (Input.GetKeyDown(KeyCode.W) && _canSecondJump)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.S))
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
            OnFall(_fallSpeed);
        }
    }

    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");

        transform.Translate(new Vector3(horizontal, 0, 0) * _speed * Time.deltaTime);

        if(_isInAir == false)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -12f, 12f), transform.position.y, 0);
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
        _isInAir = true;
    }

    private void OnFall(float speed)
    {        
        _rb.velocity = Vector3.down * speed;
        _rb.AddForce(_rb.velocity);
    }

    private void Shoot()
    {
        Vector3 mousePosition = Input.mousePosition - UnityEngine.Camera.main.WorldToScreenPoint(transform.position);
        mousePosition.z = 0;

        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        GameObject newBullet = Instantiate(_bulletPrefab, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
        newBullet.transform.parent = _bulletContainer.transform;
    }

    public void SetGround(GameObject beam)
    {
        _playerGround = beam;
    }
}
