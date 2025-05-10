using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public new string name;
    public int type;
    public int hp = 100;
    public int sp;
    public int[,] splist = new[,] { { 1,2,5,7},
                                    { 1,3,5,6},
                                    { 1,4,6,7}
                                  };
}