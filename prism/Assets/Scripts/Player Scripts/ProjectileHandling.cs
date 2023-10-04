using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandling : MonoBehaviour
{
    ManageBeatEvents beatManager;

    public GameObject playerBullet;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        beatManager = gameObject.GetComponent<ManageBeatEvents>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            FireProjectile();
            //if (!beatManager.onQuarter.contains(FireProjectile()))
            //{
            //    beatManager.onQuarter += FireProjectile();
            //}
            
        }
    }

    void FireProjectile()
    {
        print("FIRE");
        Instantiate(playerBullet, player.transform);
        // Debug.Log("FIRE");
    }
}
