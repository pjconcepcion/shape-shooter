using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImage : MonoBehaviour
{
    public void OnShoot()
    {
        Destroy(this.gameObject);
    }
}
