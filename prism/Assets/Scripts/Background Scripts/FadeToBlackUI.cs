using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToBlackUI : MonoBehaviour
{
    public float speed = 4f;
    public bool startFade = false;
    public bool finished = false;
    private float curAlpha = 0f;
    private float initialMusicVol = 1f;

    // Start is called before the first frame update

    void Start()
    {   
        speed /= 1000f;
        initialMusicVol = GameObject.Find("Music Audio Source").GetComponent<AudioSource>().volume;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startFade)
            return;
        
        //print("FADING");

        if (gameObject.GetComponent<UnityEngine.UI.Image>().color.a + speed > 1)
        {
            gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(r: 0, g: 0, b: 0, a: 1);
            finished = true;
            return;
        }
        else
        {
            // Increase opacity
            curAlpha = speed + gameObject.GetComponent<UnityEngine.UI.Image>().color.a;
            gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, curAlpha);

            // Decrease volume
            GameObject.Find("Music Audio Source").GetComponent<AudioSource>().volume = initialMusicVol * (1 - curAlpha);
        }

        
    }
}
