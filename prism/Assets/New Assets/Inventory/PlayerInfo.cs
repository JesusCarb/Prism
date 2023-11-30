using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInfo", menuName = "PlayerStuff")]
public class PlayerInfo : ScriptableObject
{
    public string weapon1;
    public string weapon2;
    public string weapon3;
    public float permPowerBuff = 1;
    public float permSpeedBuff = 1;
    public int maxHealth = 3;
    public int extraBulletsFired = 0;
    public float shotSpeedBuff = 1;
    public float rangeBuff = 1;
    public float hitLeewayBuff = 1;
    public bool ouchImmunity = false;
    public int floorNum = 1;
    public int savedHP = 3;
    public string lastWeaponEquipped = "Pistol";

    void OnEnable()
    {
        weapon1 = "";
        weapon2 = "";
        weapon3 = "";
        permPowerBuff = 1;
        permSpeedBuff = 1;
        maxHealth = 3;
        extraBulletsFired = 0;
        shotSpeedBuff = 1;
        rangeBuff = 1;
        hitLeewayBuff = 1;
        ouchImmunity = false;
        floorNum = 1;
        savedHP = 3;
        lastWeaponEquipped = "Pistol";
    }
}
