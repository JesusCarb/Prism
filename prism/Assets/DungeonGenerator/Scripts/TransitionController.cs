using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour
{
    public PlayerInfo playerInfo;
    
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.UI.Text text = GameObject.Find("FloorText").GetComponent<Text>();

        switch (playerInfo.floorNum)
        {
            case 1:
                text.text = "Floor 1";
                break;
            
            case 2:
                text.text = "Floor 2";
                break;
            
            case 3:
                text.text = "Floor 3";
                break;
            
            case 4:
                text.text = "Floor 4";
                break;
            
            case 5:
                text.text = "Floor 5";
                break;
            
            case 6:
                text.text = "Floor 6";
                break;
            
            case 7:
                text.text = "Floor 7";
                break;
            
            case 8:
                text.text = "Floor 8";
                break;
            
            case 9:
                text.text = "Floor 9";
                break;
            
            case 10:
                text.text = "Floor 10";
                break;
            
            case 11:
                text.text = "You Win!";
                break;
        }
        
        StartCoroutine(SendToDungeon());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SendToDungeon()
    {
        yield return new WaitForSeconds(2.75f);
        if (playerInfo.floorNum == 11)
        {
            playerInfo.weapon1 = "";
            playerInfo.weapon2 = "";
            playerInfo.weapon3 = "";
            playerInfo.permPowerBuff = 1;
            playerInfo.permSpeedBuff = 1;
            playerInfo.maxHealth = 3;
            playerInfo.extraBulletsFired = 0;
            playerInfo.shotSpeedBuff = 1;
            playerInfo.rangeBuff = 1;
            playerInfo.hitLeewayBuff = 1;
            playerInfo.ouchImmunity = false;
            playerInfo.floorNum = 1;
            playerInfo.savedHP = 3;
            playerInfo.lastWeaponEquipped = "Pistol";
            
            SceneManager.LoadScene("OVERWORLD");
        }
        else
        {
            SceneManager.LoadScene("DungeonGen");
        }
    }
}
