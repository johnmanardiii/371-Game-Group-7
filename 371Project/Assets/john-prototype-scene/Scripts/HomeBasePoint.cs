using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBasePoint : MonoBehaviour
{
    public int health = 100;
    public GameObject LoseText;
    public ParticleSystem system;
    public AudioSource source;

    private void OnTriggerEnter(Collider other)
    {
        // must be enemy
        if (health > 0)
        {
            health -= 5;
        }
        checkHealth();
    }

    private void checkHealth()
    {
        if (health == 0)
        {
            StartCoroutine(waitbeforedestroy());
        }
    }

    IEnumerator waitbeforedestroy()
    {
        system.Play();
        source.Play();
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0;
        LoseText.SetActive(true);
    }
}
