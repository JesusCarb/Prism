using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBehavior : MonoBehaviour
{
    public float speed = 5f;
    private float distx;
    private float disty;

    // Start is called before the first frame update
    void Start()
    {
        float targx = GameObject.FindGameObjectWithTag("Cursor").transform.position.x;
        float targy = GameObject.FindGameObjectWithTag("Cursor").transform.position.y;
        float hypot = Mathf.Sqrt((targx * targx) + (targy * targy));

        distx = targx / hypot * speed;
        disty = targy / hypot * speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (new Vector3(distx, disty, 0) * Time.deltaTime * speed);
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
}
