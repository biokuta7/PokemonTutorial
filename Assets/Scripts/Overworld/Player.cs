using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public new string name;

    public static Player instance;

    private void Awake()
    {
        instance = this;
    }



}
