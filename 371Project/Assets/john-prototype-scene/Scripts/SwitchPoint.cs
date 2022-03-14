using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPoint : MonoBehaviour, IClickable
{
    [HideInInspector] public SwitchManager _manager;
    
    // on click, set this one to active:
    public bool isActive = true;
    private MeshRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    public void SetMaterial(Material m)
    {
        _renderer.material = m;
    }

    public void OnClicked()
    {
        _manager.SetSwitch(this);
    }
}
