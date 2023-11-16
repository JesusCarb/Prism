using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class WeaponRotator : MonoBehaviour
{
    Transform player;
    
    Vector3 starting;
    // Start is called before the first frame update
    void Start()
    {
        player = this.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseLoc = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        // subtracted by player position to get directional vector
        Vector3 finalPos =  mouseLoc - player.transform.position;
        // finalPos = new Vector3(Mathf.Abs(finalPos.x),Mathf.Abs(finalPos.y),0);

        float angle = 0;
        if(finalPos.x >= 0)
        {
            angle = Math.Clamp(Mathf.Atan2(finalPos.y, finalPos.x) * Mathf.Rad2Deg,
            -90,90);
        }
        else
        {
            // false;pat
            angle = Math.Clamp(Mathf.Atan2(-1 * finalPos.y, -1 * finalPos.x) * Mathf.Rad2Deg,
            -90,90);
        }

       
        
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
