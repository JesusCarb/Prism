using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
// using System.Numerics;
// using UnityEditor.Experimental.GraphView;
// using System.Numerics;

using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public MusicInfo musicInfo;

    public float moveSpeed = 5f;

    Vector2 playerInput;

    Vector3 velocity;

    float velocityXSmoothing;

    float velocityYSmoothing;

    float accelerationTime = .1f;

    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotspot = Vector2.zero;


    private bool onBeat;

    private float timeLastBeat;

    private float timeNextBeat;

    // percentage betwween beats to hit
    public float hitLeeway = .25f;

    float beatsPerSecond;
    float period;

    public AudioSource audioSource;
    public AudioSource songAudioSource;

    bool beatChange;


    float BPM;
    // Start is called before the first frame update
    void Start()
    {
        BPM = musicInfo.BPM;
        BPM = 100f;
        onBeat = false;
        CalculateTimings();
        Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
        beatChange = false;
        PlayMusicWrapper();
    }

    void Update()
    {
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        BeatTracker();
        PlayBeat();
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        float targetVelocityX = playerInput.x * moveSpeed;
        float targetVelocityY = playerInput.y * moveSpeed;

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTime);
        velocity.y = Mathf.SmoothDamp(velocity.y, targetVelocityY, ref velocityYSmoothing, accelerationTime);

        Move(velocity);

    }

    public void Move(Vector3 velocity)
    {
        transform.Translate(velocity);
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

        print("timeLastBeat" + normalTLB + " " + onBeat);
        print("timeNextBeat" + normalTNB + " " + onBeat);

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
}
