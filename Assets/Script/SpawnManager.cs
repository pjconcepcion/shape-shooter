using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _beamPrefab;

    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] _pickupPrefab;

    [SerializeField]
    private GameObject _pickupContainer;

    [SerializeField]
    private int _prefabIndex;
    
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(BeamRoutine());
        StartCoroutine(EnemyRoutine());
        StartCoroutine(PickupRoutine());
    }

    // Update is called once per frame
    private void Update()
    {
    }

    IEnumerator BeamRoutine()
    {
        while(true)
        {
            Vector3 position = new Vector3(Random.Range(-11f, 11f), Random.Range(2f, 5f), 0);
            GameObject newBeam = Instantiate(_beamPrefab[_prefabIndex], position, Quaternion.identity);
            newBeam.tag = "Beam";
            newBeam.AddComponent<Beam>();
            yield return new WaitForSeconds(Random.Range(5f, 10f));
        }
    }

    IEnumerator EnemyRoutine()
    {
        while(true)
        {
            Vector3 position =  new Vector3(Random.Range(-11f, 11f), Random.Range(5f, 8f), 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(Random.Range(3f, 5f));
        }
    }

    IEnumerator PickupRoutine()
    {
        while(true)
        {
            Vector3 position = new Vector3(Random.Range(-11f,11f), Random.Range(2f, 4f), 0);
            int pickupIndex = Random.Range(0,_pickupPrefab.Length);
            GameObject newPickup = Instantiate(_pickupPrefab[pickupIndex], position, Quaternion.identity);
            newPickup.transform.parent = _pickupContainer.transform;
            yield return new WaitForSeconds(Random.Range(4f,6f));
        }
    }
}
