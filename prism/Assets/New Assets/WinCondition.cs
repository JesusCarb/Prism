using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WinCondition : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerC;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerC = player.GetComponent<PlayerController>();
    }

    IEnumerator OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            playerC.playerInfo.weapon1 = playerC.weapon1;
            playerC.playerInfo.weapon2 = playerC.weapon2;
            playerC.playerInfo.weapon3 = playerC.weapon3;
            playerC.playerInfo.permPowerBuff = playerC.permPowerBuff;
            playerC.playerInfo.permSpeedBuff = playerC.permSpeedBuff;
            playerC.playerInfo.maxHealth = playerC.maxHealth;
            playerC.playerInfo.extraBulletsFired = playerC.extraBulletsFired;
            playerC.playerInfo.shotSpeedBuff = playerC.shotSpeedBuff;
            playerC.playerInfo.rangeBuff = playerC.rangeBuff;
            playerC.playerInfo.hitLeewayBuff = playerC.hitLeewayBuff;
            playerC.playerInfo.ouchImmunity = playerC.ouchImmunity;
            playerC.playerInfo.floorNum = playerC.floorNum + 1;
            playerC.playerInfo.savedHP = playerC.hp;
            playerC.playerInfo.lastWeaponEquipped = playerC.lastWeaponEquipped;
            
            yield return new WaitForSecondsRealtime(.05f);

            print("youwin");

            playerC.moveSpeed = 0;

            //Time.timeScale = 0;
            //yield return new WaitForSecondsRealtime(2);

            //Time.timeScale = 1;

            // Fade to black (and decrease music volume)
            GameObject.FindGameObjectWithTag("Fade").GetComponent<FadeToBlackUI>().startFade = true;
            while(!GameObject.FindGameObjectWithTag("Fade").GetComponent<FadeToBlackUI>().finished)
            {
                yield return new WaitForSeconds(1);
            }


            SceneManager.LoadScene("TransitionScene");

            /*
            if (playerC.playerInfo.floorNum == 11)
            {
                SceneManager.LoadScene("OVERWORLD");
            }
            else
            {
                SceneManager.LoadScene("TransitionScene");
            }
            */
            // Destroy(gameObject);
        }
    }
}
