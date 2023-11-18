using System;
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

    public GameObject EnemyBullet;

    public float shootDelay;

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

        distanceToPlayer = player.transform.position - this.transform.position;
        directionToPlayer = distanceToPlayer.normalized;
        if(wanderLength <= 1)
        {
            JesusWanderTest();

            if(Math.Abs(distanceToPlayer.magnitude) < detectionDistance)
                Shoot();

        }

        wanderLength -= Time.deltaTime;
    }


    private void JesusWanderTest()
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

    // private IEnumerator WaitAndShoot()
    // {
    //     rb.velocity = Vector3.zero;

    //     yield return new WaitForSeconds(1f);
        
    //     // JesusWanderTest();
    //     // Instantiate(EnemyBullet, this.transform.position, this.transform.rotation);

    // }
     private void Shoot()
    {
        Vector3 pos = this.transform.position;
        Vector3 mouseLoc = player.transform.position;;
        Vector3 final = mouseLoc - pos;

        float angle = Mathf.Atan2(final.y, final.x) * Mathf.Rad2Deg;

        Quaternion rot = Quaternion.Euler(new Vector3(0,0, angle));

        Instantiate(EnemyBullet, pos, rot);
    }
}
