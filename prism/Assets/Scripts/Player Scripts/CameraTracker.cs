using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        GameObject p = GameObject.FindWithTag("Player");
        player = p.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new UnityEngine.Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }
}
