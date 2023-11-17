using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderChase : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]

    private float detectionDistance;

    
    [SerializeField]
    private float wanderLimit;
    PlayerController player;

    Rigidbody2D rb;

    Vector3 distanceToPlayer;
    Vector3 directionToPlayer;

    float wanderLength;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        wanderLength = 0f;
    }

    // Update is called once per frame
    void Update()
    {
          // flips sprite
        if(rb.velocity.x < 0)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            this.transform.localScale = new Vector3(1, 1, 1);

        }
    }

    void FixedUpdate()
    {

        wanderLength -= Time.fixedDeltaTime;

        distanceToPlayer = player.transform.position - this.transform.position;
        directionToPlayer = distanceToPlayer.normalized;

        // chase if close enough otherwise wander
        if(Math.Abs(distanceToPlayer.x) < detectionDistance)
        {
            Chase();
        }
        else
        {
            if(wanderLength <= 0)
            {
                Wander();
            }
        }
    }
    void Chase()
    {

        rb.velocity = Vector3.Normalize( new Vector3(directionToPlayer.x,
        directionToPlayer.y, 0)) * speed;

    }

    void Wander()
    {

        if(wanderLength <= 0)
        {
 

            // if too far will move towards the player 
            if(distanceToPlayer.magnitude >= wanderLimit)
            {
                rb.velocity = Vector3.Normalize( new Vector3(directionToPlayer.x,
                directionToPlayer.y, 0)) * speed;
                wanderLength = UnityEngine.Random.Range(2f, 6f);

            }
            else
            {
                // changes direction every time interval you want seconds
            // picks random direction
                float randx = UnityEngine.Random.Range(-1f, 1f);
                float randy = UnityEngine.Random.Range(-1f, 1f);
                // moves in said direction
                rb.velocity = Vector3.Normalize( new Vector3(randx,
                randy, 0)) * speed;
                wanderLength = UnityEngine.Random.Range(1f, 4f);
                
            }
        }
    }
}
