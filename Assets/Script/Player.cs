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
    private float _jumpHeight = 5.0f;

    private Rigidbody rb;

    private bool _canJump = true;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody not found.");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        Movement();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W) && _canJump)
        {
            rb.velocity = new Vector3(0, _jumpHeight, 0);
            rb.AddForce(rb.velocity);
            _canJump = false;
        }
    }

    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");

        transform.Translate(new Vector3(horizontal, 0, 0) * _speed * Time.deltaTime);
        if(_canJump)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -12f, 12f), transform.position.y, 0);
        }

        if (Physics.Raycast(transform.position, Vector3.down, 0.5f))
        {
            _canJump = true;
        }
    }

    
}
