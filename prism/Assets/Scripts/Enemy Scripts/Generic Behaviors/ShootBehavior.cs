using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBehavior : MonoBehaviour
{
    bool behaviorEnabled = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (!behaviorEnabled)
            return;
    }

    public void enableBehavior()
    {
        print("Shoot enabled");
        behaviorEnabled = true;
    }

    public void disableBehavior()
    {
        print("Shoot disabled");
        behaviorEnabled = false;
    }
}
