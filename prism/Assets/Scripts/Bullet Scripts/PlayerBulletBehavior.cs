using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBehavior : MonoBehaviour
{
    public float speed = 10f;
    private float distx;
    private float disty;

    Vector3 direction;

    private float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 mouseLoc = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        spawnTime = Time.time;
        print(mouseLoc);
        float targx = mouseLoc.x;
        float targy = mouseLoc.y;
        float hypot = Mathf.Sqrt((targx * targx) + (targy * targy));

        distx = targx / hypot * speed;
        disty = targy / hypot * speed;

        direction = new Vector3(distx, disty, 0);
    }

    // Update is called once per frame
    void Update()
    {
        DeSpawn();
        transform.position += direction * Time.deltaTime * speed;
    }

    // Probably better to move this to enemy to reduce lag
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Prevent bullet-on-bullet collision
        if (collision.gameObject.tag.Equals("Bullet"))
            return;

        if (collision.gameObject.tag.Equals("Enemy"))
        {
            Destroy(collision.gameObject);
        }

        Destroy(gameObject);
    }

    private void DeSpawn()
    {
        float currentTime = Time.time;
        
        if(currentTime - spawnTime > 3)
        {
            Destroy(gameObject);
        }
    }
}
