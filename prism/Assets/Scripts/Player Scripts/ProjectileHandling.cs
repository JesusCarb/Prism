using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandling : MonoBehaviour
{
    ManageBeatEvents beatManager;

    PlayerController playerController;
    public GameObject playerBullet;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        beatManager = gameObject.GetComponent<ManageBeatEvents>();
        playerController = gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if(playerController.OnBeat())
            {
                FireProjectile();
                Debug.Log("Beat Hit");

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
        // currently spawning on player position
        print("FIRE");
        Instantiate(playerBullet, playerController.transform);
        // Debug.Log("FIRE");
    }
}
