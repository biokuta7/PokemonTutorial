using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PokemonTrainer
{
    public static Player instance;

    private void Awake()
    {
        instance = this;
    }



}
