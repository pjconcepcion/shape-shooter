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
    private bool _isGameOver = false;
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    private void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager not found.");
        }

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

        if(_isGameOver)
        {
            Time.timeScale = 0;
            _spawnManager.SetGameOver(_isGameOver);
        }
    }

    public void SetGameOver(bool isGameOver)
    {
        _isGameOver = isGameOver;
    }
}
