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
    
    // Awake is called when the script/entity loads in and becomes active
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
    }

    // FixedUpdate works better with the physics system
    private void FixedUpdate()
    {
        //behaviorEnabled = chaseScript.behaviorEnabled;
        if (!behaviorEnabled)
            return;

        UpdateTargetDirection();
        RotateTowardsTarget();
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
        if (_targetDirection == Vector2.zero)
        {
            _rigidbody.velocity = Vector2.zero;
        }
        else
        {
            _rigidbody.velocity = transform.up * _speed;
        }
    }
}
