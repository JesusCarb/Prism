using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBehavior : MonoBehaviour
{
    bool behaviorEnabled = false;
    bool canShoot = true;
    public GameObject enemyBullet;
    public Vector3 enemyPos;
    public Quaternion enemyRot;

    private Rigidbody2D _rigidbody;

    public AudioClip enemyShootClip;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        enemyPos = new Vector3(0, 0, 0);
        enemyRot = new Quaternion();
    }


    // Update is called once per frame
    void Update()
    {
        if (!behaviorEnabled)
        {
            canShoot = true;
            return;
        }

        if (canShoot)
        {
            Invoke("spawnEnemyBullet", 0);
            canShoot = false;
            audioSource.PlayOneShot(enemyShootClip);
        }
    }

    public void spawnEnemyBullet()
    {
        Instantiate(enemyBullet, position: enemyPos, rotation: enemyRot);
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
