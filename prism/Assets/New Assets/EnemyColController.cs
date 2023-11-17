using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyColController : MonoBehaviour
{
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    void Update()
    {
        // print(rb.velocity);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "AICollider")
        {
            // flip velocity
            rb.velocity = new Vector2(-rb.velocity.x, -rb.velocity.y);


            // HITWAALLLL!
            print("HITWAALLL");
        }

        //if(collision.gameObject.tag == "PlayerBullet")
        //{
        //    Destroy(collision.gameObject);
        //
        //    EnemyDamageHandling damageScript = gameObject.GetComponent<EnemyDamageHandling>();
        //    damageScript.takeDamage(1);
        //}
    }

}
