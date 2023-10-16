using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
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

    private Vector2 storeVector;
    
    // Awake is called when the script/entity loads in and becomes active
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
        _targetDirection = transform.up;
    }

    // FixedUpdate works better with the physics system
    private void FixedUpdate()
    {
        if (!behaviorEnabled)
            return;

        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
    }

    // Updates the target direction to a random direction
    private void UpdateTargetDirection()
    {
        HandleRandomDirectionChange();
        //HandlePlayerTargeting();
    }

    // Chooses a random direction as part of the wandering phase
    private void HandleRandomDirectionChange()
    {
        _changeDirectionCooldown -= Time.deltaTime;

        if (_changeDirectionCooldown <= 0)
        {
            float angleChange = Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
            _targetDirection = rotation * _targetDirection;

            _changeDirectionCooldown = Random.Range(1,5);
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
