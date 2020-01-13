using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _floor;

    [SerializeField]
    private GameObject _floorContainer;

    private bool _isGameReady = false;

    // Start is called before the first frame update
    private void Start()
    {
        int index = Random.Range(0, _floor.Length - 1);
        float x = -11f;
        for(int ctr = 0; ctr < 9; ctr++){
            GameObject newFloor = Instantiate(_floor[index], new Vector3(x, 0, 0), Quaternion.identity);
            newFloor.transform.parent = _floorContainer.transform;
            newFloor.tag = "Floor";
            x += 3;
        }

        Time.timeScale = 0;
    }

        // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !_isGameReady)
        {
            _isGameReady = true;
            Time.timeScale = 1;
        }
    }
}
