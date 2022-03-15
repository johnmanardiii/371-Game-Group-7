using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WavePreview : MonoBehaviour
{
    public TextMeshProUGUI regularText;
    public TextMeshProUGUI beefyText;
    public TextMeshProUGUI fastText;
    public TextMeshProUGUI moabText;

    private int regCount, beefCount, fastCount, moabCount;
    
    public void ResetCount()
    {
        regCount = beefCount = fastCount = moabCount = 0;
    }

    public void UpdateText()
    {
        SetText(EnemyType.REGULAR);
        SetText(EnemyType.BEEFY);
        SetText(EnemyType.FAST);
        SetText(EnemyType.MOAB);
    }

    public void AddCount(int count, EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.REGULAR:
                regCount += count;
                break;
            case EnemyType.BEEFY:
                beefCount += count;
                break;
            case EnemyType.FAST:
                fastCount += count;
                break;
            case EnemyType.MOAB:
                moabCount += count;
                break;
            default:
                Debug.Log("Enemy Type not found!!!");
                break;
        }
    }

    private void SetText(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.REGULAR:
                regularText.text = "x" + regCount;
                break;
            case EnemyType.BEEFY:
                beefyText.text = "x" + beefCount;
                break;
            case EnemyType.FAST:
                fastText.text = "x" + fastCount;
                break;
            case EnemyType.MOAB:
                moabText.text = "x" + moabCount;
                break;
            default:
                Debug.Log("Enemy Type not found!!!");
                break;
        }
    }
}
