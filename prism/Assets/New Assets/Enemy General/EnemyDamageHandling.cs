using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDamageHandling : MonoBehaviour
{
    public AudioClip enemyDamageAudio;
    public AudioClip enemyDeathAudio;


    public GameObject FullHealItem;
    public GameObject PartialHealItem;
    public GameObject PowerBuffItem;
    public GameObject SpeedBuffItem;


    public Room roomSpawnedIn;

    public int enemyID;

    public float hp = 3;

    const float damageTintTime = 0.3f;
    private float timeLeftTinted = 0f;
    //private bool isTinted = false;
    private UnityEngine.Color originalColor;

    private GameObject player;

    public GameObject corpse;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        originalColor = player.GetComponent<SpriteRenderer>().color;
        hp += .5f * player.GetComponent<PlayerController>().floorNum - 1;
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

    public void takeDamage(float damageVal)
    {
        
        hp-= damageVal;

        if (hp <= 0)
        {
            Instantiate(corpse, position: this.transform.position, rotation: this.transform.rotation);

            //enemy drops
            if (Random.Range(0, 10) == 0)
            {
                int itemnum = Random.Range(0, 4);
                switch (itemnum)
                {
                    case 0:
                        GameObject FHIclone = Instantiate(FullHealItem, position: this.transform.position, rotation: this.transform.rotation);
                        FHIclone.name = "FullHealItem";
                        break;

                    case 1:
                        GameObject PHIclone = Instantiate(PartialHealItem, position: this.transform.position, rotation: this.transform.rotation);
                        PHIclone.name = "PartialHealItem";
                        break;
                    
                    case 2:
                        GameObject PBIclone = Instantiate(PowerBuffItem, position: this.transform.position, rotation: this.transform.rotation);
                        PBIclone.name = "PowerBuffItem";
                        break;
                    
                    case 3:
                        GameObject SBIclone = Instantiate(SpeedBuffItem, position: this.transform.position, rotation: this.transform.rotation);
                        SBIclone.name = "SpeedBuffItem";
                        break;
                }
            }

            killEnemy();
        }
        else
        {
            player.GetComponent<AudioSource>().PlayOneShot(enemyDamageAudio);

            timeLeftTinted = damageTintTime;
            //isTinted = true;
            gameObject.GetComponent<SpriteRenderer>().color = UnityEngine.Color.red;
        }
    }

    public void killEnemy()
    {
        player.GetComponent<AudioSource>().PlayOneShot(enemyDeathAudio);
        //roomSpawnedIn.RemoveEnemy(enemyID);
        Destroy(gameObject);
    }
}
