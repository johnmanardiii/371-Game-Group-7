using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningRangeIndicator : MonoBehaviour
{
    private MiningBase _mine;
    // Start is called before the first frame update
    void Awake()
    {
        _mine = GetComponentInParent<MiningBase>();
    }

    void Update()
    {
        float objScale = _mine.range * 1.92f;
        transform.localScale = new Vector3(objScale, objScale, objScale);
    }
}
