using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public fList Func;
    public void Push()
    {
        Func.ChangeScene("BuildScene");
    }
}
