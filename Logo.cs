using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo : MonoBehaviour
{
    float speed = 0.5f;
    public int on;

    void Update()
    {
        transform.Translate(0, speed, 0);
        on = (on + 1) % 60;
        if(on == 0)
        {
            speed *= -1;
        }
    }

    
}
