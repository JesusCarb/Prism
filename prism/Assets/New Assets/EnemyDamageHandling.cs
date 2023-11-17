using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageHandling : MonoBehaviour
{
    public AudioClip enemyDamageAudio;
    public AudioClip enemyDeathAudio;

    private int hp = 3;

    const float damageTintTime = 0.3f;
    private float timeLeftTinted = 0f;
    private bool isTinted = false;
    private UnityEngine.Color originalColor;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        originalColor = player.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeftTinted > 0f)
        {
            timeLeftTinted -= Time.deltaTime;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = originalColor;
        }
    }

    public void takeDamage(int damageVal)
    {
        
        hp-= damageVal;

        if (hp <= 0)
        {
            killEnemy();
        }
        else
        {
            player.GetComponent<AudioSource>().PlayOneShot(enemyDamageAudio);

            timeLeftTinted = damageTintTime;
            isTinted = true;
            gameObject.GetComponent<SpriteRenderer>().color = UnityEngine.Color.red;
        }
    }

    public void killEnemy()
    {
        player.GetComponent<AudioSource>().PlayOneShot(enemyDeathAudio);
        Destroy(gameObject);
    }
}