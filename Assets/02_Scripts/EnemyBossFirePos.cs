using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossFirePos : MonoBehaviour
{
    public GameObject obj;
    public Transform firePos;

    void Fire()
    {
        // Calculate the forward direction based on the fire position's rotation
        Vector3 forwardDirection = firePos.forward;

        // Instantiate the object with the forward direction
        GameObject projectile = Instantiate(obj, firePos.position, Quaternion.LookRotation(forwardDirection));

        // Apply a force to the projectile to make it move forward
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        if (projectileRigidbody != null)
        {
            projectileRigidbody.AddForce(forwardDirection * 1000f);
        }
    }
}
