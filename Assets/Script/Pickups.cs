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

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            player.OnPickup();
            Destroy(this.gameObject);
        }
    }

    IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(this.gameObject);
    }
}
