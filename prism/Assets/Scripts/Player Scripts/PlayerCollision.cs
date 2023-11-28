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


        //checks for collision with ouch or ouchobstacle reduces health
        if (collision.gameObject.tag.Equals("OuchObstacle"))
        {
            // collision.gameObject.takeDamage()
            if (gameObject.GetComponent<PlayerController>().ouchImmunity) {return;};
            gameObject.GetComponent<PlayerController>().hp -= 1;
            gameObject.GetComponent<PlayerController>().audioSource.PlayOneShot(playerDamageAudio);
            // If ^ doesn't work, just GameObject.findAnyObjectOfType<>
        }

    }

    

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Ouch"))
        {
            if (gameObject.GetComponent<PlayerController>().ouchImmunity) {return;};
            gameObject.GetComponent<PlayerController>().hp -= 1;
            gameObject.GetComponent<PlayerController>().audioSource.PlayOneShot(playerDamageAudio);
        }

        
        if (collision.gameObject.name == "FullHealItem")
        {
            if (gameObject.GetComponent<PlayerController>().hp == gameObject.GetComponent<PlayerController>().maxHealth) {return;};
            Destroy(collision.gameObject);
            gameObject.GetComponent<PlayerController>().hp = 3;
        }

        if (collision.gameObject.name == "PartialHealItem")
        {
            if (gameObject.GetComponent<PlayerController>().hp == gameObject.GetComponent<PlayerController>().maxHealth) {return;};
            Destroy(collision.gameObject);
            gameObject.GetComponent<PlayerController>().hp += 1;
        }

        if (collision.gameObject.name == "PowerBuffItem")
        {
            Destroy(collision.gameObject);
            gameObject.GetComponent<PlayerController>().damageMultiplier *= 1.5f;
            StartCoroutine(PowerBuffTimer());
        }

        if (collision.gameObject.name == "SpeedBuffItem")
        {
            Destroy(collision.gameObject);
            gameObject.GetComponent<PlayerController>().moveSpeed *= 1.4f;
            StartCoroutine(SpeedBuffTimer());
        }
        
    }

    IEnumerator SpeedBuffTimer()
    {
        yield return new WaitForSeconds(20f);
        gameObject.GetComponent<PlayerController>().moveSpeed /= 1.4f;
    }

    IEnumerator PowerBuffTimer()
    {
        yield return new WaitForSeconds(20f);
        gameObject.GetComponent<PlayerController>().damageMultiplier /= 2;
    }
}


