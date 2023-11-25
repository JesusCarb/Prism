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
    public int damageMultiplier = 1;
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
    bool beatChange;
    public float BPM;
    private bool onBeat;
    private float timeLastBeat;
    private float timeNextBeat;
    private float beatOffset = .15f;

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
    public AudioSource songAudioSource;

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
        rb = GetComponent<Rigidbody2D>();
        //BPM = musicInfo.BPM;
        onBeat = false;
        CalculateTimings();
        UnityEngine.Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
        beatChange = false;
        // PlayMusicWrapper();
        
        weapon1 = playerInfo.weapon1;
        weapon2 = playerInfo.weapon2;
        weapon3 = playerInfo.weapon3;
        permPowerBuff = playerInfo.permPowerBuff;
        permSpeedBuff = playerInfo.permSpeedBuff;
        maxHealth = playerInfo.maxHealth;

        currentWeapon = (int)Weapon.None;
                // get animator 
        anim = GetComponent<Animator>();

        // SetWeapon((int)Weapon.Rifle);
    }

    void Update()
    {
        //sorry this is so you can equip previously acquired weapons
        if (Input.GetKeyDown("1"))
        {
            SetWeapon(1);
        }
        if (Input.GetKeyDown("2"))
        {
            SetWeapon(2);
        }
        if (Input.GetKeyDown("3"))
        {
            SetWeapon(3);
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

        float targetVelocityX = playerInput.x * moveSpeed;
        float targetVelocityY = playerInput.y * moveSpeed;

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
                weapon3 = weapon2;
                weapon2 = weapon1;
                weapon1 = "Pistol";

                Instantiate(pistol, this.transform);
                currentWeapon = (int)Weapon.Pistol;
                break;
                
            case 2:
                weapon3 = weapon2;
                weapon2 = weapon1;
                weapon1 = "Rifle";

                Instantiate(rifle, this.transform);
                currentWeapon = (int)Weapon.Rifle;
                break;

            case 3:
                weapon3 = weapon2;
                weapon2 = weapon1;
                weapon1 = "Shotty";

                Instantiate(shotty, this.transform);
                currentWeapon = (int)Weapon.Shotty;
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

        if(normalTLB < hitLeeway || normalTNB < hitLeeway)
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
        if(beatCounter == 4 && beatChange == true)
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
        songAudioSource.Play();
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

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            // timescale will stay at zero so I'm turning it back to 1
        }

    }

    void AdjustHealth()
    {
        switch (hp)
        {
            case 3:
                GameObject.Find("Heart3").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart3Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart2").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart2Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart1").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart1Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                break;

            case 2:
                GameObject.Find("Heart3").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart3Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                GameObject.Find("Heart2").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart2Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart1").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart1Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                break;

            case 1:
                GameObject.Find("Heart3").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart3Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                GameObject.Find("Heart2").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                GameObject.Find("Heart2Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.75f);
                GameObject.Find("Heart1").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                GameObject.Find("Heart1Gone").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                break;
            
            case 0:
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
                    if (OnOffTracker)
                    {
                        obj.GetComponent<SpriteRenderer>().color = new Color(0.612f, 0, 0, 1);
                        obj.GetComponent<BoxCollider2D>().enabled = true;
                    }
                    else
                    {
                        obj.GetComponent<SpriteRenderer>().color = new Color(0.612f, 0, 0, 0.5f);
                        obj.GetComponent<BoxCollider2D>().enabled = false;
                    }
                    
                }

                if (obj.name == "BeatOuchOff")
                {
                    if (OnOffTracker)
                    {
                        obj.GetComponent<SpriteRenderer>().color = new Color(0.612f, 0, 0, 0.5f);
                        obj.GetComponent<BoxCollider2D>().enabled = false;
                    }
                    else
                    {
                        obj.GetComponent<SpriteRenderer>().color = new Color(0.612f, 0, 0, 1);
                        obj.GetComponent<BoxCollider2D>().enabled = true;
                    }
                    
                }
            }

            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Obstacle"))
            {
                if (obj.name == "BeatObstacleOn")
                {
                    if (OnOffTracker)
                    {
                        obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                        obj.GetComponent<BoxCollider2D>().enabled = true;
                    }
                    else
                    {
                        obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                        obj.GetComponent<BoxCollider2D>().enabled = false;
                    }
                    
                }

                if (obj.name == "BeatObstacleOff")
                {
                    if (OnOffTracker)
                    {
                        obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                        obj.GetComponent<BoxCollider2D>().enabled = false;
                    }
                    else
                    {
                        obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                        obj.GetComponent<BoxCollider2D>().enabled = true;
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
