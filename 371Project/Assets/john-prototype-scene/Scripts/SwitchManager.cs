using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class SwitchManager : PointObject
{
    private SwitchPoint[] _switchPoints;
    public SwitchPoint activePoint;
    private EnemyPath _path;
    private void Awake()
    {
        _switchPoints = GetComponentsInChildren<SwitchPoint>();
        
        // set manager in children to this one:
        // and initialize switch points
        foreach(SwitchPoint point in _switchPoints)
        {
            point._manager = this;
            if (point.isActive)
            {
                activePoint = point;
            }
        }

        _path = FindObjectOfType<EnemyPath>();
    }

    private void Start()
    {
        // set materials
        foreach(SwitchPoint point in _switchPoints)
        {
            if (point.isActive)
            {
                point.GetComponent<MeshRenderer>().material = enabledMat;
            }
            else
            {
                point.GetComponent<MeshRenderer>().material = disabledMat;
            }
        }
    }

    public void SetSwitch(SwitchPoint point)
    {
        // set materials
        activePoint.SetMaterial(disabledMat);
        point.SetMaterial(enabledMat);
        activePoint = point;
        // update some stuff
        
        // calculate new path
        _path.UpdatePath();
    }

    public override Transform GetTransform()
    {
        return activePoint.transform;
    }
}
