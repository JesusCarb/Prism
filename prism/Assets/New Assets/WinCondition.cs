using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WinCondition : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerC;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerC = player.GetComponent<PlayerController>();
    }

    IEnumerator OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            yield return new WaitForSecondsRealtime(.05f);

            print("youwin");

            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(2);

            Time.timeScale = 1;

            // thiswasnt working L
            // playerC.audioSource.Stop();
            playerC.PlayEndingMusic();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            // Destroy(gameObject);
        }
    }
}
