using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

/*
 * To River: I basically made this the game manager class lol
 * feel free to move that stuff out or tell me to if its too cluttered.
 */
public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemySubWave
    {
        public EnemyPath wavePath;
        public float delayForNextWave;
        public GameObject enemy;
        public int count;
        public float enemySpeed;
    }
    
    [System.Serializable]
    public class Wave
    {
        public EnemySubWave[] subWaves;
        public int moneyForWave;
    }

    public Wave[] waves;
    public int _nextWave = 0;
    [HideInInspector] public int _enemiesInPlay = 0;
    private ShopManagerScript _shopManager;
    
    public GameObject winText;

    public bool waveActive = false;
    public bool spawningEnemies = false;

    public WavePreview path1;
    public WavePreview path2;
    public WavePreview path3;

    private CursorController _cursor;

    public Action OnWaveStart;

    public AudioSource source;

    private void Awake()
    {
        _cursor = FindObjectOfType<CursorController>();
        _shopManager = FindObjectOfType<ShopManagerScript>();
        EnableRoundText();
    }

    private void Update()
    {
        if (_nextWave == waves.Length && _enemiesInPlay <= 0 && !spawningEnemies)
        {
            winText.SetActive(true);
        }
    }

    public void WaveEnded()
    {
        if (spawningEnemies)
        {
            return;
        }
        Debug.Log("WE should get here!");
        Debug.Log("Money: " + waves[_nextWave - 1].moneyForWave);
        source.Play();
        _shopManager.AddResource(waves[_nextWave - 1].moneyForWave, Planet.PlanetResourceType.COIN);
        waveActive = false;
        EnableRoundText();
    }

    private void DisableRoundText()
    {
        path1.gameObject.SetActive(false);
        path2.gameObject.SetActive(false);
        path3.gameObject.SetActive(false);
    }

    private void SetPathText()
    {
        
    }

    private void EnableRoundText()
    {
        if(_nextWave == waves.Length)
        {
            return;
        }
        
        path1.ResetCount();
        path2.ResetCount();
        path3.ResetCount();
        
        
        foreach(var _subWave in waves[_nextWave].subWaves)
        {
            if(_subWave.wavePath.id == 1)
            {
                path1.AddCount(_subWave.count, _subWave.enemy.GetComponent<Enemy>().type);
                //path1 += _subWave.count;
            }
            else if(_subWave.wavePath.id == 2)
            {
                path2.AddCount(_subWave.count, _subWave.enemy.GetComponent<Enemy>().type);
                //path2 += _subWave.count;
            }
            else
            {
                path3.AddCount(_subWave.count, _subWave.enemy.GetComponent<Enemy>().type);
                //path3 += _subWave.count;
            }
        }
        path1.gameObject.SetActive(true);
        path2.gameObject.SetActive(true);
        path3.gameObject.SetActive(true);
        // set the text of all objects to display number
        path1.UpdateText();
        path2.UpdateText();
        path3.UpdateText();
    }

    public void SpawnNextWave()
    {
        _cursor._currentlyHeldArm = null;
        if (_enemiesInPlay <= 0 && _nextWave < waves.Length)
        {
            waveActive = true;
            spawningEnemies = true;
            StartCoroutine(SpawnWave(waves[_nextWave]));
            OnWaveStart();
            _nextWave++;
        }
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        foreach (var _subWave in _wave.subWaves)
        {
            yield return new WaitForSeconds(_subWave.delayForNextWave);
            StartCoroutine(SpawnSubwave(_subWave));
            
        }
        spawningEnemies = false;
    }


    IEnumerator SpawnSubwave(EnemySubWave _wave)
    {
        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy, _wave);
            yield return new WaitForSeconds(.2f);    // completely random value change later
        }
    }

    void SpawnEnemy(GameObject _enemy, EnemySubWave _wave)
    {
        Debug.Log("Spawning Enemy: " + _enemy.name);
        Enemy enemy = Instantiate(_enemy, Vector3.up * 1000, Quaternion.identity).GetComponent<Enemy>();
        // set path of enemy
        enemy.PathCreator = _wave.wavePath.EnemyPathObject;
        _enemiesInPlay++;
        if (enemy != null)
        {
            enemy.startSpeed = _wave.enemySpeed;
        }
    }
    
    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }
}
