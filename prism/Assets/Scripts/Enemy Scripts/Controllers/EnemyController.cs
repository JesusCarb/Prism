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

    private Rigidbody2D _rigidbody;
    private Vector2 _targetDirection;
    private float _speed;

    private GameObject playerObject;
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

        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerControllerScript = playerObject.GetComponent<PlayerController>();
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _targetDirection = transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Sqrt(_rigidbody.velocity.x * _rigidbody.velocity.x + _rigidbody.velocity.y * _rigidbody.velocity.y) < 10)
        {
            _speed = _rigidbody.velocity.x * _rigidbody.velocity.x + _rigidbody.velocity.y * _rigidbody.velocity.y;
        }

        // SWAP BEHAVIORS DEPENDING ON BEAT
        if (playerControllerScript != null)
        {
            onBeat = playerControllerScript.OnBeat();
        }
        

        if (!onBeat)
        {
            firstFrameOnBeat = true;
        }
        
        if (onBeat && firstFrameOnBeat)
        {
            print("BEAT TIME");
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
                {
                    enableShoot();
                }
                    
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

    // Doesn't work ;-;
    /* private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            if (_rigidbody.velocity.x < 0.1f)
            {
                print("enemy hit left/right wall");
                print("start: " + _rigidbody.rotation);

                BounceOffWall();

                print("end: " + _rigidbody.rotation);
            }
            if (_rigidbody.velocity.y < 0.1f)
            {
                print("enemy hit top/bottom wall");
                print("start: " + _rigidbody.rotation);

                BounceOffWall();

                print("end: " + _rigidbody.rotation);
            }
        }
    } */

    // Doesn't work ;-;
    /* void BounceOffWall()
    {
        Quaternion rotation = Quaternion.AngleAxis(180f, transform.forward);
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _targetDirection);
        _targetDirection = rotation * _targetDirection;

        Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 10000f);
        _rigidbody.SetRotation(newRotation);

        _rigidbody.velocity = transform.up * _speed;
    } */

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
