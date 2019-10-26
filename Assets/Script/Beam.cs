using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    private BoxCollider _boxCollider;
    private GameObject _player;

    private bool _isPlayerGoingDown = false;

    // Start is called before the first frame update
    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _player = GameObject.Find("Player");

        if (_boxCollider == null)
        {
            Debug.LogError("BoxCollider not found.");
        }

        if (_player == null)
        {
            Debug.LogError("Player not found.");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (_player.transform.position.y < transform.position.y)
        {
            _boxCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
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
            else
            {
                _isPlayerGoingDown = false;
            }
        }
    }

    public void OnBeamDisabled()
    {
        _isPlayerGoingDown = true;
        _boxCollider.isTrigger = true;
    }
}
