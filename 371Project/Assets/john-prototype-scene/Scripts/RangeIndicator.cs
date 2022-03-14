using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
    private BaseTurret _turret;
    // Start is called before the first frame update
    void Awake()
    {
        _turret = GetComponentInParent<BaseTurret>();
    }

    void Update()
    {
        float objScale = _turret._range * 1.92f;
        transform.localScale = new Vector3(objScale, objScale, objScale);
    }
}
