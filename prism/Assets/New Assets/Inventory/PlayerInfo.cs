using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInfo", menuName = "PlayerStuff")]
public class PlayerInfo : ScriptableObject
{
    public string weapon1;
    public string weapon2;
    public string weapon3;
    public int permPowerBuff = 1;
    public float permSpeedBuff = 1;
    public int maxHealth = 3;

    void OnEnable()
    {
        weapon1 = "";
        weapon2 = "";
        weapon3 = "";
        permPowerBuff = 1;
        permSpeedBuff = 1;
        maxHealth = 3;
    }
}
