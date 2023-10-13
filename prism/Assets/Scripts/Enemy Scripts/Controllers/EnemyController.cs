using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public ChaseMovement chaseScript;
    public PlayerAwarenessController playerAwarenessScript;

    public bool chaseBehaviorEnabled = true;
    public bool wanderBehaviorEnabled = false;
    public bool shootBehaviorEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
