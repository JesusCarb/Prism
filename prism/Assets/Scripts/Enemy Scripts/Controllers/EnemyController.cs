using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public ChaseMovement chaseScript;
    public PlayerAwarenessController playerAwarenessScript;
    // public WanderBehavior wanderScript;
    public ShootBehavior shootScript;
    public bool chaseBehaviorEnabled = true;
    public bool wanderBehaviorEnabled = false;
    public bool shootBehaviorEnabled = false;

    public PlayerController playerControllerScript;

    private bool onBeat = false;
    private bool firstFrameOnBeat = true;
    private int curBeatCount = 0;
    private int maxBeatCount = 7;   // fire every 8 beats

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // SWAP BEHAVIORS DEPENDING ON BEAT
        onBeat = playerControllerScript.OnBeat();

        if (!onBeat)
        {
            firstFrameOnBeat = true;
        }
        
        if (onBeat && firstFrameOnBeat)
        {
            firstFrameOnBeat = false;

            print("On beat: " + curBeatCount);

            if (curBeatCount == 0)
                enableShoot();
            else if (curBeatCount == 2)
                enableChase(); // shoot behavior lasts for two beats
            // TODO: ^ should be wander behavior, replace

            if (curBeatCount == 7)
                curBeatCount = 0;
            else
                curBeatCount++;
        }
    }

    void enableChase()
    {
        //wanderScript.disableBehavior();
        shootScript.disableBehavior();
        chaseScript.enableBehavior();
    }

    void enableWander()
    {
        chaseScript.disableBehavior();
        shootScript.disableBehavior();
        //wanderScript.enableBehavior();
    }

    void enableShoot()
    {
        chaseScript.disableBehavior();
        //wanderScript.disableBehavior();
        shootScript.enableBehavior();
    }
}
