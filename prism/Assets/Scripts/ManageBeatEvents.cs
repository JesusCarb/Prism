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

        curMeasureTime += Time.deltaTime;
        if (curMeasureTime >= singleMeasureTime / 16f * (curSixteenth + 1))
        {
            pushSixteenth();

            if (curSixteenth % 2 == 0)
            {
                pushEighth();

                //if (curSixteenth)
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

    }

    void pushQuarter()
    {

    }

    void pushHalf()
    {

    }

    void pushWhole()
    {

    }

    void pushHalfMeasure()
    {

    }

    void pushMeasure()
    {

    }

    // Event based function; when song changed, reset Update
}
