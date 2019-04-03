using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSettings : MonoBehaviour
{

    public static MapSettings instance;

    [System.Serializable]
    public class Encounter
    {
        public PokemonData pokemonData;
        public int minLevel = 30;
        public int maxLevel = 30;
        public int percentageChance = 50;
    }

    public Encounter[] grassEncounters;
    public Encounter[] waterEncounters;
    public Encounter[] randomEncounters;

    public bool encountersOn = true;

    [Range(0f, .20f)]
    public float grassEncounterChance = .15f;
    [Range(0f, .20f)]
    public float waterEncounterChance = .10f;
    [Range(0f, .20f)]
    public float randomEncounterChance = .05f;

    private void Awake()
    {
        instance = this;
    }



    public bool TryGrassEncounter()
    {
        return TryEncounter(grassEncounters, grassEncounterChance);
    }

    public bool TryWaterEncounter()
    {
        return TryEncounter(waterEncounters, waterEncounterChance);
    }

    public bool TryRandomEncounter()
    {
        return TryEncounter(randomEncounters, randomEncounterChance);
    }

    private bool TryEncounter(Encounter[] encounters, float encounterChance)
    {

        if(encounters.Length <= 0) { return false; }

        if (!encountersOn) { return false; }

        if (Random.Range(0f, 1f) > encounterChance)
        {
            return false;
        }

        int r = Random.Range(0, 100);

        foreach (Encounter e in encounters)
        {
            if (r > e.percentageChance)
            {
                r -= e.percentageChance;
                continue;
            }
            else
            {
                int level = Random.Range(e.minLevel, e.maxLevel);
                Debug.Log("A wild " + e.pokemonData.name + " appeared! LVL " + level);
                PokemonGameManager.instance.StartWildBattle(e.pokemonData, level);
                return true;
            }
        }

        return false;

    }

}
