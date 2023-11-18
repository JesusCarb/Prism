using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq.Expressions;

public class PlayerBulletBehavior : MonoBehaviour
{
    public static event Action EnemyKilled = delegate {};

    Vector2 direction;

    private float spawnTime;

    GameObject player;

    public int bulletType;

    public AudioClip enemyDamageAudio;

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.time;
        player = GameObject.FindWithTag("Player");


    }

    // Update is called once per frame
    void Update()
    {
        DeSpawn();
        //print(direction);
        //print("bullet velocity" + rb.velocity);
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
            collision.gameObject.GetComponent<EnemyDamageHandling>().takeDamage(1);

            
            //EnemyKilled();
            //Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag.Equals("Obstacle"))
        {
            Destroy(gameObject);

        }
    }

  
    // despawns the bullet after 3 seconds
    private void DeSpawn()
    {
        // sets despawn time depending on which bullet it be
        float currentTime = Time.time;
        float despawnTime = 1f;
        switch(bulletType)
        {
            // pistol

            case 1:
            despawnTime = 1.35f;
            break;

            // rifle

            case 2:

            despawnTime = 1.75f;
            break;

            // shotty

            case 3:
            despawnTime = 0.75f;
            break;

        }
        if(currentTime - spawnTime > despawnTime)
        {
            Destroy(gameObject);
        }
    }
}
