using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RuneController : MonoBehaviour
{

    [SerializeField]
    public int runeNum;
    public bool active = false;

    public string runeName;
    public List<string> changeList;

    public GameObject runePanel;


    // Start is called before the first frame update
    void Start()
    {
        new WaitForSeconds(0.5f);
        PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        runeNum = Random.Range(1, 14);

        while(true)
        {
            if (runeNum == 1 && player.maxHealth > 4) {continue;};
            if (runeNum == 4 && player.extraBulletsFired == 5) {continue;};
            if (runeNum == 8 && player.maxHealth < 2) {continue;};
            if (runeNum == 9 && player.maxHealth < 3) {continue;};
            if (runeNum == 11 && player.maxHealth > 5) {continue;};
            if (runeNum == 13 && (player.weapon1 == "Shotty" || player.weapon2 == "Shotty" || player.weapon3 == "Shotty")) {continue;};
            if (runeNum == 14 && (player.weapon1 == "Rifle" || player.weapon2 == "Rifle" || player.weapon3 == "Rifle")) {continue;};

            break;
        }

        switch (runeNum)
        {
            case 1:
                runeName = "Rune of Gloas";
                changeList.Add("- Increases max HP by 2");
                changeList.Add("- Decreases player speed by 40%");
                break;
            
            case 2:
                runeName = "Rune of Spect";
                changeList.Add("- Increases damage by 50%");
                changeList.Add("- Decreases shot speed by 25%");
                break;
            
            case 3:
                runeName = "Rune of Rush";
                changeList.Add("- Increases player speed by 75%");
                break;
            
            case 4:
                runeName = "Rune of Break";
                changeList.Add("- Increases bullet fire amount by 1");
                changeList.Add("- Decreases damage by 25%");
                break;
            
            case 5:
                runeName = "Rune of Steel";
                changeList.Add("- Grants immunity to spikes");
                changeList.Add("- Decreases player speed by 30%");
                break;
            
            case 6:
                runeName = "Rune of Mass";
                changeList.Add("- Increases damage by 50%");
                changeList.Add("- Decreases player speed by 25%");
                break;
            
            case 7:
                runeName = "Rune of Sight";
                changeList.Add("- Increases range by 50%");
                changeList.Add("- Decreases damage by 25%");
                break;
            
            case 8:
                runeName = "Rune of Daring";
                changeList.Add("- Increases damage by 50%");
                changeList.Add("- Decreases max HP by 1");
                break;
            
            case 9:
                runeName = "Rune of Frag";
                changeList.Add("- Increases damage by 100%");
                changeList.Add("- Decreases max HP by 2");
                break;
            
            case 10:
                runeName = "Rune of Lasting";
                changeList.Add("- Increases range by 75%");
                break;
            
            case 11:
                runeName = "Rune of Frost";
                changeList.Add("- Increases range by 40%");
                changeList.Add("- Increases max HP by 1");
                changeList.Add("- Decreases player speed by 25%");
                break;
            
            case 12:
                runeName = "Rune of Preci";
                changeList.Add("- Increases damage by 50%");
                changeList.Add("- Decreases hit leeway by 25%");
                break;
            
            case 13:
                runeName = "Shotgun Spell";
                changeList.Add("- Grants the Shotty weapon");
                break;
            
            case 14:
                runeName = "Rifle Ritual";
                changeList.Add("- Grants the Rifle weapon");
                break;
        }

        GameObject targetPanel = GameObject.Find("RunePanel");
        runePanel = Instantiate(targetPanel, GameObject.Find("Canvas").GetComponent<RectTransform>());

        for (int i = 0; i <= 4; i++)
        {
            GameObject obj = runePanel.transform.GetChild(i).gameObject;
            if (obj.name == "RuneNameText")
            {
                obj.GetComponent<UnityEngine.UI.Text>().text = runeName;
            }
            if (obj.name == "ChangeListText1")
            {
                obj.GetComponent<UnityEngine.UI.Text>().text = changeList[0];
            }
            if (obj.name == "ChangeListText2")
            {
                if (changeList.Count < 2)
                {
                    obj.GetComponent<UnityEngine.UI.Text>().text = "";
                }
                else
                {
                    obj.GetComponent<UnityEngine.UI.Text>().text = changeList[1];
                }
            }
            if (obj.name == "ChangeListText3")
            {
                if (changeList.Count < 3)
                {
                    obj.GetComponent<UnityEngine.UI.Text>().text = "";
                }
                else
                {
                    obj.GetComponent<UnityEngine.UI.Text>().text = changeList[2];
                }
            }
        }
    }

    void Update()
    {
        if (active) {return;};
        if (Input.GetKeyDown("e") && runePanel.GetComponent<UnityEngine.UI.Image>().enabled)
        {
            active = true;
            runePanel.GetComponent<UnityEngine.UI.Image>().enabled = false;
            runePanel.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Text>().enabled = false;
            runePanel.transform.GetChild(1).gameObject.GetComponent<UnityEngine.UI.Text>().enabled = false;
            runePanel.transform.GetChild(2).gameObject.GetComponent<UnityEngine.UI.Text>().enabled = false;
            runePanel.transform.GetChild(3).gameObject.GetComponent<UnityEngine.UI.Text>().enabled = false;
            runePanel.transform.GetChild(4).gameObject.GetComponent<UnityEngine.UI.Text>().enabled = false;

            PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

            switch (runeNum)
            {
                case 1:
                    player.maxHealth += 2;
                    player.hp += 2;
                    player.permSpeedBuff *= 0.6f;
                    break;
                
                case 2:
                    player.permPowerBuff *= 1.5f;
                    player.shotSpeedBuff *= 0.75f;
                    break;
                
                case 3:
                    player.permSpeedBuff *= 1.75f;
                    break;
                
                case 4:
                    player.extraBulletsFired += 1;
                    player.permPowerBuff *= 0.75f;
                    break;
                
                case 5:
                    player.ouchImmunity = true;
                    player.permSpeedBuff *= 0.7f;
                    break;
                
                case 6:
                    player.permPowerBuff *= 1.5f;
                    player.permSpeedBuff *= 0.75f;
                    break;
                
                case 7:
                    player.rangeBuff *= 1.5f;
                    player.permPowerBuff *= 0.75f;
                    break;
                
                case 8:
                    player.permPowerBuff *= 1.5f;
                    if (player.hp == player.maxHealth) {player.hp -= 1;};
                    player.maxHealth -= 1;
                    break;
                
                case 9:
                    player.permPowerBuff *= 2;
                    if (player.hp == player.maxHealth) {player.hp -= 2;}
                    else if (player.hp == player.maxHealth - 1) {player.hp -= 1;};
                    player.maxHealth -= 2;
                    break;
                
                case 10:
                    player.rangeBuff *= 1.75f;
                    break;
                
                case 11:
                    player.rangeBuff *= 1.4f;
                    player.maxHealth += 1;
                    player.hp += 1;
                    player.permSpeedBuff *= 0.75f;
                    break;
                
                case 12:
                    player.permPowerBuff *= 1.5f;
                    player.hitLeewayBuff *= 0.75f;
                    break;
                
                case 13:
                    player.SetWeapon(3);
                    break;
                
                case 14:
                    player.SetWeapon(2);
                    break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (active) {return;};
        if (collision.tag != "Player") {return;};
        runePanel.GetComponent<UnityEngine.UI.Image>().enabled = true;
        runePanel.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Text>().enabled = true;
        runePanel.transform.GetChild(1).gameObject.GetComponent<UnityEngine.UI.Text>().enabled = true;
        runePanel.transform.GetChild(2).gameObject.GetComponent<UnityEngine.UI.Text>().enabled = true;
        runePanel.transform.GetChild(3).gameObject.GetComponent<UnityEngine.UI.Text>().enabled = true;
        runePanel.transform.GetChild(4).gameObject.GetComponent<UnityEngine.UI.Text>().enabled = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (active) {return;};
        if (collision.tag != "Player") {return;};
        runePanel.GetComponent<UnityEngine.UI.Image>().enabled = false;
        runePanel.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Text>().enabled = false;
        runePanel.transform.GetChild(1).gameObject.GetComponent<UnityEngine.UI.Text>().enabled = false;
        runePanel.transform.GetChild(2).gameObject.GetComponent<UnityEngine.UI.Text>().enabled = false;
        runePanel.transform.GetChild(3).gameObject.GetComponent<UnityEngine.UI.Text>().enabled = false;
        runePanel.transform.GetChild(4).gameObject.GetComponent<UnityEngine.UI.Text>().enabled = false;
    }
}
