using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CorpseBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Despawn());
    }

    // Update is called once per frame
    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(5f);

        Destroy(gameObject);

    }
}
