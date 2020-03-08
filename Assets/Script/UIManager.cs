using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _scorePauseText;

    [SerializeField]
    private Text _pauseText;

    [SerializeField]
    private Text _startText;

    [SerializeField]
    private Image _lifeImage;

    [SerializeField]
    private Sprite[] _lifeSprite;

    [SerializeField]
    private Sprite[] _bulletSprite;

    [SerializeField]
    private Image _bulletImage;

    [SerializeField]
    private GameObject _bulletImageContainer;

    [SerializeField]
    private Button _resumeButton;

    private int _score = 0;
    private bool _isGameOver = false;
    private Image[] _bulletsImage = new Image[5];

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + _score;
    }

    public void UpdateScore(int score)
    {
        _score += score;
        _scoreText.text = "Score: "  + _score;
    }

    public void UpdateLife(int currentLife)
    {
        _lifeImage.sprite = _lifeSprite[currentLife];
    }

    public void AddBullet(int bulletNumber, int counter)
    {
        Vector3 position = new Vector3(-35f * (counter + 1), -32.7f, 0.0f);
        Image newImage = Instantiate(_bulletImage, position, Quaternion.identity);
        newImage.sprite = _bulletSprite[bulletNumber];
        newImage.transform.SetParent(_bulletImageContainer.transform, false);
        _bulletsImage[counter] = newImage;
    }

    public void RemoveBullet(int counter)
    {
        BulletImage bulletImage = _bulletsImage[counter].GetComponent<BulletImage>();

        if(bulletImage == null)
        {
            Debug.LogError("Bullet Image not found.");
        }
        else
        {
            bulletImage.OnShoot();
        }
    }

    public void StartGame()
    {
        _startText.gameObject.SetActive(false);
    }

    public void ResumeGame()
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        if(gameManager == null)
        {
            Debug.LogError("Game Manager not found.");
        }

        if(!_isGameOver)
        {
            gameManager.ResumeGame();
        }
        else{
            gameManager.RestartGame();
        }
        
        _scoreText.gameObject.SetActive(true);
    }

    public void PauseGame()
    {
        _pauseText.text = "Game Paused!";
        _scorePauseText.text = _scoreText.text;
        _scoreText.gameObject.SetActive(false);
        _resumeButton.GetComponentInChildren<Text>().text = "Resume";
    }

    public void QuitGame()
    {        
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if(gameManager == null)
        {
            Debug.LogError("Game Manager not found.");
        }

        gameManager.QuitGame();
    }

    public void GameOver()
    {
        _isGameOver = true;
        _pauseText.text = "Game Over!";
        _scorePauseText.text = _scoreText.text;
        _resumeButton.GetComponentInChildren<Text>().text = "Restart";
    }
}
