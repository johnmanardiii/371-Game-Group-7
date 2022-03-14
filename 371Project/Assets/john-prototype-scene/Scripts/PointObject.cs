using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObject : MonoBehaviour
{
    protected static Material immovableMat, enabledMat, disabledMat;
    protected MeshRenderer _renderer;
    public virtual Transform GetTransform()
    {
        return transform;
    }

    private void Awake()
    {
        if (immovableMat == null)
        {
            immovableMat = Resources.Load("Materials/ImmovablePoint") as Material;
            enabledMat = Resources.Load("Materials/PathPointEnabled") as Material;
            disabledMat = Resources.Load("Materials/PathPointDisabled") as Material;
        }

        _renderer = GetComponent<MeshRenderer>();
        _renderer.material = immovableMat;
    }
}
