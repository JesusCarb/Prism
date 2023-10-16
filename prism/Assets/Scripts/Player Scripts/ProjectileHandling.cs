using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileHandling : MonoBehaviour
{
    ManageBeatEvents beatManager;

    PlayerController playerController;
    public GameObject playerBullet;

    public GameObject enemyBullet;

    private GameObject player;



    private bool firstFireCurrentBeat = true;
    private float timeUntilNextFire = 0f;
    private float delayFromFire = .25f;

    // Start is called before the first frame update
    void Start()
    {
        beatManager = gameObject.GetComponent<ManageBeatEvents>();
        playerController = gameObject.GetComponent<PlayerController>();
        player = this.gameObject;
        print(player);
        SpawnEnemyBullet();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeUntilNextFire > 0f)
            timeUntilNextFire -= Time.deltaTime;
        if (timeUntilNextFire <= 0f)
            firstFireCurrentBeat = true;


        if (Input.GetButtonDown("Fire1"))
        {
            print("fire1");
            if(playerController.OnBeat() && firstFireCurrentBeat)
            {
                FireProjectile();
                Debug.Log("Beat Hit");
                firstFireCurrentBeat = false;
                timeUntilNextFire = delayFromFire;
            }
            else
            {
                Debug.Log("Beat Miss");

            }
            //if (!beatManager.onQuarter.contains(FireProjectile()))
            //{
            //    beatManager.onQuarter += FireProjectile();
            //}
            
        }
    }

    void FireProjectile()
    {
        print("bullet spawn");
        // gets position of player to spawn bullet
        Vector3 pos = playerController.transform.position + new Vector3(0,0,0);
        Quaternion rot = playerController.transform.rotation;
        // currently spawning on player position
        Instantiate(playerBullet, position: pos, rotation: rot);
        // Debug.Log("FIRE");
    }

    // for debugging, will spawn enemy bullets
    private void SpawnEnemyBullet()
    {
        Vector3 pos = playerController.transform.position;
        Quaternion rot = playerController.transform.rotation;
        
        Vector3 offset = new Vector3(0,10,0);
        pos += offset;
        print(pos);
        
        Instantiate(enemyBullet, position: pos, rotation: rot);
    }
}
