using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public AudioClip playerDamageAudio;

    void OnCollisionEnter2D(Collision2D collision)
    {

        // checks for collision with bullet reduces health destroys bullet
        if (collision.gameObject.tag.Equals("EnemyBullet"))
        {   
            print("COLLISION");
            Destroy(collision.gameObject);
            // reduce hp by 1 when hit by bullet
            gameObject.GetComponent<PlayerController>().hp -= 1;

            gameObject.GetComponent<PlayerController>().audioSource.PlayOneShot(playerDamageAudio);
        }

        // checks for collision with enemy reduces health

        if (collision.gameObject.tag.Equals("Enemy"))
        {
            // collision.gameObject.takeDamage()
            gameObject.GetComponent<PlayerController>().hp -= 1;
            gameObject.GetComponent<PlayerController>().audioSource.PlayOneShot(playerDamageAudio);
            // If ^ doesn't work, just GameObject.findAnyObjectOfType<>
        }

    }
}
