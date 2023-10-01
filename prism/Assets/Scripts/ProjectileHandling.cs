using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandling : MonoBehaviour
{

    private ManageBeatEvents beatManager;
    // Start is called before the first frame update
    void Start()
    {
        //beatManager = gameObject.GetComponent<ManageBeatEvents>;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //gameObject.
        }
    }

    void FireProjectile()
    {
        print("FIRE");
    }
}
