using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _floor;

    [SerializeField]
    private GameObject _floorContainer;

    [SerializeField]
    private GameObject _pauseMenuPanel; 

    private bool _isGameReady = false;  
    private bool _isGameOver = false;
    private bool _isGamePause = false;
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    private void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager not found.");
        }
        
        SetGameFloor();

        Time.timeScale = 0;
        _pauseMenuPanel.SetActive(_isGamePause);
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !_isGameReady)
        {
            _isGameReady = true;
            Time.timeScale = 1f;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(_isGamePause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if(_isGameOver)
        {
            Time.timeScale = 0f;
            _spawnManager.SetGameOver(_isGameOver);
        }
    }

    private void SetGameFloor()
    {    
        int index = Random.Range(0, _floor.Length - 1);
        float x = -11f;
        for(int ctr = 0; ctr < 9; ctr++){
            GameObject newFloor = Instantiate(_floor[index], new Vector3(x, 0, 0), Quaternion.identity);
            newFloor.transform.parent = _floorContainer.transform;
            newFloor.tag = "Floor";
            x += 3;
        }
    }

    public void SetGameOver(bool isGameOver)
    {
        _isGameOver = isGameOver;
    }

    public void SetGamePause(bool isGamePause)
    {
        _isGamePause = isGamePause;
    }

    public bool GetIsGamePause()
    {
        return _isGamePause;
    }

    public void ResumeGame()
    {
        SetGamePause(false);
        _pauseMenuPanel.SetActive(_isGamePause);
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {        
        UIManager _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _uiManager.PauseGame();

        SetGamePause(true);
        _pauseMenuPanel.SetActive(_isGamePause);
        Time.timeScale = 0f;
    }
}
