using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderScript : MonoBehaviour
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
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>();
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
        distanceToPlayer = player.transform.position - this.transform.position;
        directionToPlayer = distanceToPlayer.normalized;
        wanderLength -= Time.fixedDeltaTime;
        if(wanderLength <= 0)
        {
            JesusWanderTest();

        }
    }

    private void JesusWanderTest()
    {

        if(wanderLength <= 0)
        {

              if(distanceToPlayer.magnitude >= wanderLimit)
            {
                rb.velocity = Vector3.Normalize( new Vector3(directionToPlayer.x,
                directionToPlayer.y, 0)) * speed;
                wanderLength = UnityEngine.Random.Range(4f, 8f);

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
