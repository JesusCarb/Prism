using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            Destroy(collision.gameObject);
        }

        Destroy(gameObject);
    }
}
