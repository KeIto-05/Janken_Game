using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TitleScene : MonoBehaviour
{
    public fList Func;
    void Start()
    {

    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            Func.ChangeScene("BuildScene");
        }
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            Func.ChangeScene("TutorialScene");
        }
    }

}
