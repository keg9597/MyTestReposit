using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RimoveBullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Bullet")
        {
            Destroy(collision.gameObject);
        }
    }
}
