using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    private BoxCollider2D _boxCollider;
    private GameObject _player;
    private Vector3 _direction;

    private float _speed = 3.0f;
    private bool _isPlayerGoingDown = false;

    // Start is called before the first frame update
    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _player = GameObject.Find("Player");

        if (_boxCollider == null)
        {
            Debug.LogError("BoxCollider not found.");
        }

        if (_player == null)
        {
            Debug.LogError("Player not found.");
        }

        if (Random.Range(0,2) == 1)
        {
            _direction = Vector3.left;
        }
        else
        {
            _direction = Vector3.right;
        }

        StartCoroutine(DestroyRoutine());   
    }

    // Update is called once per frame
    private void Update()
    {
        if (_player.transform.position.y <= transform.position.y)
        {
            _boxCollider.isTrigger = true;
            _isPlayerGoingDown = false;
        }

        transform.Translate(_direction * _speed * Time.deltaTime);

        if (transform.position.x >= 23.0f || transform.position.x <= -23.0f || transform.position.y >= 15.0f || transform.position.y <= -10.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.transform.position.y >= transform.position.y && _isPlayerGoingDown == false)
            {
                Player player = other.gameObject.GetComponent<Player>();
                if (player == null)
                {
                    Debug.LogError("Player not found.");
                }
                player.SetGround(this.gameObject);
                _boxCollider.isTrigger = false;
            }
        }

        if (other.gameObject.tag == "Enemy")
        {            
            if (other.transform.position.y >= transform.position.y)
            {
                _boxCollider.isTrigger = false;
            }
        }
    }

    IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(5.0f);        
        Destroy(this.gameObject);
    }

    public void OnBeamDisabled()
    {
        _isPlayerGoingDown = true;
        _boxCollider.isTrigger = true;
    }
}
