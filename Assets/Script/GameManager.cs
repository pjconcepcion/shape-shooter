using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _floor;

    [SerializeField]
    private GameObject _floorContainer;

    [SerializeField]
    private GameObject _pauseMenuPanel;

    [SerializeField]
    private GameObject _helpPanel;

    [SerializeField]
    private AudioClip _audioGameOver;

    [SerializeField]
    private AudioClip _audioGameStart;

    private string MAIN_MENU = "MainMenu";

    private bool _isGameReady = false;  
    private bool _isGameOver = false;
    private bool _isGamePause = false;
    private bool _isPlayGameOver = false;
    private bool _isNextLevel = false;
    private int _spawnCount = 5;
    private int _spawnRate = 5;
    private int _playerKillCount = 0;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    private void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        Background background = GameObject.Find("Background").GetComponent<Background>();

        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager not found.");
        }

        if(_uiManager == null)
        {
            Debug.LogError("UI Manager not found.");
        }

        if(_audioSource == null)
        {
            Debug.LogError("Audio Source not found.");
        }

        if(background == null)
        {
            Debug.LogError("Background not found.");
        }
        
        SetGameFloor();
        background.SetBackground();

        Time.timeScale = 0;
        _pauseMenuPanel.SetActive(_isGamePause);
        _helpPanel.SetActive(_isGamePause);
        _spawnManager.NextLevel(_spawnCount);
    }

    // Update is called once per frame
    private void Update()
    {
        if(!_isGameOver)
        {
            if(_isGameReady)
            {
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    if(_isGamePause)
                    {
                        _uiManager.ResumeGame();
                    }
                    else
                    {
                        PauseGame(KeyCode.Escape);
                    }
                }

                if(Input.GetKeyDown(KeyCode.H))
                {
                    if(_isGamePause)
                    {
                        _uiManager.ResumeGame();
                    }
                    else
                    {
                        PauseGame(KeyCode.H);
                    }
                }
            }
            else{
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    _isGameReady = true;
                    _audioSource.PlayOneShot(_audioGameStart);
                    _uiManager.StartGame();
                    Time.timeScale = 1f;                    
                }

                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    QuitGame();
                }
            }
        }
        else
        {
            GameOver();
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

    private void GameOver()
    {
        PauseGame(KeyCode.Escape);
        Time.timeScale = 0f;
        _uiManager.GameOver();

        if(!_isPlayGameOver)
        {
            _audioSource.PlayOneShot(_audioGameOver, 0.45f);
            _isPlayGameOver = true;
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

    public bool GetIsGameReady()
    {
        return _isGameReady;
    }

    public void ResumeGame()
    {
        SetGamePause(false);
        _pauseMenuPanel.SetActive(_isGamePause);
        _helpPanel.SetActive(_isGamePause);
        Time.timeScale = 1f;
    }

    public void PauseGame(KeyCode key)
    {
        _uiManager.PauseGame();
        SetGamePause(true);

        if(key == KeyCode.Escape)
        {
            _pauseMenuPanel.SetActive(_isGamePause);
        }
        else{
            _helpPanel.SetActive(_isGamePause);
        }
        
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(MAIN_MENU);
    }

    public void AddKillCount()
    {
        _playerKillCount += 1;
        Debug.Log(_playerKillCount);

        if(_playerKillCount == _spawnCount)
        {
            _playerKillCount = 0;
            _spawnCount += _spawnRate;
            _isNextLevel = true;
            _spawnManager.NextLevel(_spawnCount);
            _pauseMenuPanel.SetActive(true);
            _uiManager.NextLevel();
            Time.timeScale = 0f;
        }
    }
}
