using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoButton : MonoBehaviour
{
    public fList Func;
    public void Push()
    {
        Func.ChangeScene("TutorialScene");
    }
}
