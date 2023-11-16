using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    public enum doorType
    {
        up = 0,
        down = 1,
        left = 2,
        right = 3
    }
    public int doorTypeNum;

    public struct Door
    {
        public bool active;
        public bool locked;
        public Door leadsTo;
        public WalkableRoom leadsToRoom; 
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
