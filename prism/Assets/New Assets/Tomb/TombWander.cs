using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombWander : MonoBehaviour
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

    int currBeatCount = 0;
    private PlayerController pc;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>();
        pc = player.GetComponent<PlayerController>();

        wanderLength = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        if(pc.beatChange == true)
        {
            currBeatCount += 1;
        }
        // flips sprite
        if(rb.velocity.x < 0)
        {
            this.transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x) * -1, this.transform.localScale.y, this.transform.localScale.z);
        }
        else
        {
            this.transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);

        }

        distanceToPlayer = player.transform.position - this.transform.position;
        directionToPlayer = distanceToPlayer.normalized;
        if(wanderLength <= 1)
        {
            JesusWanderTest();
        }

        
        if(Math.Abs(distanceToPlayer.magnitude) < detectionDistance)
        {
            if(currBeatCount >= 2)
            {
                Shoot(); 
                currBeatCount = 0;  
            }
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
        print(angle);
        print(angle+90);
        Quaternion rot = Quaternion.Euler(new Vector3(0,0, angle - 90));
        Instantiate(EnemyBullet, pos, rot);
    }
}
