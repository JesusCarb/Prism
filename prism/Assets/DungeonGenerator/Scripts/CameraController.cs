using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Room currRoom;
    public float transitionSpeed;
    private bool startingChange = false;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSwitchingScene() == false)
        {
            if (Input.GetKeyDown("h"))
            {
                if (currRoom.roomDoors[0].leadsTo != null)
                {
                    currRoom = currRoom.roomDoors[0].leadsTo;
                }
            }
            if (Input.GetKeyDown("f"))
            {
                if (currRoom.roomDoors[1].leadsTo != null)
                {
                    currRoom = currRoom.roomDoors[1].leadsTo;
                }
            }
            if (Input.GetKeyDown("t"))
            {
                if (currRoom.roomDoors[2].leadsTo != null)
                {
                    currRoom = currRoom.roomDoors[2].leadsTo;
                }
            }
            if (Input.GetKeyDown("g"))
            {
                if (currRoom.roomDoors[3].leadsTo != null)
                {
                    currRoom = currRoom.roomDoors[3].leadsTo;
                }
            }
        }
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if (currRoom == null)
        {
            return;
        }

        Vector3 targetPos = GetCameraTargetPosition();

        if (transform.position != targetPos && startingChange)
        {
            gameObject.GetComponent<Camera>().DOColor(currRoom.transform.Find("Body").GetComponent<SpriteRenderer>().color, 0.5f).SetEase(Ease.Linear);

            startingChange = false;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * transitionSpeed);
    }

    Vector3 GetCameraTargetPosition()
    {
        if (currRoom == null)
        {
            return Vector3.zero;
        }

        //Try to get the center of the room here
        Vector3 targetPos = currRoom.transform.position;
        targetPos.z = transform.position.z;

        return targetPos;
    }

    public bool IsSwitchingScene()
    {
        if (!startingChange)
        {
            startingChange = true;
        }

        return transform.position.Equals(GetCameraTargetPosition()) == false;
    }
}
