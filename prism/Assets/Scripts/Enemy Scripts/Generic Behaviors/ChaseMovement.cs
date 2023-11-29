using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseMovement : MonoBehaviour
{
    private bool behaviorEnabled = true;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _rotationSpeed;

    private Rigidbody2D _rigidbody;
    private PlayerAwarenessController _playerAwarenessController;
    private Vector2 _targetDirection;

    private Vector2 storeVector;
    
    // Awake is called when the script/entity loads in and becomes active
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
    }

    // FixedUpdate works better with the physics system
    private void FixedUpdate()
    {
        if (!behaviorEnabled)
            return;

        UpdateTargetDirection();
        // RotateTowardsTarget();
        SetVelocity();
    }

    // Updates the target direction if aware of the player
    private void UpdateTargetDirection()
    {
        if (_playerAwarenessController.AwareOfPlayer)
        {
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
        }
        else
        {
            _targetDirection = Vector2.zero;
        }
    }

    // Rotates towards the player if aware of the player
    private void RotateTowardsTarget()
    {
        if (_targetDirection == Vector2.zero)
        {
            return;
        }

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
                // flip
        if(_playerAwarenessController.DirectionToPlayer.x < 0)
        {
            this.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            this.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        }


        if (_targetDirection == Vector2.zero)
        {
            _rigidbody.velocity = Vector2.zero;
        }
        else
        {
            // _rigidbody.velocity = transform.up * _speed;            
            _rigidbody.velocity = Vector3.Normalize( new Vector3(_playerAwarenessController.DirectionToPlayer.x,
            _playerAwarenessController.DirectionToPlayer.y, 0)) * _speed;
        }
    }

    public void enableBehavior()
    {
        //print("Chase enabled");
        _rigidbody.velocity = new Vector2(storeVector.x, storeVector.y);
        behaviorEnabled = true;
        _playerAwarenessController.chaseBehaviorEnabled = true;
    }

    public void disableBehavior()
    {
        //print("Chase disabled");
        storeVector = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y);
        behaviorEnabled = false;
        _playerAwarenessController.chaseBehaviorEnabled = false;
    }
}
