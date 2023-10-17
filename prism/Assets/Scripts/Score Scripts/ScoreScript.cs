using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreScript : MonoBehaviour
{

    public static int scoreValue = 0;
    private TextMeshProUGUI scoreCounterText;

    private void Awake()
    {
        PlayerBulletBehavior.EnemyKilled += RunCo;
        scoreCounterText = GetComponent<TextMeshProUGUI>();
    }

    public void RunCo()
    {
        StartCoroutine(Pulse());
    }

    private IEnumerator Pulse()
    {
        /* for (float i = 1f; i <= 1.2f; i+=0.05f)
        {
            scoreCounterText.rectTransform.localScale = new Vector3(i, i, i);
            yield return new WaitForEndOfFrame();
        }
        scoreCounterText.rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        */
        scoreValue += 25;
        /*
        for (float i = 1.2f; i >= 1f; i -= 0.05f)
        {
            scoreCounterText.rectTransform.localScale = new Vector3(i, i, i);
            yield return new WaitForEndOfFrame();
        }
        scoreCounterText.rectTransform.localScale = new Vector3(1f, 1f, 1f);
        */
        yield return new WaitForEndOfFrame();
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        scoreCounterText.text = scoreValue.ToString();
    }

    private void OnDestroy()
    {
        PlayerBulletBehavior.EnemyKilled -= RunCo;
    }
}
