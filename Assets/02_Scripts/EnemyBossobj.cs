using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossobj : MonoBehaviour
{
    public int damage;
    public float force = 100f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * force);
        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            damage = 10;
            Destroy(gameObject);
        }
    }
}
