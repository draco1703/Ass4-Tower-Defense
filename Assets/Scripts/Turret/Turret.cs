using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [HideInInspector]
    public ObjectPool pool;
    private Transform target;
    private Enemy targetEnemy;
    [Header("Attributes")]
    public float range = 15f;

    [Header("Use Bullets (default)")]

    public string poolDicTag;
    public bool turretUpgraded;
    public float fireRate = 1f;
    private float fireCountDown = 0f;

    [Header("Use Laser")]
    public bool useLaser = false;

    public int damageOverTime = 50;
    public float slowAmount = .5f;

    public LineRenderer lineRenderer;
    public ParticleSystem LaserImpactEffect;
    public Light impactLight;


    [Header("unity setup fields")]

    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 10;
    public Transform firePoint;
    
 
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

   
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (var enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        if(nearestEnemy !=null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }
    }

  
    void Update()
    {
        
        if (target == null)
        {
            if (useLaser)
            {
                if (lineRenderer.enabled)
                { 
                    lineRenderer.enabled = false;
                    LaserImpactEffect.Stop();
                    impactLight.enabled = false;
                }
            }
            return;
        }


        LockOnTarget();

        if (useLaser)
        {
            Laser();
        }
        else
        {
            if (fireCountDown <= 0f)
            {
                Shoot();
                fireCountDown = 1f / fireRate;
            }
            fireCountDown -= Time.deltaTime;
        }

      
    }

    void LockOnTarget()
    {
        //target lockon
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

    }
    void Laser ()
    {
        targetEnemy.GetComponent<Enemy>().TakkeDamager(damageOverTime * Time.deltaTime);
        targetEnemy.Slow(slowAmount);

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            LaserImpactEffect.Play();
            impactLight.enabled = true;
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 LaserDir = firePoint.position - target.position;

        LaserImpactEffect.transform.position = target.position + LaserDir.normalized;

        LaserImpactEffect.transform.rotation = Quaternion.LookRotation(LaserDir);

    }

    //pass target to bullet 
    void Shoot()
    {
        //  GameObject bulletGo = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        GameObject bulletGo = ObjectPool.Instance.SpawnFromPool(poolDicTag, firePoint.transform.position, firePoint.transform.rotation);
        Bullet bullet = bulletGo.GetComponent<Bullet>();
        if (turretUpgraded)
        {
            bullet.damage = 70;
        }
        if (bullet != null)
            bullet.Seek(target);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
