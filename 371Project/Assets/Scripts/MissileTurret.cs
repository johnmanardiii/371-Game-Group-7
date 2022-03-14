using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTurret : BaseTurret, IPlaceable
{
    private Transform target;
    private Enemy targetEnemy;

    [Header("General")]
    private float fireCountdown = 0f;
    public GameObject projectilePrefab;

    [Header("Upgrades")]
    public float damageUpgradeAddition = 10f; // added to damage
    public float fireRateUpgradeMultiplier = 1.1f; // firerate multiplied by this

    [Header("Unity Setup Fields")]
    public Transform partToRotate;
    public float turnSpeed = 10f;
    public Transform firePoint;
    public AudioSource audioSource;
    public AudioClip audioClip;

    [HideInInspector] public RangeIndicator _rangeIndicator;

    public override string getTurretName()
    {
        return "Missile Turret";
    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        _rangeIndicator = GetComponentInChildren<RangeIndicator>();
    }

    public override void OnPlace()
    {
        DisableRangeIndicator();
    }

    public override void EnableRangeIndicator()
    {
        _rangeIndicator.gameObject.SetActive(true);
    }

    public override void DisableRangeIndicator()
    {
        _rangeIndicator.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(_enemyTag);
        float shortestDistance = Mathf.Infinity; // i.e. distance to closest enemy
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= _range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            return;
        }

        LockOnTarget();

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f); // necessary to only rotate the head in the Y direction
    }

    void Shoot()
    {
        audioSource.PlayOneShot(audioClip);

        GameObject bulletGO = (GameObject)Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile bullet = bulletGO.GetComponent<Projectile>();
        bullet.damage = baseDamage;

        if (bullet != null)
            bullet.Seek(target);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }

    public override void upgradeOption1()
    {
        upgradeDamage();
    }

    public override void upgradeOption2()
    {
        upgradeFireRate();
    }

    public override string getUpgrade1Name()
    {
        return "Upgrade Damage";
    }

    public override string getUpgrade2Name()
    {
        return "Upgrade Fire Rate";
    }

    public void upgradeDamage()
    {
        baseDamage = baseDamage + damageUpgradeAddition;
    }

    public void upgradeFireRate()
    {
        fireRate = fireRate * fireRateUpgradeMultiplier;
    }
}
