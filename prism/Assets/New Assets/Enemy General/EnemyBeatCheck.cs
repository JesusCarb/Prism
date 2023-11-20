using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyBeatCheck : MonoBehaviour
{
    public PlayerController playerController;
    public bool enemyFireBeat = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyFireBeat = playerController.GetEnemyFireBeat();
    }
}
