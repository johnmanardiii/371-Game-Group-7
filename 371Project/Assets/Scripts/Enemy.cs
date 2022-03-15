using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public enum EnemyType
{
    REGULAR, BEEFY, FAST, MOAB
}

public class Enemy : MonoBehaviour
{
    private PathCreator pathCreator;

    public float startSpeed = 5f;
    [HideInInspector] public float speed;

    [HideInInspector] public float distanceTravelled;
    public float health = 50;
    public ParticleSystem system;
    public AudioSource source;
    public AudioSource source2;
    private bool check = true;

    public EnemyType type;
    public PathCreator PathCreator
    {
        set => pathCreator = value;
    }

    private void Start()
    {
        speed = startSpeed;
    }

    private void Update()
    {
        if (pathCreator == null)
        {
            return;
        }
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);

        speed = startSpeed; // reset speed if enemy is no longer being slowed
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Reactor"))
        {
            Debug.Log("Enemy reached reactor");
            ReactorDie();
        }
    }

    public void TakeDamage(float dmgAmount)
    {
        health -= dmgAmount;

        if (health <= 0f && check)
        {
            check = false;
            Die();
        }
    }


    public void Slow(float slowPercentage)
    {
        if (speed == startSpeed) // if enemy is not already slowed
        {
            speed = startSpeed * (1f - slowPercentage);
        }
    }

    // make this more efficient if its a problem lol
    public void Die()
    {
        source.Play();
        StartCoroutine(particleDestroy());
        var spawner = FindObjectOfType<WaveSpawner>();  // this should rlly be singleton since so many enemies
        spawner._enemiesInPlay--;
        if (spawner._enemiesInPlay == 0 && !spawner.spawningEnemies)
        {
            spawner.WaveEnded();    // spawner will check if it is still spawning when no enemies in case spawn kill)
        }
    }

    public void ReactorDie()
    {
        source2.Play();
        StartCoroutine(reactorHit());
        var spawner = FindObjectOfType<WaveSpawner>();  // this should rlly be singleton since so many enemies
        spawner._enemiesInPlay--;
        if (spawner._enemiesInPlay == 0 && !spawner.spawningEnemies)
        {
            spawner.WaveEnded();    // spawner will check if it is still spawning when no enemies in case spawn kill)
        }
    }

    IEnumerator particleDestroy()
    {
        system.Play();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        Debug.Log("Enemy just died.");
    }

    IEnumerator reactorHit()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        Debug.Log("Enemy died from hitting reactor.");
    }
}
