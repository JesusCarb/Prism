using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponDropCollisionScript : MonoBehaviour
{
    [SerializeField]
    private int currentWeapon;

    private enum Weapon
    {
        None,
        Pistol,
        Shotty,
        Rifle
    }

    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindAnyObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
        {
            player.SetWeapon(currentWeapon);
            Destroy(gameObject);
        }
    }
}
