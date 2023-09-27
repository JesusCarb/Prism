using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageBeatEvents : MonoBehaviour
{
    public MusicInfo musicInfo;
    private int BPM;
    private int timeSignature0;
    private int timeSignature1;

    private float singleMeasureTime;
    private float curMeasureTime = 0f;
    private int curSixteenth = 0;
    private int maxSixteenth;

    private bool lastQuarterWasHalf = true;
    private bool lastWhole = false;



    // Start is called before the first frame update
    void Start()
    {
        BPM = musicInfo.BPM;
        timeSignature0 = musicInfo.timeSignature[0];
        timeSignature1 = musicInfo.timeSignature[1];
        singleMeasureTime = 60f / BPM * timeSignature0;
        maxSixteenth = 4 * timeSignature0;
    }

    // Update is called once per frame
    void Update()
    {
        // TODO- decide how it should work with and update to work with 3:4 time signature   

        curMeasureTime += Time.deltaTime;
        if (curMeasureTime >= singleMeasureTime / 16f * (curSixteenth + 1))
        {
            pushSixteenth();

            if (curSixteenth % 2 == 0)
            {
                pushEighth();

                if (curSixteenth % 4 == 0)
                {
                    pushQuarter();

                    if (curSixteenth % 8 == 0)
                    {
                        pushHalf();

                        if (curSixteenth == 0)
                        {
                            pushWhole();

                            if (!lastWhole)
                                pushTwoWhole();

                            lastWhole = !lastWhole;
                        }
                    }
                }
            }



            if (curSixteenth + 1 == 16)
            {
                curSixteenth = -1;
                curMeasureTime = curMeasureTime - singleMeasureTime;
            }


            curSixteenth++;
        }
    }

    void UpdateMusicChange()
    {

    }

    void pushSixteenth()
    {
        
    }

    void pushEighth()
    {
        print("8th");
    }

    void pushQuarter()
    {
        print("4th");
    }

    void pushHalf()
    {
        print("Half");
    }

    void pushWhole()
    {
        print("Whole");
    }

    void pushTwoWhole()
    {
        print("Two whole");
    }

    // Event based function; when song changed, reset Update
}
