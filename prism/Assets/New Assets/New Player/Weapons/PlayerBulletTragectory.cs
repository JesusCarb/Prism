using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBulletTragectory : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float spread;
    
    private float distx;
    private float disty;

    Vector2 direction;

    Rigidbody2D rb;
    GameObject player;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");

        CalculatePlayerBulletTragectory();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = direction * speed;

        // this.transform.Rotate(0f,0f, Random.Range(-spread,spread));
    }
    private void CalculatePlayerBulletTragectory()
    {
        
        // Added "GameObject" before player bc it didn't compile
        // Vector3  mouseLoc = Input.mousePosition;
        float spr = Random.Range(-spread, spread);
        //print(spr);

        // gets location of camera, changes it to world space
        Vector3 mouseLoc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // mouseLoc = new Vector3(mouseLoc.x + randomAngle, mouseLoc.y + randomAngle2, 0);

        // subtracted by player position to get directional vector
        Vector3 finalPos = (mouseLoc - player.transform.position);

        // the below snippet of code is a godsend
        float posAngle = Mathf.Atan2(finalPos.y, finalPos.x);
        posAngle += spr;
        direction = new Vector2(Mathf.Cos(posAngle), Mathf.Sin(posAngle));

        // float targx = finalPos.x;
        // float targy = finalPos.y;
        // float hypot = Mathf.Sqrt((targx * targx) + (targy * targy));

        // distx = targx / hypot;
        // disty = targy / hypot;
    
        // direction = new Vector2(distx, disty);
        //print(direction);
    }
}
