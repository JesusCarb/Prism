using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooterController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    public PlayerController playerController;   // could move to Start(), change to private __ __ = player.GetComponent<PlayerController>();

    private Vector3 playerPos;
    private bool onBeat = true;
    private int curBeatCount = 0;
    //private int maxBeatCount = 7;   // fire every 8 beats

    private bool shootBehaviorOn = false;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = new Vector3(player.transform.position.x, player.transform.position.y, 0);
    }


    // Update is called once per frame
    void Update()
    {
        onBeat = playerController.OnBeat();

        if (onBeat)
        {
            if (curBeatCount == 0)
                shootBehaviorOn = true;
            else if (curBeatCount == 2)
                shootBehaviorOn = false; // shoot behavior lasts for two beats

            if (curBeatCount == 7)
                curBeatCount = 0;
            else
                curBeatCount++;
        }

        if (shootBehaviorOn)
        {

        }
    }
}
