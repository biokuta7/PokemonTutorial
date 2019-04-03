using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PokemonParty
{

    public Pokemon[] pokemons;

    public Pokemon GetFirstNonFaintedPokemon()
    {
        foreach (Pokemon p in pokemons)
        {
            if (p != null && !p.status.Equals(Status.FAINTED))
            {
                return p;
            }
        }

        return null;
    }

    public void InitParty()
    {
        foreach(Pokemon p in pokemons)
        {
            p.InitPokemon();
        }
    }

}

public class PokemonTrainer : MonoBehaviour
{

    public new string name;

    public PokemonParty party;

    private void Start()
    {
        party.InitParty();
    }

}