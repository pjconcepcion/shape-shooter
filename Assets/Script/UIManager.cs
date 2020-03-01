using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

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

    private int _score = 0;
    private Image[] _bulletsImage = new Image[5];

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + _score;
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
