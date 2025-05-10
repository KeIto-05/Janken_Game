using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class fList : MonoBehaviour
{
    public void ChangeScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }




    public  int GetCommand()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || (Input.GetKeyDown(KeyCode.Return)))
        {
            return 8;
        }
        else
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    return 999;
                }
                foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(code))
                    {
                        if (code == KeyCode.Alpha1 || code == KeyCode.Keypad1)
                        {
                            return 1;
                        }
                        else if (code == KeyCode.Alpha2 || code == KeyCode.Keypad2)
                        {
                            return 2;
                        }
                        else if (code == KeyCode.Alpha3 || code == KeyCode.Keypad3)
                        {
                            return 3;
                        }
                        else if (code == KeyCode.Alpha4 || code == KeyCode.Keypad4)
                        {
                            return 4;
                        }
                        else
                        {
                            return 999;
                        }
                    }
                }
            }
            return 999;
        }
    }

    /*コピペ用
    while (true)
    {
        Debug.Log("コマンドを入力してください");
        yield return new WaitUntil(() => Input.anyKeyDown);
         xxx = Func.GetCommand();
        if(xxx < 5)
        {
            yield return new WaitForSeconds(1.0f);
            break;
        }
        else
        {
            Debug.Log("未対応の入力です");
            yield return new WaitForSeconds(2.0f);
        }
    }
     */



}
