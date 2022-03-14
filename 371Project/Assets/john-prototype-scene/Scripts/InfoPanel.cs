using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoPanel : MonoBehaviour
{
    public BaseTurret turretRef = null;
    public MiningBase mineRef = null;

    public TextMeshProUGUI Name;
    public TextMeshProUGUI FireRate;
    public TextMeshProUGUI Range;
    public TextMeshProUGUI BaseDamage;
    
    public TextMeshProUGUI Upgrade1Text;
    public TextMeshProUGUI Upgrade2Text;

    public Image upgradeOneResource, upgradeTwoResource;
    public Sprite coinImage, rubyImage, diamondImage, oilImage;

    private ShopManagerScript shop;

    private void Awake()
    {
        shop = FindObjectOfType<ShopManagerScript>();
    }

    public void UpdateText()
    {
        FireRate.text = "Fire Rate: " + turretRef.fireRate.ToString();
        Range.text = "Range: " + turretRef._range.ToString();
        BaseDamage.text = "Base Damage: " + turretRef.baseDamage.ToString();
    }

    public void SetTurret(BaseTurret turret)
    {
        // deselect old turret if any:
        DeselectAll();

        turretRef = turret;
        Name.text = turretRef.getTurretName();
        UpdateText();

        Upgrade1Text.text = turretRef.getUpgrade1Name();
        Upgrade2Text.text = turretRef.getUpgrade2Name();

        turret.EnableRangeIndicator();

        switch(turret.resourceType)
        {
            case Planet.PlanetResourceType.COIN:
                upgradeOneResource.sprite = coinImage;
                upgradeTwoResource.sprite = coinImage;
                break;
            case Planet.PlanetResourceType.RUBY:
                upgradeOneResource.sprite = rubyImage;
                upgradeTwoResource.sprite = rubyImage;
                break;
            case Planet.PlanetResourceType.DIAMOND:
                upgradeOneResource.sprite = diamondImage;
                upgradeTwoResource.sprite = diamondImage;
                break;
            case Planet.PlanetResourceType.OIL:
                upgradeOneResource.sprite = oilImage;
                upgradeTwoResource.sprite = oilImage;
                break;
        }
    }

    public void SetMine(MiningBase mine)
    {
        // deselect old turret if any:
        DeselectAll();

        mineRef = mine;
        Name.text = "Mining Tower";
        FireRate.text = "";
        Range.text = "Range: " + mineRef.range.ToString();
        BaseDamage.text = "";

        Upgrade1Text.text = "";
        Upgrade2Text.text = "";

        mineRef.EnableRangeIndicator();
    }

    public void DeselectAll()
    {
        if(turretRef != null)
        {
            turretRef.DisableRangeIndicator();
            turretRef = null;
        }
        if(mineRef != null)
        {
            mineRef.DisableRangeIndicator();
            mineRef = null;
        }
    }

    public void UpgradeOne()
    {
        if(shop.Purchase(100, turretRef.resourceType))
        {
            turretRef.upgradeOption1();
        }
        UpdateText();
    }

    public void UpgradeTwo()
    {
        if(shop.Purchase(100, turretRef.resourceType))
        {
            turretRef.upgradeOption2();
        }
        UpdateText();
    }

    public void OnSell()
    {
        shop.AddResource(turretRef.GetSellAmount(), Planet.PlanetResourceType.COIN);
        Destroy(turretRef.gameObject);
        turretRef = null;
        mineRef = null;

        // somehow disable this menu
    }
}
