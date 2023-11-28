using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehavior : MonoBehaviour
{
    public float speed = 5f;
    private float distx;
    private float disty;

    Rigidbody2D rb;
    Vector2 direction;
    GameObject player;
    private float spawnTime;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        CalculateEnemyBulletTragectory();
        
        spawnTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = direction * speed;
    }

    private void CalculateEnemyBulletTragectory()
    {

        Vector3 playerLoc = player.transform.position;
        Vector3 finalPos = playerLoc - this.transform.position;
        
        float targx = finalPos.x;
        float targy = finalPos.y;
        float hypot = Mathf.Sqrt((targx * targx) + (targy * targy));

        distx = targx / hypot * speed;
        disty = targy / hypot * speed;

        direction = new Vector2(distx, disty);
    }

    // Probably better to move this to enemy to reduce lag
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
            
                // Prevent bullet-on-bullet collision
        if (collision.gameObject.tag.Equals("Bullet"))
            return;

        if (collision.gameObject.tag.Equals("Player"))
        {
            // collision.gameObject.takeDamage()
            // If ^ doesn't work, just GameObject.findAnyObjectOfType<>
        }

        if (collision.gameObject.tag.Equals("Obstacle") || collision.gameObject.tag.Equals("OuchObstacle"))
        {

            Destroy(gameObject);
        }

        if (collision.gameObject.tag.Equals("Ouch"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }
    // despawns the bullet after 3 seconds

    private void DeSpawn()
    {
        float currentTime = Time.time;
        
        if(currentTime - spawnTime > 1.5)
        {
            Destroy(gameObject);
        }
    }


}
