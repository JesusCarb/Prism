using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField]
    public Room roomOfDoor;
    public int direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        print("door entered");
        if (other.tag == "Player")
        {
            if (direction == 0)
            {
                if (roomOfDoor.roomDoors[0].leadsTo != null)
                    CameraController.instance.currRoom = roomOfDoor.roomDoors[0].leadsTo;
                other.transform.position = new Vector2(other.transform.position.x + 30, other.transform.position.y);
            } 
            else if (direction == 1)
            {
                if (roomOfDoor.roomDoors[1].leadsTo != null)
                    CameraController.instance.currRoom = roomOfDoor.roomDoors[1].leadsTo;
                other.transform.position = new Vector2(other.transform.position.x - 30, other.transform.position.y);
            }
            else if (direction == 2)
            {
                if (roomOfDoor.roomDoors[2].leadsTo != null)
                    CameraController.instance.currRoom = roomOfDoor.roomDoors[2].leadsTo;
                other.transform.position = new Vector2(other.transform.position.x, other.transform.position.y + 22);
            }
            else
            {
                if (roomOfDoor.roomDoors[3].leadsTo != null)
                    CameraController.instance.currRoom = roomOfDoor.roomDoors[3].leadsTo;
                other.transform.position = new Vector2(other.transform.position.x, other.transform.position.y - 22);
            }
        }
    }
}
