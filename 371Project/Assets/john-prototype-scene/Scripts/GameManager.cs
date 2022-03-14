using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Enemy;
    public float timeBetweenSpawns = 1.0f;
    public int numEnemiesThisRound = 3;
    private int numEnemiesSpawned = 0;

    private bool _roundStarted = false;
    private bool _roundEnded = false;
    public GameObject winText;

    public void StartGame()
    {
        if (!_roundStarted)
        {
            _roundStarted = true;
            StartCoroutine(SpawnEnemies());
        }
    }

    /*
     * Enemies should most likely be tracked in a queue. This is because physics is likely inconsistent and a lot
     * can be precalculated if not using physics.
     */
    private IEnumerator SpawnEnemies()
    {
        while (numEnemiesSpawned < numEnemiesThisRound)
        {
            Instantiate(Enemy, new Vector3(0, -1000, 0), Quaternion.identity);
            ++numEnemiesSpawned;
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
    
    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }

    private void Update()
    {
        if (_roundEnded)
        {
            return;
        }
        // absolutely terrible way of tracking this but im sleepy
        if (_roundStarted && FindObjectsOfType<PathEnemy>().Length == 0)
        {
            winText.SetActive(true);
            _roundEnded = true;
        }
    }
}
