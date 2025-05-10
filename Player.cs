using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public new string name;
    public int handspoint = 3;
    public Hand[] hands = new Hand[3];
    public int action;
}