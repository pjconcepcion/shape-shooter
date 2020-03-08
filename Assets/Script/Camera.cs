using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private GameObject _player;
    // Start is called before the first frame update
    private void Start()
    {
        _player = GameObject.Find("Player");
        
        if (_player == null)
        {
            Debug.LogError("Player not found.");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (_player != null)
        {
            transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y + 2, -9);
        }
    }
}
