using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class EnemyPath : MonoBehaviour
{
    private PathCreator _enemyPath;
    private RoadMeshCreator _meshCreator;
    private HomeBasePoint _lastPoint;

    private Vector3[] _pathPoints;

    public PathCreator EnemyPathObject => _enemyPath;

    public int id = 0;

    private void Awake()
    {
        _lastPoint = FindObjectOfType<HomeBasePoint>();
    }

    private void Start()
    {
        _enemyPath = GetComponent<PathCreator>();
        _meshCreator = GetComponent<RoadMeshCreator>();
        
        // generate path from other game object that contains a list of points, some moveable, some not.
        UpdatePath();
    }

    /*
     * Creates a list of points from pathPointsObject in the order they are in the heirarchy.
     * uses transforms from children.
     */
    void GetPointsByOrder()
    {
        PointObject[] temp = GetComponentsInChildren<PointObject> () as PointObject[];
        _pathPoints = new Vector3[temp.Length + 1];
        int index = 0;
        int noOfChildren = transform.childCount;
        for (int i=0; i< noOfChildren;i++)
        {
            PointObject childComponent = transform.GetChild(i).GetComponent<PointObject>();
            if(childComponent!=null)
            {
                _pathPoints[index] = childComponent.GetTransform().position;
                index++;
            }
        }

        _pathPoints[_pathPoints.Length - 1] = _lastPoint.transform.position;
    }

    /*
     * This method assumes _pathPoints is updated with the most recent path transforms. This should be called
     * every time a path point is changed / moved
     */
    public void UpdatePath()
    {
        GetPointsByOrder();
        if (_pathPoints.Length > 0) {
            // Create a new bezier path from the waypoints.
            BezierPath bezierPath = new BezierPath(_pathPoints, false, PathSpace.xz);
            GetComponent<PathCreator>().bezierPath = bezierPath;
        }
        
        _meshCreator.UpdatePath();
    }

    public void OnEnemyPathChanged()
    {
        
    }
}
