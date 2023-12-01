using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
// using System.Numerics;
// using System.Numerics;
using UnityEngine.SceneManagement;
using System;
using UnityEngine;
using JetBrains.Annotations;
using System.Diagnostics.Tracing;
using DG.Tweening;
//using Microsoft.Unity.VisualStudio.Editor;

public class PlayerController : MonoBehaviour
{
    //player info stuff (for scene switching)
    [SerializeField]
    public PlayerInfo playerInfo;
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


    
    //weapons
    [SerializeField]
    private GameObject pistol;

    [SerializeField]
    private GameObject rifle;

    [SerializeField]
    private GameObject shotty;

    public AudioClip pistolAudio;
    public AudioClip rifleAudio;
    public AudioClip shottyAudio;

    public AudioClip weaponJam;


    // player vars
    public int hp = 3;
    public float moveSpeed = 40f;
    public float damageMultiplier = 1;
    public int numShards = 0;
    public int totalShards = 0;
    Vector2 playerInput;
    Vector3 velocity;
    float velocityXSmoothing;
    float velocityYSmoothing;
    public float accelerationTime = .1f;

    private Rigidbody2D rb;

    // cursor vars
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotspot = Vector2.zero;

    // beat calculation vars
    public bool beatChange;
    public float BPM;
    private bool onBeat;
    private float timeLastBeat;
    private float timeNextBeat;
    //private float beatOffset = .15f;

    // percentage betwween beats to hit
    public float hitLeeway = .25f;
    float beatsPerSecond;
    private float period;
    private float halfperiod;
    private float tweenDelay;
    private bool countingTweenDelay = false;
    private bool barIsUp = true;
    private bool OnOffTracker = false;
    

    // audio vars
    public MusicInfo musicInfo;
    public AudioSource audioSource;
    public AudioSource songAudioSource0;
    public AudioSource songAudioSource1;
    public AudioSource songAudioSource2;
    public AudioSource songAudioSource3;
    public AudioSource songAudioSource4;
    public AudioSource songAudioSource5;
    public AudioSource songAudioSource6;
    public AudioSource songAudioSource7;
    public AudioSource songAudioSource8;
    public AudioSource songAudioSource9;
    public AudioSource songAudioSource10;

    public AudioSource endingSongAudioSource;


    private Animator anim;

    // enemy timing vars
    private bool enemyFireBeat = false;

    // only L/R movement for start screen
    public bool inOverworld = false;

    // to fix bug
    public bool noHitWindowGraphic = false;

    private enum Walk
    {
        Not,
        Forward,
        Backward
    }
    private int currentWeapon;
    private enum Weapon
    {
        None,
        Pistol,
        Rifle,
        Shotty
    }
    private float songTimeStart;
    int beatCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        weapon1 = playerInfo.weapon1;
        weapon2 = playerInfo.weapon2;
        weapon3 = playerInfo.weapon3;
        permPowerBuff = playerInfo.permPowerBuff;
        permSpeedBuff = playerInfo.permSpeedBuff;
        maxHealth = playerInfo.maxHealth;
        extraBulletsFired = playerInfo.extraBulletsFired;
        shotSpeedBuff = playerInfo.shotSpeedBuff;
        rangeBuff = playerInfo.rangeBuff;
        hitLeewayBuff = playerInfo.hitLeewayBuff;
        ouchImmunity = playerInfo.ouchImmunity;
        floorNum = playerInfo.floorNum;
        savedHP = playerInfo.savedHP;
        lastWeaponEquipped = playerInfo.lastWeaponEquipped;

        switch (floorNum)
        {
            case 1:
                BPM = 100;
                break;
            
            case 2:
                BPM = 125;
                break;
            
            case 3:
                BPM = 100;
                break;
            
            case 4:
                BPM = 90;
                break;
            
            case 5:
                BPM = 120;
                break;
            
            case 6:
                BPM = 125;
                break;
            
            case 7:
                BPM = 130;
                break;
            
            case 8:
                BPM = 120;
                break;
            
            case 9:
                BPM = 100;
                break;
            
            case 10:
                BPM = 147;
                break;
        }
        
        rb = GetComponent<Rigidbody2D>();
        //BPM = musicInfo.BPM;
        onBeat = false;
        CalculateTimings();
        UnityEngine.Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
        beatChange = false;
        // PlayMusicWrapper();

        currentWeapon = (int)Weapon.None;
                // get animator 
        anim = GetComponent<Animator>();

        hp = savedHP;

        if (SceneManager.GetActiveScene().name == "OVERWORLD") {return;};

        if (weapon1 == "Pistol") {GameObject.Find("Panel1").transform.Find("Pistol").GetComponent<UnityEngine.UI.Image>().enabled = true;}
        else if (weapon1 == "Rifle") {GameObject.Find("Panel1").transform.Find("Rifle").GetComponent<UnityEngine.UI.Image>().enabled = true;}
        else if (weapon1 == "Shotty") {GameObject.Find("Panel1").transform.Find("Shotty").GetComponent<UnityEngine.UI.Image>().enabled = true;};

        if (weapon2 == "Pistol") {GameObject.Find("Panel2").transform.Find("Pistol").GetComponent<UnityEngine.UI.Image>().enabled = true;}
        else if (weapon2 == "Rifle") {GameObject.Find("Panel2").transform.Find("Rifle").GetComponent<UnityEngine.UI.Image>().enabled = true;}
        else if (weapon2 == "Shotty") {GameObject.Find("Panel2").transform.Find("Shotty").GetComponent<UnityEngine.UI.Image>().enabled = true;};

        if (weapon3 == "Pistol") {GameObject.Find("Panel3").transform.Find("Pistol").GetComponent<UnityEngine.UI.Image>().enabled = true;}
        else if (weapon3 == "Rifle") {GameObject.Find("Panel3").transform.Find("Rifle").GetComponent<UnityEngine.UI.Image>().enabled = true;}
        else if (weapon3 == "Shotty") {GameObject.Find("Panel3").transform.Find("Shotty").GetComponent<UnityEngine.UI.Image>().enabled = true;};

        if (lastWeaponEquipped == "Pistol")
        {
            SetWeapon(1);
        }
        else if (lastWeaponEquipped == "Rifle")
        {
            SetWeapon(2);
        }
        else if (lastWeaponEquipped == "Shotty")
        {
            SetWeapon(3);
        }

        //PlayerController playerC = gameObject.GetComponent<PlayerController>();
        //GameObject.Find("ShardCountText").GetComponent<UnityEngine.UI.Text>().text = playerC.numShards + " / " + playerC.totalShards;

        // SetWeapon((int)Weapon.Rifle);
    }

    void Update()
    {
        //sorry this is so you can equip previously acquired weapons
        if (Input.GetKeyDown("1"))
        {
            //SetWeapon(1);
            if (weapon1 == "Pistol")
            {
                SetWeapon(1);
            }
            if (weapon1 == "Rifle")
            {
                SetWeapon(2);
            }
            if (weapon1 == "Shotty")
            {
                SetWeapon(3);
            }
        }
        if (Input.GetKeyDown("2"))
        {
            //SetWeapon(2);
            if (weapon2 == "Pistol")
            {
                SetWeapon(1);
            }
            if (weapon2 == "Rifle")
            {
                SetWeapon(2);
            }
            if (weapon2 == "Shotty")
            {
                SetWeapon(3);
            }
        }
        if (Input.GetKeyDown("3"))
        {
            //SetWeapon(3);
            if (weapon3 == "Pistol")
            {
                SetWeapon(1);
            }
            if (weapon3 == "Rifle")
            {
                SetWeapon(2);
            }
            if (weapon3 == "Shotty")
            {
                SetWeapon(3);
            }
        }

        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        BeatTracker();
        TweenTimer();
        BeatBlocks();
        PlayBeat();
        AdjustHealth();
        if(hp <= 0)
        {
            StartCoroutine(FailureState());
        }
        else
        {
            SetPlayerAnimations();
        }
        //print(hp);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 mouseLoc = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        Vector3 finalPos = mouseLoc - transform.position;
        if(finalPos.x < 0)
        {
            this.transform.localScale = new Vector3(-1 * Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }
        else
        {
            this.transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);

        }

        float targetVelocityX = playerInput.x * moveSpeed * permSpeedBuff;
        float targetVelocityY = playerInput.y * moveSpeed * permSpeedBuff;

        // velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTime);
        // velocity.y = Mathf.SmoothDamp(velocity.y, targetVelocityY, ref velocityYSmoothing, accelerationTime);

        velocity.x = targetVelocityX;

        if (inOverworld) // start screen
            velocity.y = 0f;
        else
            velocity.y = targetVelocityY;

        Move(velocity);

    }

    public void SetWeapon(int type)
    {
        /*
        if(currentWeapon != (int)Weapon.None)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Weapon");
            Destroy(obj);
        }
        switch(type)
        {
            case 1:
                    Instantiate(pistol, this.transform);
                    currentWeapon = (int)Weapon.Pistol;

            break;
            case 2:
                    Instantiate(rifle, this.transform);
                    currentWeapon = (int)Weapon.Rifle;

            break;
            case 3:
                    Instantiate(shotty, this.transform);
                    currentWeapon = (int)Weapon.Shotty;

            break;

            default:
            break;
        }
        */
        
        if(currentWeapon != (int)Weapon.None)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Weapon");
            Destroy(obj);
        }

        switch (type)
        {
            case 1:
                if (weapon1 != "Pistol" && weapon2 != "Pistol" && weapon3 != "Pistol")
                {
                    weapon3 = weapon2;
                    if (weapon3 == "Rifle")
                    {
                        GameObject.Find("Panel3").transform.Find("Rifle").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = true;
                        GameObject.Find("Panel2").transform.Find("Rifle").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = false;
                    }
                    else if (weapon3 == "Shotty")
                    {
                        GameObject.Find("Panel3").transform.Find("Shotty").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = true;
                        GameObject.Find("Panel2").transform.Find("Shotty").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = false;
                    }

                    weapon2 = weapon1;
                    if (weapon2 == "Rifle")
                    {
                        GameObject.Find("Panel2").transform.Find("Rifle").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = true;
                        GameObject.Find("Panel1").transform.Find("Rifle").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = false;
                    }
                    else if (weapon2 == "Shotty")
                    {
                        GameObject.Find("Panel2").transform.Find("Shotty").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = true;
                        GameObject.Find("Panel1").transform.Find("Shotty").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = false;
                    }

                    weapon1 = "Pistol";
                    GameObject.Find("Panel1").transform.Find("Pistol").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = true;
                }

                Instantiate(pistol, this.transform);
                currentWeapon = (int)Weapon.Pistol;
                lastWeaponEquipped = "Pistol";
                break;
                
            case 2:
                if (weapon1 != "Rifle" && weapon2 != "Rifle" && weapon3 != "Rifle")
                {
                    weapon3 = weapon2;
                    if (weapon3 == "Pistol")
                    {
                        GameObject.Find("Panel3").transform.Find("Pistol").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = true;
                        GameObject.Find("Panel2").transform.Find("Pistol").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = false;
                    }
                    else if (weapon3 == "Shotty")
                    {
                        GameObject.Find("Panel3").transform.Find("Shotty").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = true;
                        GameObject.Find("Panel2").transform.Find("Shotty").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = false;
                    }

                    weapon2 = weapon1;
                    if (weapon2 == "Pistol")
                    {
                        GameObject.Find("Panel2").transform.Find("Pistol").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = true;
                        GameObject.Find("Panel1").transform.Find("Pistol").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = false;
                    }
                    else if (weapon2 == "Shotty")
                    {
                        GameObject.Find("Panel2").transform.Find("Shotty").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = true;
                        GameObject.Find("Panel1").transform.Find("Shotty").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = false;
                    }

                    weapon1 = "Rifle";
                    GameObject.Find("Panel1").transform.Find("Rifle").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = true;
                }

                Instantiate(rifle, this.transform);
                currentWeapon = (int)Weapon.Rifle;
                lastWeaponEquipped = "Rifle";
                break;

            case 3:
                if (weapon1 != "Shotty" && weapon2 != "Shotty" && weapon3 != "Shotty")
                {
                    weapon3 = weapon2;
                    if (weapon3 == "Pistol")
                    {
                        GameObject.Find("Panel3").transform.Find("Pistol").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = true;
                        GameObject.Find("Panel2").transform.Find("Pistol").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = false;
                    }
                    else if (weapon3 == "Rifle")
                    {
                        GameObject.Find("Panel3").transform.Find("Rifle").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = true;
                        GameObject.Find("Panel2").transform.Find("Rifle").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = false;
                    }

                    weapon2 = weapon1;
                    if (weapon2 == "Pistol")
                    {
                        GameObject.Find("Panel2").transform.Find("Pistol").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = true;
                        GameObject.Find("Panel1").transform.Find("Pistol").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = false;
                    }
                    else if (weapon2 == "Rifle")
                    {
                        GameObject.Find("Panel2").transform.Find("Rifle").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = true;
                        GameObject.Find("Panel1").transform.Find("Rifle").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = false;
                    }

                    weapon1 = "Shotty";
                    GameObject.Find("Panel1").transform.Find("Shotty").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = true;
                }

                Instantiate(shotty, this.transform);
                currentWeapon = (int)Weapon.Shotty;
                lastWeaponEquipped = "Shotty";
                break;
        }
    }

    public void Move(Vector2 velocity)
    {
        rb.velocity = velocity;
    }

    void CalculateTimings()
    {
        beatsPerSecond = BPM/60;
        period = 1f / beatsPerSecond;

        timeLastBeat = Time.time % period;
        timeNextBeat = 1 - (Time.time % period);

        // print("period" + period);
        // print("bpS" + beatsPerSecond);

        // print("timeLastBeat" + timeLastBeat);
        // print("timeNextBeat" + timeNextBeat);
 
    }

        // spaghetti code :)

        // yum i ated it all
        // -remy

    void SetPlayerAnimations()
    {
        float velocityNorm = (float)Math.Sqrt(velocity.x * velocity.x + velocity.y * velocity.y);
        anim.SetInteger("Direction", (int)this.transform.localScale.x);

        if(velocityNorm == 0)
        {
            anim.SetInteger("Direction", (int)Walk.Not);

        }
        else if((velocity.x > 0) && this.transform.localScale.x < 0
        || (velocity.x < 0) && this.transform.localScale.x > 0)
        {

            anim.SetInteger("Direction", (int)Walk.Backward);
            
        }
        else
        {
            anim.SetInteger("Direction", (int)Walk.Forward);

        }
    }

    void BeatTracker()
    {
        float currentTime = Time.deltaTime + timeLastBeat;
        float newTimelastBeat = currentTime % period;
        float newTimeNextBeat = period - (currentTime % period);


        // print(currentTime);

        if (newTimeNextBeat > timeNextBeat)
        {
            beatChange = true;
            enemyFireBeat = true;
        }
        else
        {
            beatChange = false;
            enemyFireBeat = false;
        }    
            
        
        timeLastBeat = newTimelastBeat;
        timeNextBeat = newTimeNextBeat;

        // extra 0.18f added because it's more likely to click early
        // to align unfortunately delayed sound with beat
        float normalTLB = timeLastBeat / period;
        float normalTNB = timeNextBeat / period;

        //print("timeLastBeat" + normalTLB + " " + onBeat);
        //print("timeNextBeat" + normalTNB + " " + onBeat);

        if(normalTLB < hitLeeway * hitLeewayBuff || normalTNB < hitLeeway * hitLeewayBuff)
        {
            onBeat = true;
        }
        else
        {
            onBeat = false;
        }
        // print(onBeat);
    }
    
    private void PlayBeat()
    {
        if(beatCounter == 1 && beatChange == true)
        {
            // PlayMusicWrapper();
            PlayMusic();
            beatCounter += 1;

        }else if(beatChange)
        {
            beatCounter += 1;
        }
        // if(beatChange)
        // {
        //     // audioSource.Play();
        //     beatCounter += 1;;

        // }

    }

    private void PlayMusicWrapper()
    {
        // Add delay to start of music to match up with beats
        // trying new song without early delay
        // songAudioSource.Play();

        Invoke("PlayMusic", 0.0f);
    }

    public void PlayEndingMusic()
    {
        endingSongAudioSource.Play();

    }

    private void PlayMusic()
    {
        GameObject fadeOverlay = GameObject.Find("FadeOverlayPanel");
        if (fadeOverlay != null)
        {
            fadeOverlay.GetComponent<UnityEngine.UI.Image>().color = new Color(0,0,0,0);
        }

        if (SceneManager.GetActiveScene().name == "OVERWORLD")
        {
            songAudioSource0.loop = true;
            songAudioSource0.Play();  //menu music
            return;
        }

        switch (floorNum)
        {
            case 1:
                songAudioSource1.loop = true;
                songAudioSource1.Play();
                break;
            
            case 2:
                songAudioSource2.loop = true;
                songAudioSource2.Play();
                break;
            
            case 3: 
                songAudioSource3.loop = true;
                songAudioSource3.Play();
                break;
            
            case 4:
                songAudioSource4.loop = true;
                songAudioSource4.Play();
                break;
            
            case 5:
                songAudioSource5.loop = true;
                songAudioSource5.Play();
                break;
            
            case 6:
                songAudioSource6.loop = true;
                songAudioSource6.Play();
                break;
            
            case 7:
                songAudioSource7.loop = true;
                songAudioSource7.Play();
                break;
            
            case 8:
                songAudioSource8.loop = true;
                songAudioSource8.Play();
                break;
            
            case 9:
                songAudioSource9.loop = true;
                songAudioSource9.Play();
                break;

            case 10:
                songAudioSource10.loop = true;
                songAudioSource10.Play();
                break;
            
            default:
                songAudioSource0.loop = true;
                songAudioSource0.Play();  //menu music
                break;
        }
        
    }

    public bool OnBeat()
    {
        return onBeat;
    }

    public float GetPeriod()
    {
        return period;
    }

    public bool GetEnemyFireBeat()
    {
        return enemyFireBeat;
    }

    private IEnumerator FailureState()
    {
        if(hp <= 0)
        {
            // when dead, pause time, wait 1 second, then transition to main menu
            anim.SetInteger("Direction", -1);
            print(anim.GetInteger("Direction"));
            
            yield return new WaitForSecondsRealtime(.05f);

            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(2);

            Time.timeScale = 1;

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

            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            SceneManager.LoadScene("OVERWORLD");
            // timescale will stay at zero so I'm turning it back to 1
        }

    }

    void AdjustHealth()
    {
        if (inOverworld)
            return;

        //im so sorry
        switch(maxHealth)
        {
            case 1:
                GameObject.Find("Heart1").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart1Gone").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart2").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart2Gone").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart3").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart3Gone").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart4").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart4Gone").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart5").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart5Gone").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart6").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart6Gone").GetComponent<UnityEngine.UI.Image>().enabled = false;
                break;

            case 2:
                GameObject.Find("Heart1").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart1Gone").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart2").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart2Gone").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart3").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart3Gone").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart4").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart4Gone").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart5").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart5Gone").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart6").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart6Gone").GetComponent<UnityEngine.UI.Image>().enabled = false;
                break;
            
            case 3:
                GameObject.Find("Heart1").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart1Gone").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart2").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart2Gone").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart3").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart3Gone").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart4").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart4Gone").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart5").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart5Gone").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart6").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart6Gone").GetComponent<UnityEngine.UI.Image>().enabled = false;
                break;
            
            case 4:
                GameObject.Find("Heart1").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart1Gone").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart2").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart2Gone").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart3").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart3Gone").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart4").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart4Gone").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart5").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart5Gone").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart6").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart6Gone").GetComponent<UnityEngine.UI.Image>().enabled = false;
                break;
            
            case 5:
                GameObject.Find("Heart1").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart1Gone").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart2").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart2Gone").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart3").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart3Gone").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart4").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart4Gone").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart5").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart5Gone").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart6").GetComponent<UnityEngine.UI.Image>().enabled = false;
                GameObject.Find("Heart6Gone").GetComponent<UnityEngine.UI.Image>().enabled = false;
                break;
            
            case 6:
                GameObject.Find("Heart1").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart1Gone").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart2").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart2Gone").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart3").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart3Gone").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart4").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart4Gone").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart5").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart5Gone").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart6").GetComponent<UnityEngine.UI.Image>().enabled = true;
                GameObject.Find("Heart6Gone").GetComponent<UnityEngine.UI.Image>().enabled = true;
                break;
        }
        
        //im so so sorry why does this go backwards welp i dont feel like changing it
        switch (hp)
        {
            case 6:
                GameObject.Find("Heart6").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart6Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart5").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart5Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart4").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart4Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart3").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart3Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart2").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart2Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart1").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart1Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                break;
            
            case 5:
                GameObject.Find("Heart6").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart6Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                GameObject.Find("Heart5").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart5Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart4").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart4Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart3").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart3Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart2").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart2Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart1").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart1Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                break;
            
            case 4:
                GameObject.Find("Heart6").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart6Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                GameObject.Find("Heart5").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart5Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                GameObject.Find("Heart4").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart4Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart3").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart3Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart2").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart2Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart1").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart1Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                break;
            
            case 3:
                GameObject.Find("Heart6").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart6Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                GameObject.Find("Heart5").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart5Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                GameObject.Find("Heart4").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart4Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                GameObject.Find("Heart3").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart3Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart2").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart2Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart1").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart1Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                break;

            case 2:
                GameObject.Find("Heart6").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart6Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                GameObject.Find("Heart5").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart5Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                GameObject.Find("Heart4").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart4Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                GameObject.Find("Heart3").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart3Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                GameObject.Find("Heart2").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart2Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart1").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart1Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                break;

            case 1:
                GameObject.Find("Heart6").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart6Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                GameObject.Find("Heart5").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart5Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                GameObject.Find("Heart4").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart4Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                GameObject.Find("Heart3").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart3Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                GameObject.Find("Heart2").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart2Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                GameObject.Find("Heart1").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart1Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                break;
            
            case 0:
                GameObject.Find("Heart6").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart6Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                GameObject.Find("Heart5").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart5Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                GameObject.Find("Heart4").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart4Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                GameObject.Find("Heart3").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart3Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                GameObject.Find("Heart2").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart2Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                GameObject.Find("Heart1").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart1Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                break;
        }
    }

    private void BeatBlocks()
    {
        if(beatChange)
        {
            if (OnOffTracker)
            {
                OnOffTracker = false;
            }
            else
            {
                OnOffTracker = true;
            }
            
            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Ouch"))
            {
                if (obj.name == "BeatOuchOn")
                {
                    BoxCollider2D objCollider = obj.GetComponent<BoxCollider2D>();
                    PolygonCollider2D objPolyCollider = obj.GetComponent<PolygonCollider2D>();

                    if (OnOffTracker)
                    {
                        //obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                        Color clr = obj.GetComponent<SpriteRenderer>().color;
                        obj.GetComponent<SpriteRenderer>().color = new Color(clr.r, clr.g, clr.b, 1);

                        if (objCollider != null)
                        {
                            objCollider.enabled = true;
                        }
                        if (objPolyCollider != null)
                        {
                            objPolyCollider.enabled = true;
                        }
                    }
                    else
                    {
                        //obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                        Color clr = obj.GetComponent<SpriteRenderer>().color;
                        obj.GetComponent<SpriteRenderer>().color = new Color(clr.r, clr.g, clr.b, 0.5f);

                        if (objCollider != null)
                        {
                            objCollider.enabled = false;
                        }
                        if (objPolyCollider != null)
                        {
                            objPolyCollider.enabled = false;
                        }
                    }
                    
                }

                if (obj.name == "BeatOuchOff")
                {
                    BoxCollider2D objCollider = obj.GetComponent<BoxCollider2D>();
                    PolygonCollider2D objPolyCollider = obj.GetComponent<PolygonCollider2D>();
                    
                    if (OnOffTracker)
                    {
                        //obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                        Color clr = obj.GetComponent<SpriteRenderer>().color;
                        obj.GetComponent<SpriteRenderer>().color = new Color(clr.r, clr.g, clr.b, 0.5f);

                        if (objCollider != null)
                        {
                            objCollider.enabled = false;
                        }
                        if (objPolyCollider != null)
                        {
                            objPolyCollider.enabled = false;
                        }
                    }
                    else
                    {
                        //obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                        Color clr = obj.GetComponent<SpriteRenderer>().color;
                        obj.GetComponent<SpriteRenderer>().color = new Color(clr.r, clr.g, clr.b, 1);

                        if (objCollider != null)
                        {
                            objCollider.enabled = true;
                        }
                        if (objPolyCollider != null)
                        {
                            objPolyCollider.enabled = true;
                        }
                    }
                    
                }
            }

            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Obstacle"))
            {
                if (obj.name == "BeatObstacleOn")
                {
                    BoxCollider2D objCollider = obj.GetComponent<BoxCollider2D>();
                    PolygonCollider2D objPolyCollider = obj.GetComponent<PolygonCollider2D>();

                    if (OnOffTracker)
                    {
                        //obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                        Color clr = obj.GetComponent<SpriteRenderer>().color;
                        obj.GetComponent<SpriteRenderer>().color = new Color(clr.r, clr.g, clr.b, 1);

                        if (objCollider != null)
                        {
                            objCollider.enabled = true;
                        }
                        if (objPolyCollider != null)
                        {
                            objPolyCollider.enabled = true;
                        }
                    }
                    else
                    {
                        //obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                        Color clr = obj.GetComponent<SpriteRenderer>().color;
                        obj.GetComponent<SpriteRenderer>().color = new Color(clr.r, clr.g, clr.b, 0.5f);

                        if (objCollider != null)
                        {
                            objCollider.enabled = false;
                        }
                        if (objPolyCollider != null)
                        {
                            objPolyCollider.enabled = false;
                        }
                    }
                    
                }

                if (obj.name == "BeatObstacleOff")
                {
                    BoxCollider2D objCollider = obj.GetComponent<BoxCollider2D>();
                    PolygonCollider2D objPolyCollider = obj.GetComponent<PolygonCollider2D>();
                    
                    if (OnOffTracker)
                    {
                        //obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                        Color clr = obj.GetComponent<SpriteRenderer>().color;
                        obj.GetComponent<SpriteRenderer>().color = new Color(clr.r, clr.g, clr.b, 0.5f);

                        if (objCollider != null)
                        {
                            objCollider.enabled = false;
                        }
                        if (objPolyCollider != null)
                        {
                            objPolyCollider.enabled = false;
                        }
                    }
                    else
                    {
                        //obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                        Color clr = obj.GetComponent<SpriteRenderer>().color;
                        obj.GetComponent<SpriteRenderer>().color = new Color(clr.r, clr.g, clr.b, 1);

                        if (objCollider != null)
                        {
                            objCollider.enabled = true;
                        }
                        if (objPolyCollider != null)
                        {
                            objPolyCollider.enabled = true;
                        }
                    }
                    
                }
            }

            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("OuchObstacle"))
            {
                if (obj.name == "BeatOuchObstacleOn")
                {
                    BoxCollider2D objCollider = obj.GetComponent<BoxCollider2D>();
                    PolygonCollider2D objPolyCollider = obj.GetComponent<PolygonCollider2D>();

                    if (OnOffTracker)
                    {
                        //obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                        Color clr = obj.GetComponent<SpriteRenderer>().color;
                        obj.GetComponent<SpriteRenderer>().color = new Color(clr.r, clr.g, clr.b, 1);

                        if (objCollider != null)
                        {
                            objCollider.enabled = true;
                        }
                        if (objPolyCollider != null)
                        {
                            objPolyCollider.enabled = true;
                        }
                    }
                    else
                    {
                        //obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                        Color clr = obj.GetComponent<SpriteRenderer>().color;
                        obj.GetComponent<SpriteRenderer>().color = new Color(clr.r, clr.g, clr.b, 0.5f);

                        if (objCollider != null)
                        {
                            objCollider.enabled = false;
                        }
                        if (objPolyCollider != null)
                        {
                            objPolyCollider.enabled = false;
                        }
                    }
                    
                }

                if (obj.name == "BeatOuchObstacleOff")
                {
                    BoxCollider2D objCollider = obj.GetComponent<BoxCollider2D>();
                    PolygonCollider2D objPolyCollider = obj.GetComponent<PolygonCollider2D>();
                    
                    if (OnOffTracker)
                    {
                        //obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                        Color clr = obj.GetComponent<SpriteRenderer>().color;
                        obj.GetComponent<SpriteRenderer>().color = new Color(clr.r, clr.g, clr.b, 0.5f);

                        if (objCollider != null)
                        {
                            objCollider.enabled = false;
                        }
                        if (objPolyCollider != null)
                        {
                            objPolyCollider.enabled = false;
                        }
                    }
                    else
                    {
                        //obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                        Color clr = obj.GetComponent<SpriteRenderer>().color;
                        obj.GetComponent<SpriteRenderer>().color = new Color(clr.r, clr.g, clr.b, 1);

                        if (objCollider != null)
                        {
                            objCollider.enabled = true;
                        }
                        if (objPolyCollider != null)
                        {
                            objPolyCollider.enabled = true;
                        }
                    }
                    
                }
            }
        }
        
    }

    private void TweenTimer()
    {
        if (beatChange)
        {
            halfperiod = period / 2;
            tweenDelay = 0;
            countingTweenDelay = true;
        }

        if (countingTweenDelay)
        {
            tweenDelay += Time.deltaTime;
        }
        else
        {
            tweenDelay = 0;
        }

        if (tweenDelay > halfperiod)
        {
            // jesus deleted mom "sorry" he says
            // print("mom");
            //insert tween here
            if (barIsUp)
            {
                if (!noHitWindowGraphic)
                {
                    //tween downwards
                    RectTransform targetTransform = GameObject.Find("TimingBar").GetComponent<RectTransform>();
                    targetTransform.DOMoveY(GameObject.Find("DownPosition").GetComponent<RectTransform>().position.y, period, false).SetEase(Ease.InOutQuad);
                    barIsUp = false;
                }
                
            }
            else
            {
                if (!noHitWindowGraphic)
                {
                    //tween upwards
                    RectTransform targetTransform = GameObject.Find("TimingBar").GetComponent<RectTransform>();
                    targetTransform.DOMoveY(GameObject.Find("UpPosition").GetComponent<RectTransform>().position.y, period, false).SetEase(Ease.InOutQuad);
                    barIsUp = true;
                }
                    
            }
            countingTweenDelay = false;
        }
    }
}
