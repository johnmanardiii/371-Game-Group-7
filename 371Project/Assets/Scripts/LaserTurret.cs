using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : BaseTurret
{
    private Transform target;
    private Enemy targetEnemy;

    [Header("General")]
    public float slowPercentage = 0.25f;

    [Header("Upgrades")]
    public float damageUpgradeAddition = 10f; // added to damage
    // TODO: upgrade slow multiplier

    [Header("Laser VFX")]
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;
    public float impactEffectCreationRadius = 1f;

    [Header("Unity Setup Fields")]
    public Transform partToRotate;
    public float turnSpeed = 10f;
    public Transform firePoint;

    [HideInInspector] public RangeIndicator _rangeIndicator;

    public override string getTurretName()
    {
        return "Laser Turret";
    }

    void Awake()
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
            // remove laser if no current target
            if (lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
                impactEffect.Stop();
                impactLight.enabled = false;
            }
            return;
        }

        LockOnTarget();

        Laser();
    }

    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f); // necessary to only rotate the head in the Y direction
    }

    void Laser()
    {
        targetEnemy.TakeDamage(baseDamage * Time.deltaTime);

        Debug.Log("Doing damage: " + baseDamage * Time.deltaTime);

        targetEnemy.Slow(slowPercentage);

        // create laser if it does not already exist
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position; // vector from enemy towards turret

        impactEffect.transform.position = target.position + dir.normalized * impactEffectCreationRadius;

        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }

    //public void Slow(float slowPercentage)
    //{
    //    // TODO: add float startSpeed variable to enemy script
    //    //       add speed = startSpeed to enemy script Start() function
    //    //       add [HideInspector] to the speed variable of enemy script



    //    speed = startSpeed * (1f - slowPercentage);
    //}

    public override void upgradeOption1()
    {
        upgradeDamage();
    }

    public override string getUpgrade1Name()
    {
        return "Upgrade Damage";
    }

    public override void upgradeOption2()
    {
        upgradeSlowEffect();
    }

    public override string getUpgrade2Name()
    {
        return "Upgrade Slow";
    }

    public void upgradeDamage()
    {
        baseDamage = baseDamage + damageUpgradeAddition;
    }

    public void upgradeSlowEffect()
    {

    }

}
