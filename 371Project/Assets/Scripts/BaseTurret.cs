using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTurret : MonoBehaviour, IPlaceable
{
    public float _range = 30f;
    public float fireRate = 1f;
    public float baseDamage = 30f;

    protected int sellAmount = 50;
    [SerializeField] protected string _attackPriorityMode = "closest";

    public Planet.PlanetResourceType resourceType;

    protected string _enemyTag = "Enemy";

    public abstract string getTurretName();

    public abstract void upgradeOption1();

    public abstract void upgradeOption2();

    public abstract void EnableRangeIndicator();
    public abstract void DisableRangeIndicator();

    public abstract string getUpgrade1Name();
    public abstract string getUpgrade2Name();

    public abstract void OnPlace();

    public int GetSellAmount()
    {
        return sellAmount;
    }
}
