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



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        CalculateEnemyBulletTragectory();
        
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
    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     // Prevent bullet-on-bullet collision
    //     if (collision.gameObject.tag.Equals("Bullet"))
    //         return;

    //     if (collision.gameObject.tag.Equals("Player"))
    //     {
    //         // collision.gameObject.takeDamage()
    //         // If ^ doesn't work, just GameObject.findAnyObjectOfType<>
    //         Destroy(collision.gameObject);
    //     }

    //     Destroy(gameObject);
    // }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    
    //}
}
