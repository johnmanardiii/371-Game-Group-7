using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFire : MonoBehaviour
{
    public GameObject laser;
    public float coolDown = .1f;
    private float timeSinceFire = 0.0f;

    private void FireLaser(PathEnemy enemy)
    {
        Vector3 currentPos = transform.position;
        TurretProjectile spawnedLaser = Instantiate(laser, currentPos,
            Quaternion.LookRotation(enemy.transform.position - currentPos)).GetComponent<TurretProjectile>();
        spawnedLaser.StartChasing(currentPos, enemy);
    }
    
    private void OnTriggerStay(Collider other)
    {
        // assume it is enemy:
        if (timeSinceFire < 0)
        {
            PathEnemy enemy = other.GetComponent<PathEnemy>();
            if (enemy == null)
            {
                return;
            }
            FireLaser(enemy);
            timeSinceFire = coolDown;
        }
    }

    private void Update()
    {
        timeSinceFire -= Time.deltaTime;
    }
}
