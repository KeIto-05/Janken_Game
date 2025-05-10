using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScene : MonoBehaviour
{
    public GameObject message;
    public fList Func;

    void Start()
    {
        message.GetComponent<Text>().text = BattleScene.winner +  "の勝ち！！！！！";
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            Func.ChangeScene("TitleScene");
        }    
    }
}
