using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.Composites;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class FastForwardButton : MonoBehaviour
{
    public TextMeshProUGUI ffText;
    public Image button;
    private bool fastFowardOn = false;

    private void Awake()
    {
        UpdateButton();
    }

    private void UpdateButton()
    {
        if (fastFowardOn)
        {
            button.color = Color.white;
            Time.timeScale = 2.5f;
            ffText.text = "Fast Forward: ON";
        }
        else
        {
            button.color = Color.red;
            Time.timeScale = 1.0f;
            ffText.text = "Fast Forward: OFF";
        }
    }

    public void OnFastFowarwardClick()
    {
        fastFowardOn = !fastFowardOn;
        UpdateButton();
    }
}
