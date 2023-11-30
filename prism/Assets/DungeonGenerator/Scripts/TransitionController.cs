using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SendToDungeon());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SendToDungeon()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("DungeonGen");
    }
}
