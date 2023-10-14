using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBehavior : MonoBehaviour
{
    bool behaviorEnabled = false;

    private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!behaviorEnabled)
            return;

    }

    public void enableBehavior()
    {
        //print("Shoot enabled");
        _rigidbody.velocity = Vector2.zero;   //This doesn't work. I'm trying to freeze it while it shoots.
        //_rigidbody.velocity -= _rigidbody.velocity;
        behaviorEnabled = true;
    }

    public void disableBehavior()
    {
        //print("Shoot disabled");
        behaviorEnabled = false;
    }
}
