using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public ChaseMovement chaseScript;
    public PlayerAwarenessController playerAwarenessScript;
    public WanderMovement wanderScript;
    public ShootBehavior shootScript;
    public bool chaseBehaviorEnabled = false;
    public bool wanderBehaviorEnabled = false;
    public bool shootBehaviorEnabled = false;

    public PlayerController playerControllerScript;

    private bool onBeat = false;
    private bool firstFrameOnBeat = true;
    private int curBeatCount = 0;
    private int maxBeatCount = 7;   // fire every 8 beats

    public bool enemyIsChaser;
    public bool enemyIsShooter;

    // Start is called before the first frame update
    void Start()
    {
        if (enemyIsChaser && !enemyIsShooter)
            enableChase();
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

            // I'm debuggin here :)
            // print("On beat: " + curBeatCount);

            // CHASER BEHAVIOR
            if (enemyIsChaser && !enemyIsShooter)
            {
                // Nothing needed, chase behavior enabled in Start()
            }

            // SHOOTER BEHAVIOR
            else if (enemyIsShooter && !enemyIsChaser)
            {
                if (curBeatCount == 4)
                    enableShoot();
                else if (curBeatCount == 6)
                    enableWander(); // shoot behavior lasts for two beats
            }

            // CHASER-SHOOTER BEHAVIOR
            else
            {
                if (curBeatCount == 0)
                    enableShoot();
                else if (curBeatCount == 2)
                    enableChase(); // shoot behavior lasts for two beats
            }
            

            if (curBeatCount == 7)
                curBeatCount = 0;
            else
                curBeatCount++;
        }
    }

    void enableChase()
    {
        wanderScript.disableBehavior();
        shootScript.disableBehavior();
        chaseScript.enableBehavior();
    }

    void enableWander()
    {
        chaseScript.disableBehavior();
        shootScript.disableBehavior();
        wanderScript.enableBehavior();
    }

    void enableShoot()
    {
        chaseScript.disableBehavior();
        wanderScript.disableBehavior();
        shootScript.enableBehavior();
        shootScript.enemyPos = gameObject.transform.position;
    }
}
