using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public Transform newPathPointPos;
    public GameObject pathArmPrefab;
    private WaveSpawner _waveSpawner;
    private ShopManagerScript _shopManager;
    private int price = 150;

    private void Awake()
    {
        _waveSpawner = FindObjectOfType<WaveSpawner>();
        _shopManager = FindObjectOfType<ShopManagerScript>();
    }

    public void ExtendPath()
    {
        if (_waveSpawner._enemiesInPlay > 0)
        {
            return;
        }

        if (_shopManager.Purchase(price, Planet.PlanetResourceType.COIN))
        {
            var arm = Instantiate(pathArmPrefab, newPathPointPos);
            PathArm createdArm = arm.GetComponent<PathArm>();
            newPathPointPos = createdArm.nextArmSpawnPoint;
        }
    }
}
