using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningBase : MonoBehaviour, IPlaceable
{
    public Material notClose;
    public Material Close;
    public MeshRenderer miningBase;
    public float range = 10.0f;
    public int moneyPerTick = 50;
    private Planet targetPlanet;
    
    public float cooldown = 1f;

    private ShopManagerScript _shopManager;
    private WaveSpawner _spawner;
    private MiningRangeIndicator _rangeIndicator;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.4f);
    }

    private void Awake()
    {
        _shopManager = FindObjectOfType<ShopManagerScript>();
        _spawner = FindObjectOfType<WaveSpawner>();
        _rangeIndicator = GetComponentInChildren<MiningRangeIndicator>();
    }

    public void OnPlace()
    {
        DisableRangeIndicator();
    }
    
    public void EnableRangeIndicator()
    {
        _rangeIndicator.gameObject.SetActive(true);
    }

    public void DisableRangeIndicator()
    {
        _rangeIndicator.gameObject.SetActive(false);
    }

    void UpdateTarget()
    {
        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
        float shortestDistance = Mathf.Infinity; // i.e. distance to closest enemy
        GameObject nearestPlanet = null;

        foreach (GameObject planet in planets)
        {
            float distanceToPlanet = Vector3.Distance(transform.position, planet.transform.position);
            if (distanceToPlanet <= shortestDistance)
            {
                nearestPlanet = planet;
                shortestDistance = distanceToPlanet;
            }
        }

        Planet nearestP = nearestPlanet.GetComponent<Planet>();
        
        if (nearestP != null && shortestDistance <= range)
        {
            if(targetPlanet == nearestP)
            {
                // don't change anythin, already connected
                return;
            }
            else
            {
                if(nearestP.ConnectMiningTurret())
                {
                    // connected to new planet, disconnect if any
                    if(targetPlanet != null)
                    {
                        targetPlanet.DisconnectMiningTurret();
                    }
                    targetPlanet = nearestP;
                    miningBase.material = Close;
                }
            }
            
            // connect to planet
            
        } 
        else
        {
            // too far away from anything            
            if(targetPlanet != null)
            {
                nearestP.DisconnectMiningTurret();
            }
            targetPlanet = null;
            miningBase.material = notClose;
        }
    }
}
