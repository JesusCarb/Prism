using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAwarenessController : MonoBehaviour
{
    public bool behaviorEnabled = true;
    public ChaseMovement chaseScript;

    public bool AwareOfPlayer {get; private set;}
    public Vector2 DirectionToPlayer {get; private set;}

    [SerializeField]
    private float _playerAwarenessDistance;
    private Transform _player;
    
    // Awake is called when the script/entity loads in and becomes active
    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!behaviorEnabled)
            return;

        Vector2 enemyToPlayerVector = _player.position - transform.position;
        DirectionToPlayer = enemyToPlayerVector.normalized;

        if (enemyToPlayerVector.magnitude <= _playerAwarenessDistance)
        {
            AwareOfPlayer = true;
        }
        else
        {
            AwareOfPlayer = false;
        }
    }
}
