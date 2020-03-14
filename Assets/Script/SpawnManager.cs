using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _beamPrefab;

    [SerializeField]
    private GameObject[] _enemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] _pickupPrefab;

    [SerializeField]
    private GameObject _pickupContainer;

    [SerializeField]
    private int _prefabIndex;
    
    private bool _isGameOver = false;
    private int _enemySpawnCount = 0;
    private int _pickupSpawnCount = 0;
    private int _maxSpawn = 0;
    private float _speed = 5.0f;
    private float _speedRate = 0.5f;
    private float _minSpawnRate = 3f;
    private float _maxSpawnRate = 5f;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(EnemyRoutine());
        StartCoroutine(PickupRoutine());
    }

    // Update is called once per frame
    private void Update()
    {
    }

    IEnumerator EnemyRoutine()
    {
        while(!_isGameOver)
        {
            yield return new WaitForSeconds(Random.Range(_minSpawnRate, _maxSpawnRate));
            
            if(_enemySpawnCount < _maxSpawn){
                Vector3 position =  new Vector3(Random.Range(-11f, 11f), Random.Range(5f, 8f), 0);
                int enemyNumber = Random.Range(0, _enemyPrefab.Length);
                GameObject newEnemy = Instantiate(_enemyPrefab[enemyNumber], position, Quaternion.identity);            
                newEnemy.transform.parent = _enemyContainer.transform;
                Enemy enemy = newEnemy.GetComponent<Enemy>();
                
                if(enemy == null)
                {
                    Debug.LogError("Enemy not found.");
                }
                else
                {
                    enemy.SetSpeed(_speed);
                }
                _enemySpawnCount += 1;
            }
        }
    }

    IEnumerator PickupRoutine()
    {
        while(!_isGameOver)
        {
            yield return new WaitForSeconds(Random.Range(_minSpawnRate, _maxSpawnRate));

            if(_pickupSpawnCount < _maxSpawn){
                Vector3 position = new Vector3(Random.Range(-11f,11f), Random.Range(2f, 4f), 0);
                int pickupIndex = Random.Range(0,_pickupPrefab.Length);
                GameObject newPickup = Instantiate(_pickupPrefab[pickupIndex], position, Quaternion.identity);
                newPickup.transform.parent = _pickupContainer.transform;

                _pickupSpawnCount += 1;
            }
        }
    }

    public void SetGameOver(bool isGameOver)
    {
        _isGameOver = isGameOver;
    }

    public void NextLevel(int maxSpawn)
    {
        _maxSpawn = maxSpawn;
        _enemySpawnCount = 0;
        _pickupSpawnCount = 0;
        _speed += _speedRate;
        _minSpawnRate -= 0.25f;
        _maxSpawnRate -= 0.25f;
    }
}
