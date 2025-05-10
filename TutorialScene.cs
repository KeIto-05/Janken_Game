using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public fList Func;

    public GameObject[] panel = new GameObject[4];
    public int x;
   
    void Start()
    {
    }

 
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if(x > 3)
            {
                Func.ChangeScene("TitleScene");
            }
            else
            {
                panel[x].SetActive(false);
                x += 1;
            }
        }
    }
}
