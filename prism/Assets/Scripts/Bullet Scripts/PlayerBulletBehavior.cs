using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using System;

public class PlayerBulletBehavior : MonoBehaviour
{
    public static event Action EnemyKilled = delegate {};

    public float speed = 25f;
    private float distx;
    private float disty;

    Vector2 direction;

    private float spawnTime;

    Rigidbody2D rb;
    GameObject player;


    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        spawnTime = Time.time;



        CalculatePlayerBulletTragectory();


    }

    // Update is called once per frame
    void Update()
    {
        DeSpawn();
        //print(direction);
        rb.velocity = direction * speed;
        //print("bullet velocity" + rb.velocity);
    }

    void FixedUpdate()
    {
        

    }
    // Probably better to move this to enemy to reduce lag
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Physics2D.IgnoreCollision(player.transform.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        // Prevent bullet-on-bullet collision
        if (collision.gameObject.tag.Equals("EnemyBullet"))
        {
            Destroy(collision.gameObject);

            Destroy(gameObject);

        }

        if (collision.gameObject.tag.Equals("Enemy"))
        {

            EnemyKilled();
            Destroy(collision.gameObject);
            Destroy(gameObject);

        }

        if (collision.gameObject.tag.Equals("Obstacle"))
        {
            Destroy(gameObject);

        }


    }

    private void CalculatePlayerBulletTragectory()
    {
        // Added "GameObject" before player bc it didn't compile
        // Vector3  mouseLoc = Input.mousePosition;

        // gets location of camera, changes it to world space
        Vector3 mouseLoc = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        // subtracted by player position to get directional vector
        Vector3 finalPos = mouseLoc - player.transform.position;

        float targx = finalPos.x;
        float targy = finalPos.y;
        float hypot = Mathf.Sqrt((targx * targx) + (targy * targy));

        distx = targx / hypot * speed;
        disty = targy / hypot * speed;
    
        direction = new Vector2(distx, disty);
    }
    // despawns the bullet after 3 seconds
    private void DeSpawn()
    {
        float currentTime = Time.time;
        
        if(currentTime - spawnTime > 3)
        {
            Destroy(gameObject);
        }
    }
}
