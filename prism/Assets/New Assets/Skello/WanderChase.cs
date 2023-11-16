using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WanderChase : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]

    private float detectionDistance;

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
        
    }

    // Update is called once per frame
    void Update()
    {
  
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
            Wander();
        }
    }
    void Chase()
    {
        // flip
        if(player.transform.position.x - this.transform.position.x < 0)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            this.transform.localScale = new Vector3(1, 1, 1);

        }

        rb.velocity = Vector3.Normalize( new Vector3(directionToPlayer.x,
        directionToPlayer.y, 0)) * speed;

    }

    void Wander()
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

        if(wanderLength <= 0)
        {
            // picks random direction
            float randx = UnityEngine.Random.Range(-1f, 1f);
            float randy = UnityEngine.Random.Range(-1f, 1f);
            // moves in said direction
            rb.velocity = Vector3.Normalize( new Vector3(randx,
            randy, 0)) * speed;

            // changes direction every 2-6 seconds
            wanderLength = UnityEngine.Random.Range(2f, 5f);
        }
    }
}
