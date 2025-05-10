using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BuildScene : MonoBehaviour
{
    [SerializeField] GameObject[] BGI = new GameObject[2];
    [SerializeField] GameObject[] card = new GameObject[3];
    public GameObject message;

    public fList Func;
    public static int[,] selectsp = new int[,] { {1,1,1 }, { 1,1,1} };
    public string[] player = new string[2] { "Red", "Green" };
    public string[] type = new string[3] { "グー", "チョキ", "パー" };
    

    void Start()
    {
        for(int i = 0; i < 2; i++)
        {
            BGI[i].SetActive(false);  
        }
        for (int j = 0; j < 3; j++)
        {
            card[j].SetActive(false);
        }
        StartCoroutine(Build());
    }



    IEnumerator Build()
    {
        yield return null;
        for(int i = 0; i < 2; i++)
        {
            BGI[i].SetActive(true);
            for(int j = 0; j < 3; j++)
            {
                card[j].SetActive(true);
                while (true)
                {
                    message.GetComponent<Text>().text = player[i] + "のターン：" + type[j] + "のスペシャルを選択してください";
                    yield return new WaitUntil(() => Input.anyKeyDown);
                    selectsp[i, j] = Func.GetCommand();
                    if (selectsp[i, j] < 5)
                    {
                        message.GetComponent<Text>().text = "OK";
                        GetComponent<AudioSource>().Play();
                        yield return new WaitForSeconds(1.0f);
                        break;
                    }
                    else
                    {
                        message.GetComponent<Text>().text = "未対応の入力です";
                        yield return new WaitForSeconds(2.0f);
                    }
                }
                card[j].SetActive(false);
            }
            BGI[i].SetActive(false);
        }
        Func.ChangeScene("BattleScene");
    }
}
