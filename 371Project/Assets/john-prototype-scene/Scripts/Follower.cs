using System;
using UnityEngine;
using PathCreation;

public class Follower : MonoBehaviour
{
    private PathCreator pathCreator;
    public float speed = 5;
    [HideInInspector] public float distanceTravelled;

    private void Awake()
    {
        pathCreator = FindObjectOfType<PathCreator>();
    }

    private void Update()
    {
        if (pathCreator == null)
        {
            return;
        }
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
    }
}
