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

    private Rigidbody _rb;
    private GameObject _playerGround;

    private bool _canFirstJump = true;
    private bool _canSecondJump = true;

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

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 0.5f))
        {
            _canFirstJump = true;
            _canSecondJump = true;
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
        }
    }

    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");

        transform.Translate(new Vector3(horizontal, 0, 0) * _speed * Time.deltaTime);

        if(_canFirstJump)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -12f, 12f), transform.position.y, 0);
        }
    }

    private void Jump()
    {
        if (_canFirstJump == false)
        {
            _canSecondJump = false;
        }

        _rb.velocity = new Vector3(0, _jumpHeight, 0);
        _rb.AddForce(_rb.velocity);
        _canFirstJump = false;
    }

    public void SetGround(GameObject beam)
    {
        _playerGround = beam;
    }
}
