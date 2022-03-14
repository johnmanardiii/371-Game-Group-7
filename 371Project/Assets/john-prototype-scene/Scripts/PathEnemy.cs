using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class PathEnemy : MonoBehaviour
{
    private Follower _follower;
    public int damage = 1;
    
    private void Awake()
    {
        _follower = GetComponent<Follower>();
    }
    
    
}
