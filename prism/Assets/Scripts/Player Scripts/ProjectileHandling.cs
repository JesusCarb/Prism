using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileHandling : MonoBehaviour
{

    ManageBeatEvents beatManager;

    PlayerController playerController;
    public GameObject basicBullet;

    public GameObject enemyBullet;

    private GameObject player;

    public AudioClip pistolShot;
    public AudioClip rifleShot;
    public AudioClip shottyShot;
    private AudioSource shootSource;


    private bool firstFireCurrentBeat = true;
    private float timeUntilNextFire = 0f;
    private float delayFromFire = .25f;

    [SerializeField]
    private int currentWeapon;

    private enum Weapon
    {
        None,
        Pistol,
        Rifle,
        Shotty
    }

    [SerializeField]
    public GameObject blastBullets;

    public GameObject burstBullets;

    public int blastNum;
    public int burstNum;


    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");

        beatManager = gameObject.GetComponent<ManageBeatEvents>();
        playerController = player.GetComponent<PlayerController>();
        pistolShot = playerController.pistolAudio;
        rifleShot = playerController.rifleAudio;
        shottyShot = playerController.shottyAudio;
        shootSource = player.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeUntilNextFire > 0f)
            timeUntilNextFire -= Time.deltaTime;
        if (timeUntilNextFire <= 0f)
            firstFireCurrentBeat = true;


        if (Input.GetButtonDown("Fire1"))
        {
            //print("fire1");
            if(playerController.OnBeat() && firstFireCurrentBeat)
            {
                //print(currentWeapon);
                //print((int)Weapon.Shotty);
                if(currentWeapon == (int)Weapon.Pistol)
                {
                    FireProjectile();
                }else if(currentWeapon == (int)Weapon.Rifle)
                {
                    StartCoroutine(FireBurst());
                }
                else if(currentWeapon == (int)Weapon.Shotty)
                {
                    BlastBullets();
                }

                Debug.Log("Beat Hit");
                firstFireCurrentBeat = false;
                // timeUntilNextFire = delayFromFire;
                timeUntilNextFire = delayFromFire * playerController.GetPeriod() * 2;
            }
            else
            {
                Debug.Log("Beat Miss");
            }
            //if (!beatManager.onQuarter.contains(FireProjectile()))
            //{
            //    beatManager.onQuarter += FireProjectile();
            //}
            
        }
    }

    void FireProjectile()
    {
        // gets position of player to spawn bullet

        Vector3 pos = this.transform.position;
        Vector3 mouseLoc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 final = mouseLoc - pos;

        float angle = Mathf.Atan2(final.y, final.x) * Mathf.Rad2Deg;

        Quaternion rot = Quaternion.Euler(new Vector3(0,0, angle - 90));

        // currently spawning on player position
        Instantiate(basicBullet, position: pos, rotation: rot);
        shootSource.PlayOneShot(pistolShot);
        // Debug.Log("FIRE");
    }

    private IEnumerator FireBurst()
    {
        for(int i = 0; i < burstNum; i ++)
        {
            yield return new WaitForSeconds(.075f);
                // gets position of player to spawn bullet

            Vector3 pos = this.transform.position;
            Vector3 mouseLoc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 final = mouseLoc - pos;

            float angle = Mathf.Atan2(final.y, final.x) * Mathf.Rad2Deg;

            Quaternion rot = Quaternion.Euler(new Vector3(0,0, angle - 90));

            // currently spawning on player position
            Instantiate(burstBullets, position: pos, rotation: rot);
            shootSource.PlayOneShot(rifleShot);
            // Debug.Log("FIRE");
        }
        
    }

    void BlastBullets()
    {
        Vector3 loc = this.transform.position;
        Quaternion rot = this.transform.rotation;
        for(int i = 0; i < blastNum; i++)
        {
            Instantiate(blastBullets, position: loc, rotation: rot);
        }
        shootSource.PlayOneShot(shottyShot);
    }


}
