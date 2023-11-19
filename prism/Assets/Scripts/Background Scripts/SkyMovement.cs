using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyMovement : MonoBehaviour
{
    public float speed;
    public float period;
    // Start is called before the first frame update
    void Start()
    {
        speed *= .0005f;
    }

    // Update is called once per frame
    void Update()
    {
        float curTime = Time.time % period;

        // Scroll right
        if (curTime <= period / 16)
        {
            gameObject.transform.position += new Vector3(speed / 4, 0f, 0f);
        }
        else if (curTime <= 2 * period / 16)
        {
            gameObject.transform.position += new Vector3(speed / 2, 0f, 0f);
        }
        else if (curTime <= 6 * period / 16)
        {
            gameObject.transform.position += new Vector3(speed, 0f, 0f);
        }
        else if (curTime <= 7 * period / 16)
        {
            gameObject.transform.position += new Vector3(speed / 2, 0f, 0f);
        }
        else if (curTime <= 8 * period / 16)
        {
            gameObject.transform.position += new Vector3(speed / 4, 0f, 0f);
        }

        // Scroll left
        else if (curTime <= 9 * period / 16)
        {
            gameObject.transform.position += new Vector3(-1 * speed / 4, 0f, 0f);
        }
        else if (curTime <= 10 * period / 16)
        {
            gameObject.transform.position += new Vector3(-1 * speed / 2, 0f, 0f);
        }
        else if (curTime <= 14 * period / 16)
        {
            gameObject.transform.position += new Vector3(-1 * speed, 0f, 0f);
        }
        else if (curTime <= 15 * period / 16)
        {
            gameObject.transform.position += new Vector3(-1 * speed / 2, 0f, 0f);
        }
        else
        {
            gameObject.transform.position += new Vector3(-1 * speed / 4, 0f, 0f);
        }
    }
}
