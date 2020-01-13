using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float _speed = 15.0f;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector3.right * _speed * Time.deltaTime);

        if (transform.position.x >= 23.0f || transform.position.x <= -23.0f || transform.position.y >= 15.0f || transform.position.y <= -10.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if (enemy == null)
            {
                Debug.LogError("Enemy not found");
            }

            enemy.OnDamage();
            Destroy(this.gameObject);
        }

        if (other.gameObject.tag == "Beam" || other.gameObject.tag == "Floor")
        {
            Destroy(this.gameObject);
        }
    }
}
