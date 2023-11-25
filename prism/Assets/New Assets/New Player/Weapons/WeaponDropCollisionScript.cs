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
            switch (currentWeapon)
            {
                case 1:
                    if (player.weapon1 == "Pistol" || player.weapon2 == "Pistol" || player.weapon3 == "Pistol")
                    {
                        return;
                    }
                    break;

                case 2:
                    if (player.weapon1 == "Rifle" || player.weapon2 == "Rifle" || player.weapon3 == "Rifle")
                    {
                        return;
                    }
                    break;
                
                case 3:
                    if (player.weapon1 == "Shotty" || player.weapon2 == "Shotty" || player.weapon3 == "Shotty")
                    {
                        return;
                    }
                    break;
            }
            
            player.SetWeapon(currentWeapon);
            Destroy(gameObject);
        }
    }
}
