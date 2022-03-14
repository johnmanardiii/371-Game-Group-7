using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    private Vector3 _startPos;
    private PathEnemy target;
    private Transform enemyTransform;
    public float chaseDuration = .1f;
    
    public void StartChasing(Vector3 startPos, PathEnemy enemy)
    {
        target = enemy;
        _startPos = startPos;
        enemyTransform = enemy.transform;
        StartCoroutine(ChaseEnemy());
    }

    public IEnumerator ChaseEnemy()
    {
        float currentDuration = 0;
        while (currentDuration <= chaseDuration)
        {
            if (enemyTransform == null || target == null)
            {
                Destroy(gameObject);
                yield break;
            }
            currentDuration += Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(enemyTransform.position - _startPos);
            transform.position = Vector3.Slerp(_startPos, enemyTransform.position, currentDuration / chaseDuration);
            yield return null;
        }
        if (enemyTransform == null || target == null)
        {
            Destroy(gameObject);
        }
        // Deal damage or destroy enemy
        if(target.gameObject != null) {Destroy(target.gameObject);}
        Destroy(gameObject);
    }
}
