using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyButtonInfo : MonoBehaviour
{
    private ShopManagerScript _shopManager;
    private TextMeshProUGUI costText;
    public Item item;

    private CursorController _cursor;

    private void Awake()
    {
        _shopManager = FindObjectOfType<ShopManagerScript>();
        _cursor = FindObjectOfType<CursorController>();
        costText = GetComponentInChildren<TextMeshProUGUI>();

        costText.text = item.price.ToString();
    }

    public void BuyTower()
    {
        if(_shopManager.CanBuy(item.price, Planet.PlanetResourceType.COIN))
        {
            _cursor.BuyItem(item.towerToSpawn, item.price);
        }
    }
}
