using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Planet : MonoBehaviour
{
    public Sprite coinImage, rubyImage, diamondImage, oilImage;

    //public Image 

    public enum PlanetResourceType
    {
        COIN, RUBY, DIAMOND, OIL
    }

    public PlanetResourceType resourceType;

    public int amountPerMiningTowerPerRound = 25;

    public int maxMiningConnections = 3;

    public int miningConnections = 0;

    public Image resourceImage;
    public TextMeshProUGUI amountText;
    public TextMeshProUGUI maxConnectionsText;

    private ShopManagerScript _shopManager;
    private WaveSpawner _waveSpawner;
    public GameObject PanelUI;

    void SetUIImage()
    {
        switch(resourceType)
        {
            case PlanetResourceType.COIN:
                resourceImage.sprite = coinImage;
                break;
            case PlanetResourceType.DIAMOND:
                resourceImage.sprite = diamondImage;
                break;
            case PlanetResourceType.RUBY:
                resourceImage.sprite = rubyImage;
                break;
            case PlanetResourceType.OIL:
                resourceImage.sprite = oilImage;
                break;
        }
    }

    void SetText()
    {
        amountText.text = "Amount Per Mining Tower Per Round: " + amountPerMiningTowerPerRound;
        maxConnectionsText.text = "Connections: " + miningConnections + "/" + maxMiningConnections;
    }

    private void Awake()
    {
        miningConnections = 0;
        _shopManager = FindObjectOfType<ShopManagerScript>();
        _waveSpawner = FindObjectOfType<WaveSpawner>();
        SetUIImage();
        SetText();
    }
    
    public bool ConnectMiningTurret()
    {
        if(miningConnections == maxMiningConnections)
        {
            return false;
        }
        else
        {
            miningConnections++;
            SetText();
            return true;
        }
    }

    public void DisconnectMiningTurret()
    {
        miningConnections--;
        SetText();
    }

    void OnEnable()
    {
        _waveSpawner.OnWaveStart += OnWaveStart;
    }

    void OnDisable()
    {
        _waveSpawner.OnWaveStart -= OnWaveStart;
    }

    void OnWaveStart()
    {
        StartCoroutine(GetMoneyOverRound());
    }

    IEnumerator GetMoneyOverRound()
    {
        // calculate how much of resource to give the player.
        int amount = amountPerMiningTowerPerRound * miningConnections;
        //Debug.Log("getting money" + amount);
        int i = 0;
        while(i < amount)
        {
            _shopManager.AddResource(1, resourceType);
            i++;
            yield return new WaitForSeconds(.1f);
        }
    }

    public void EnableUI()
    {
        PanelUI.SetActive(true);
        SetText();
    }

    public void DisableUI()
    {
        PanelUI.SetActive(false);
    }
}
