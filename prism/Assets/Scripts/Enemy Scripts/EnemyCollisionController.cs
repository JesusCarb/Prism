using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCollisionController : MonoBehaviour
{
    void  OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag.Equals("playerBullet"))
        {
            print("enemycollision");
            Destroy(col.gameObject);
            Destroy(gameObject);
            
        }
    }
}
