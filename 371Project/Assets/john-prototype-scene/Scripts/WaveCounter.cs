using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveCounter : MonoBehaviour
{
    public WaveSpawner spawner;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text.text = "Wave " + spawner._nextWave + " / 25";
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Wave " + spawner._nextWave + " / 25";
    }
}
