using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;

    public float damage;
    public float speed = 70f;
    public float explosionRadius = 0f;
    public GameObject impactEffect;
    public AudioClip impactAudio;

    private Vector3 targetLastPosition;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir;
        float distanceThisFrame;

        // if enemy dies before projectile reaches it
        if (target == null) 
        {
            // missile continues to last known target location, then explodes
            if (explosionRadius > 0f)
            {
                dir = targetLastPosition - transform.position;
                distanceThisFrame = speed * Time.deltaTime;

                // if target location reached, explode
                if (dir.magnitude <= distanceThisFrame)
                {
                    HitTarget();
                    return;
                }

                transform.Translate(dir.normalized * distanceThisFrame, Space.World); // normalizing makes sure we move at constant speed
                transform.LookAt(target);
            } 
            
            // bullet destroys itself
            else
            {
                Destroy(gameObject);
            }
  
            return;
        }

        // else target is still valid
        targetLastPosition = target.position; // save target position in case target dies before projectile reaches it

        dir = target.position - transform.position;
        distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World); // normalizing makes sure we move at constant speed
        transform.LookAt(target);
    }

    void HitTarget()
    {
        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 5f); // might move this to the impact effect for modularity

        if (explosionRadius > 0f)
        {
            Explode();
        } else
        {
            Damage(target);
        }

        Destroy(gameObject);
    }

    void Explode()
    {
        AudioSource.PlayClipAtPoint(impactAudio, this.gameObject.transform.position);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius); // gets array of colliders foudn within sphere
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Damage(collider.transform);
            }
        }
    }

    void Damage(Transform enemy)
    {
        enemy.gameObject.GetComponent<Enemy>().TakeDamage(damage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
