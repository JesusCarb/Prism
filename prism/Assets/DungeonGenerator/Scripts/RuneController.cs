using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneController : MonoBehaviour
{

    [SerializeField]
    public int runeNum;
    public bool active;

    // Start is called before the first frame update
    void Start()
    {
        runeNum = Random.Range(0, 9);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
