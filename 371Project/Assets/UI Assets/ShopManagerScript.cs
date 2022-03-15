using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ShopManagerScript : MonoBehaviour
{
    private int coins = 500;
    private int rubies = 0;
    private int diamonds = 0;
    private int oil = 0;
    public TextMeshProUGUI CoinsText, RubyText, DiamondText, OilText;

    void Start()
    {
        UpdateText();
    }

    void UpdateText()
    {
        CoinsText.text = coins.ToString();
        RubyText.text = rubies.ToString();
        DiamondText.text = diamonds.ToString();
        OilText.text = oil.ToString();
    }

    public bool CanBuy(int price, Planet.PlanetResourceType resourceType)
    {
        return price <= coins;
    }

    public void AddResource(int toAdd, Planet.PlanetResourceType resourceType)
    {
        switch(resourceType)
        {
            case Planet.PlanetResourceType.COIN:
                coins += toAdd;
                Debug.Log("ToAdd: " + toAdd + "coins now: " + coins);
                break;
            case Planet.PlanetResourceType.RUBY:
                rubies += toAdd;
                break;
            case Planet.PlanetResourceType.DIAMOND:
                diamonds += toAdd;
                break;
            case Planet.PlanetResourceType.OIL:
                oil += toAdd;
                break;
        }
        UpdateText();
    }

    public bool Purchase(int price, Planet.PlanetResourceType resourceType)
    {
        switch(resourceType)
        {
            case Planet.PlanetResourceType.COIN:
                if(price > coins)
                {
                    return false;
                }
                else
                {
                    coins -= price;
                }
                break;
            case Planet.PlanetResourceType.RUBY:
                if(price > rubies)
                {
                    return false;
                }
                else
                {
                    rubies -= price;
                }
                break;
            case Planet.PlanetResourceType.DIAMOND:
                if(price > diamonds)
                {
                    return false;
                }
                else
                {
                    diamonds -= price;
                }
                break;
            case Planet.PlanetResourceType.OIL:
                if(price > oil)
                {
                    return false;
                }
                else
                {
                    oil -= price;
                }
                break;
        }

        UpdateText();

        return true;
    }
}
