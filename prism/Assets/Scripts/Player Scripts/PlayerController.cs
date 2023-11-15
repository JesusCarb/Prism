using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
// using System.Numerics;
// using UnityEditor.Experimental.GraphView;
// using System.Numerics;
using UnityEngine.SceneManagement;
using System;
using UnityEngine;
using UnityEditor.Callbacks;
using JetBrains.Annotations;

public class PlayerController : MonoBehaviour
{
    // player vars
    public int hp = 3;
    public float moveSpeed = 40f;
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

    // percentage betwween beats to hit
    public float hitLeeway = .25f;
    float beatsPerSecond;
    private float period;

    // audio vars
    public MusicInfo musicInfo;
    public AudioSource audioSource;
    public AudioSource songAudioSource;
    private Animator anim;
    private enum Walk
    {
        Not,
        Forward,
        Backward
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //BPM = musicInfo.BPM;
        onBeat = false;
        CalculateTimings();
        Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
        beatChange = false;
        PlayMusicWrapper();

                // get animator 
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        BeatTracker();
        PlayBeat();
        StartCoroutine(FailureState());
        SetPlayerAnimations();
        //print(hp);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 mouseLoc = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        Vector3 finalPos = mouseLoc - transform.position;
        if(finalPos.x < 0)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            this.transform.localScale = new Vector3(1, 1, 1);

        }

        float targetVelocityX = playerInput.x * moveSpeed;
        float targetVelocityY = playerInput.y * moveSpeed;

        // velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTime);
        // velocity.y = Mathf.SmoothDamp(velocity.y, targetVelocityY, ref velocityYSmoothing, accelerationTime);

        velocity.x = targetVelocityX;
        velocity.y = targetVelocityY;
        Move(velocity);

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
    void SetPlayerAnimations()
    {
        float velocityNorm = (float)Math.Sqrt(velocity.x * velocity.x + velocity.y * velocity.y);
        anim.SetInteger("Direction", (int)this.transform.localScale.x);

        if(velocityNorm == 0)
        {
            anim.SetInteger("Direction", (int)Walk.Not);

        }else if((velocity.x > 0) && this.transform.localScale.x < 0
        || (velocity.x < 0) && this.transform.localScale.x > 0 || velocity.y <0)
        {
            anim.SetInteger("Direction", (int)Walk.Backward);
            
        }else
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
            beatChange = true;
        else    
            beatChange = false;
        
        timeLastBeat = newTimelastBeat;
        timeNextBeat = newTimeNextBeat;

        
        float normalTLB = timeLastBeat/period;
        float normalTNB = timeNextBeat/period;

        //print("timeLastBeat" + normalTLB + " " + onBeat);
        //print("timeNextBeat" + normalTNB + " " + onBeat);

        if(normalTLB < hitLeeway || normalTNB < hitLeeway)
        {
            onBeat = true;
        }
        else{
            onBeat = false;
        }
        // print(onBeat);
    }
    
    private void PlayBeat()
    {
        if(beatChange)
        {
            audioSource.Play();
        }
    }

    private void PlayMusicWrapper()
    {
        // Add 0.45s delay to start of music to match up with beats
        Invoke("PlayMusic", 0.5f);
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

    private IEnumerator FailureState()
    {
        if(hp <= 0)
        {
            // when dead, pause time, wait 1 second, then transition to main menu
            Time.timeScale = 0;

            yield return new WaitForSecondsRealtime(1);
            Time.timeScale = 1;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            // timescale will stay at zero so I'm turning it back to 1
        }

    }
}
