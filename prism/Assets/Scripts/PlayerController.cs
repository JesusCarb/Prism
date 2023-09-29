using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
// using UnityEditor.Experimental.GraphView;
// using System.Numerics;

using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;


    Vector2 playerInput;

    Vector3 velocity;

    float velocityXSmoothing;

    float velocityYSmoothing;

    float accelerationTime = .1f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
 
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        float targetVelocityX = playerInput.x * moveSpeed;
        float targetVelocityY = playerInput.y * moveSpeed;

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTime);
        velocity.y = Mathf.SmoothDamp(velocity.y, targetVelocityY, ref velocityYSmoothing, accelerationTime);

        Move(velocity);
    }

    public void Move(Vector3 velocity)
    {
        transform.Translate(velocity);
    }
}
