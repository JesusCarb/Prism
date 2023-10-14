using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class ProjectileHandling : MonoBehaviour
{
    ManageBeatEvents beatManager;

    PlayerController playerController;
    public GameObject playerBullet;

    private GameObject player;

    private bool firstFireCurrentBeat = true;
    private float timeUntilNextFire = 0f;
    private float delayFromFire = .25f;

    // Start is called before the first frame update
    void Start()
    {
        beatManager = gameObject.GetComponent<ManageBeatEvents>();
        playerController = gameObject.GetComponent<PlayerController>();
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
        Vector3 pos = playerController.transform.position;
        Quaternion rot = playerController.transform.rotation;
        // currently spawning on player position
        print("FIRE");
        Instantiate(playerBullet, position: pos, rotation: rot);
        // Debug.Log("FIRE");
    }
}
