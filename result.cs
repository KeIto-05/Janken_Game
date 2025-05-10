using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class result : MonoBehaviour
{
    float speed = -1;
    void Start()
    {
        StartCoroutine(stop());
    }

  
    void Update()
    {
        transform.Translate(0, speed, 0);
    }
    IEnumerator stop()
    {
        yield return null;
        yield return new WaitForSeconds(10);
        speed = 0;
    }
}
