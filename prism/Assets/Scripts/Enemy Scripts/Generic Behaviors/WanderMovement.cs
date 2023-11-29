using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderMovement : MonoBehaviour
{
    private bool behaviorEnabled = true;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _rotationSpeed;

    private Rigidbody2D _rigidbody;
    private PlayerAwarenessController _playerAwarenessController;
    private Vector2 _targetDirection;
    private float _changeDirectionCooldown;
    private bool collidedWithWall = false;

    float wanderLength;


    private Vector2 storeVector;
    
    // Awake is called when the script/entity loads in and becomes active
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
        _targetDirection = transform.up;
        wanderLength = 0f;
    }

    // FixedUpdate works better with the physics system
    private void FixedUpdate()
    {
        wanderLength -= Time.fixedDeltaTime;
        // if (!behaviorEnabled)
        //     return;

        // UpdateTargetDirection();
        // RotateTowardsTarget();
        // SetVelocity();
        JesusWanderTest();
    }

    private void JesusWanderTest()
    {
        // flips sprite
        if(_rigidbody.velocity.x < 0)
        {
            this.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            this.transform.localScale = new Vector3(-transform.localScale.y, transform.localScale.y, transform.localScale.z);

        }

        if(wanderLength <= 0)
        {
            // picks random direction
            float randx = UnityEngine.Random.Range(-1f, 1f);
            float randy = UnityEngine.Random.Range(-1f, 1f);
            // moves in said direction
            _rigidbody.velocity = Vector3.Normalize( new Vector3(randx,
            randy, 0)) * _speed;

            // changes direction every 2-6 seconds
            wanderLength = UnityEngine.Random.Range(2f, 5f);
        }
      
    }

    // Updates the target direction to a random direction
    private void UpdateTargetDirection()
    {
        HandleRandomDirectionChange();
        //HandlePlayerTargeting();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            collidedWithWall = true;
        }
    }

    // Chooses a random direction as part of the wandering phase
    private void HandleRandomDirectionChange()
    {
        _changeDirectionCooldown -= Time.deltaTime;

        if (collidedWithWall)
        {
            print("am collide with wall");
            Quaternion rotation = Quaternion.AngleAxis(180f, transform.forward);
            _targetDirection = rotation * _targetDirection;
            collidedWithWall = false;
        }
        else
        {
            if (_changeDirectionCooldown <= 0)
            {
                float angleChange = UnityEngine.Random.Range(-90f, 90f);
                Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
                _targetDirection = rotation * _targetDirection;
                _changeDirectionCooldown = UnityEngine.Random.Range(1,5);
            }
        }
        
        
    }

    /*
    private void HandlePlayerTargeting()
    {
        if (_playerAwarenessController.AwareOfPlayer)
        {
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
        }
    }
    */

    // Rotates towards the random target
    private void RotateTowardsTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        //Transform graphicsTransform = gameObject.GetComponentsInChildren<Transform>();

        _rigidbody.SetRotation(rotation);

        //GameObject graphicsObject = gameObject.transform.GetChild(0).gameObject;
        //graphicsObject.transform.rotation = new Quaternion(-1 * rotation.W, -1 * rotation.X, -1 * rotation.Y, -1 * rotation.Z);
    }
    
     // Gives the set velocity if aware of the player
    
    private void SetVelocity()
    {
        _rigidbody.velocity = transform.up * _speed;
    }

    public void enableBehavior()
    {
        //print("Wander enabled");
        _rigidbody.velocity = new Vector2(storeVector.x, storeVector.y);
        behaviorEnabled = true;
        _playerAwarenessController.wanderBehaviorEnabled = true;
    }

    public void disableBehavior()
    {
        //print("Wander disabled");
        storeVector = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y);
        behaviorEnabled = false;
        _playerAwarenessController.wanderBehaviorEnabled = false;
    }
}
