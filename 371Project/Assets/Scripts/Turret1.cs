//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Turret : BaseTurret, IPlaceable
//{
//    private Transform target;
//    private Enemy targetEnemy;

//    [Header("General")]
//    public float baseDamage = 10f;

//    [Header("Use Projectiles (default)")]
//    public GameObject projectilePrefab;
//    public float fireRate = 1f;
//    private float fireCountdown = 0f;

//    //[Header("Use Laser")]
//    //public bool useLaser = false;

//    //public int damageOverTime = 30; // per second?
//    //public float slowPercentage = 0.5f;

//    //public LineRenderer lineRenderer;
//    //public ParticleSystem impactEffect;
//    //public Light impactLight;
//    //public float impactEffectCreationRadius = 1f;

//    [Header("Unity Setup Fields")]

//    //public string enemyTag = "Enemy";

//    public Transform partToRotate;
//    public float turnSpeed = 10f;
//    public Transform firePoint;

//    [HideInInspector] public RangeIndicator _rangeIndicator;

//    void Awake()
//    {
//        _rangeIndicator = GetComponentInChildren<RangeIndicator>();
//    }

//    public void OnPlace()
//    {
//        DisableRangeIndicator();
//    }

//    public void EnableRangeIndicator()
//    {
//        _rangeIndicator.gameObject.SetActive(true);
//    }

//    public void DisableRangeIndicator()
//    {
//        _rangeIndicator.gameObject.SetActive(false);
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//        InvokeRepeating("UpdateTarget", 0f, 0.5f);
//    }

//    void UpdateTarget()
//    {
//        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
//        float shortestDistance = Mathf.Infinity; // i.e. distance to closest enemy
//        GameObject nearestEnemy = null;

//        foreach (GameObject enemy in enemies)
//        {
//            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
//            if (distanceToEnemy < shortestDistance)
//            {
//                shortestDistance = distanceToEnemy;
//                nearestEnemy = enemy;
//            }
//        }

//        if (nearestEnemy != null && shortestDistance <= range)
//        {
//            target = nearestEnemy.transform;
//            targetEnemy = nearestEnemy.GetComponent<Enemy>();
//        } else
//        {
//            target = null;
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (target == null)
//        {
//            if (useLaser && lineRenderer.enabled)
//            {
//                lineRenderer.enabled = false;
//                impactEffect.Stop();
//                impactLight.enabled = false;
//            }
                
//            return;
//        }
            
//        LockOnTarget();
        
//        if (useLaser)
//        {
//            Laser();
//        } else
//        {
//            if (fireCountdown <= 0f)
//            {
//                Shoot();
//                fireCountdown = 1f / fireRate;
//            }
//        }

//        fireCountdown -= Time.deltaTime;
//    }

//    void LockOnTarget()
//    {
//        Vector3 dir = target.position - transform.position;
//        Quaternion lookRotation = Quaternion.LookRotation(dir);
//        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
//        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f); // necessary to only rotate the head in the Y direction
//    }

//    void Laser()
//    {
//        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);

//        // targetEnemy.Slow(slowPercentage);

//        if (!lineRenderer.enabled)
//        {
//            lineRenderer.enabled = true;
//            impactEffect.Play();
//            impactLight.enabled = true;
//        }
            
//        lineRenderer.SetPosition(0, firePoint.position);
//        lineRenderer.SetPosition(1, target.position);

//        Vector3 dir = firePoint.position - target.position; // vector from enemy towards turret

//        impactEffect.transform.position = target.position + dir.normalized * impactEffectCreationRadius;

//        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
//    }

//    void Shoot()
//    {
//        GameObject bulletGO = (GameObject)Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
//        Projectile bullet = bulletGO.GetComponent<Projectile>();
//        bullet.damage = baseDamage;

//        if (bullet != null)
//            bullet.Seek(target);
//    }

//    void OnDrawGizmosSelected() 
//    {
//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(transform.position, range);
//    }

//    //public void Slow(float slowPercentage)
//    //{
//    //    // TODO: add float startSpeed variable to enemy script
//    //    //       add speed = startSpeed to enemy script Start() function
//    //    //       add [HideInspector] to the speed variable of enemy script



//    //    speed = startSpeed * (1f - slowPercentage);
//    //}
//}
