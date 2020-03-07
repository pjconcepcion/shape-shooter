using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{    
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(DestroyRoutine());
    }

    

    IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(this.gameObject);
    }
}
