using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButtonBad : MonoBehaviour
{
    private CursorController cursor;
    private Image _button;

    private void Awake()
    {
        _button = GetComponent<Image>();
        cursor = FindObjectOfType<CursorController>();
    }

    void Update()
    {
        if (cursor == null)
        {
            return;
        }
    }
}
