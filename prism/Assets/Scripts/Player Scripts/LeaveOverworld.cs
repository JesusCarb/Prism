using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveOverworld : MonoBehaviour
{
    public float transitionSpeed;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) <= 1.85)
        {
            player.GetComponent<PlayerController>().moveSpeed = 0f;

            if (player.transform.position.x < gameObject.transform.position.x)
            {
                player.transform.position += transitionSpeed / 1000f *
                    (Vector3.Normalize(gameObject.transform.position - player.transform.position));

                // Start fade to black
                GameObject.FindGameObjectWithTag("Fade").GetComponent<FadeToBlack>().startFade = true;
            }
            else if (player.transform.position.x != gameObject.transform.position.x)
            {
                player.transform.position = gameObject.transform.position;
            }
            else
            {
                GameObject.FindGameObjectWithTag("Fade").GetComponent<FadeToBlack>().finished = true;
                {
                    SceneManager.LoadScene("TransitionScene");
                }
            }
        }
    }
}
