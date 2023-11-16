using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderScript : MonoBehaviour
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
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        wanderLength -= Time.fixedDeltaTime;
        JesusWanderTest();
    }

    private void JesusWanderTest()
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
