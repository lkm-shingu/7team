using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTrap : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    private float nextFireTime = 0f;

    void Update()
    {
        if (Time.time > nextFireTime)
        {
            FireProjectile();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FireProjectile()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
}

