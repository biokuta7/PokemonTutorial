using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PokemonTypeData : ScriptableObject {

    public new string name;
    public string internalName;
    public bool isSpecial;
    public List<PokemonTypeData> weaknesses;
    public List<PokemonTypeData> immunities;
    public List<PokemonTypeData> resistances;
    public Color color;
    
    public void Init()
    {
        weaknesses = new List<PokemonTypeData>();
        immunities = new List<PokemonTypeData>();
        resistances = new List<PokemonTypeData>();
    }

    public bool Equals(PokemonTypeData other)
    {
        return internalName.Equals(other.internalName);
    }

    public static float Effectiveness(PokemonTypeData attackingType, Pokemon defendingPokemon)
    {
        return Effectiveness(attackingType, defendingPokemon.pokemonData);
    }

    public static float Effectiveness(PokemonTypeData attackingType, PokemonData defendingPokemon)
    {
        return Effectiveness(attackingType, defendingPokemon.type1) * Effectiveness(attackingType, defendingPokemon.type2);
    }

    public static float Effectiveness(PokemonTypeData attackingType, PokemonTypeData defendingType)
    {
        if(defendingType.weaknesses.Contains(attackingType))
        {
            return 2.0f;
        } else if(defendingType.immunities.Contains(attackingType))
        {
            return 0.0f;
        } else if(defendingType.resistances.Contains(attackingType))
        {
            return .5f;
        }

        return 1.0f;
    }

}
