
using System.Collections;
using UnityEngine;

public enum Status
{
    NONE,
    SLEEP,
    POISON,
    BURN,
    PARALYSIS,
    FREEZE,
    FAINTED
}

public enum Gender
{
    NONE,
    MALE,
    FEMALE
}

public enum Nature
{
    HARDY, 
    LONELY,
    BRAVE, 
    ADAMANT,
    NAUGHTY,
    BOLD,
    DOCILE,
    RELAXED,
    IMPISH,
    LAX,
    TIMID, 
    HASTY,
    SERIOUS,
    JOLLY, 
    NAIVE, 
    MODEST,
    MILD,
    QUIET, 
    BASHFUL,
    RASH,
    CALM,
    GENTLE,
    SASSY, 
    CAREFUL,
    QUIRKY
}

[System.Serializable]
public class MoveInSet
{
    public MoveData move;
    public int PP;

    public MoveInSet(MoveData _move)
    {
        move = _move;
        PP = move.totalPP;
    }

}

[System.Serializable]
public class NatureData
{
    public string name;
    public float ATK, DEF, SPE, SPA, SPD;

    public NatureData(string _name, float _ATK, float _DEF, float _SPA, float _SPD, float _SPE)
    {
        name = _name;
        ATK = _ATK;
        DEF = _DEF;
        SPA = _SPA;
        SPD = _SPD;
        SPE = _SPE;
    }

    public static NatureData[] natures = new NatureData[]
    {
        new NatureData("HARDY", 1, 1, 1, 1, 1),
        new NatureData("LONELY", 1.1f, 0.9f, 1, 1, 1),
        new NatureData("BRAVE", 1.1f, 1, 1, 1, 0.9f),
        new NatureData("ADAMANT", 1.1f, 1, 0.9f, 1, 1),
        new NatureData("NAUGHTY", 1.1f, 1, 1, 0.9f, 1),
        new NatureData("BOLD", 0.9f, 1.1f, 1, 1, 1),
        new NatureData("DOCILE", 1, 1, 1, 1, 1),
        new NatureData("RELAXED", 1, 1.1f, 1, 1, 0.9f),
        new NatureData("IMPISH", 1, 1.1f, 0.9f, 1, 1),
        new NatureData("LAX", 1, 1.1f, 1, 0.9f, 1),
        new NatureData("TIMID", 0.9f, 1, 1, 1, 1.1f),
        new NatureData("HASTY", 1, 0.9f, 1, 1, 1.1f),
        new NatureData("SERIOUS", 1, 1, 1, 1, 1),
        new NatureData("JOLLY", 1, 1, 0.9f, 1, 1.1f),
        new NatureData("NAIVE", 1, 1, 1, 0.9f, 1.1f),
        new NatureData("MODEST", 0.9f, 1, 1.1f, 1, 1),
        new NatureData("MILD", 1, 0.9f, 1.1f, 1, 1),
        new NatureData("QUIET", 1, 1, 1.1f, 1, 0.9f),
        new NatureData("BASHFUL", 1, 1, 1, 1, 1),
        new NatureData("RASH", 1, 1, 1.1f, 0.9f, 1),
        new NatureData("CALM", 0.9f, 1, 1, 1.1f, 1),
        new NatureData("GENTLE", 1, 0.9f, 1, 1.1f, 1),
        new NatureData("SASSY", 1, 1, 1, 1.1f, 0.9f),
        new NatureData("CAREFUL", 1, 1, 0.9f, 1.1f, 1),
        new NatureData("QUIRKY", 1, 1, 1, 1, 1)
    };

}

[System.Serializable]
public class Pokemon : MonoBehaviour {
    
    public PokemonData pokemonData;

    public string nickname;
    //public int form;
    public Gender gender;
    public int level;
    public int XP;
    private int nextLevelXP;
    private int previousLevelXP;

    public int happiness;

    public bool pokerus;
    public bool shiny;

    public Status status;
    public int sleepTurns;

    public ItemData caughtBall;
    public ItemData heldItem;

    public string metDate;
    public string metMap;
    public int metLevel;

    public string OT;
    public int ID;

    public static int count;

    public int IV_HP, IV_Attack, IV_Defense, IV_Speed, IV_SpecialAttack, IV_SpecialDefense;
    public int EV_HP, EV_Attack, EV_Defense, EV_Speed, EV_SpecialAttack, EV_SpecialDefense;

    public Nature nature;

    public int HP;
    public int currentHP;
    public int attack;
    public int defense;
    public int speed;
    public int specialAttack;
    public int specialDefense;

    public AbilityData ability;

    public MoveInSet[] moveset = new MoveInSet[4];

    public static int SHINY_CHANCE = 8192;
    public static int POKERUS_CHANCE = 21845;
    
    /// <summary>
    /// General initiator for Pokemon. All details. Use for special cases.
    /// </summary>
    public void InitPokemon(PokemonData _pokemonData, string _nickname, Gender _gender, int _level, 
        bool _shiny, ItemData _caughtBall, ItemData _heldItem, string _OT,
        int _IV_HP, int _IV_Attack, int _IV_Defense, int _IV_Speed, int _IV_SpecialAttack, int _IV_SpecialDefense,
        int _EV_HP, int _EV_Attack, int _EV_Defense, int _EV_Speed, int _EV_SpecialAttack, int _EV_SpecialDefense,
        Nature _nature, AbilityData _ability, MoveData[] _moveset, Status _status, int _sleepTurns)
    {
        ID = count;
        count++;

        pokemonData = _pokemonData;
        nickname = _nickname;
        gender = _gender;
        shiny = _shiny;
        caughtBall = _caughtBall;
        heldItem = _heldItem;
        OT = _OT;

        IV_HP = _IV_HP;
        IV_Attack = _IV_Attack;
        IV_Defense = _IV_Defense;
        IV_Speed = _IV_Speed;
        IV_SpecialAttack = _IV_SpecialAttack;
        IV_SpecialDefense = _IV_SpecialDefense;

        EV_HP = _EV_HP;
        EV_Attack = _EV_Attack;
        EV_Defense = _EV_Defense;
        EV_Speed = _EV_Speed;
        EV_SpecialAttack = _EV_SpecialAttack;
        EV_SpecialDefense = _EV_SpecialDefense;

        nature = _nature;
        ability = _ability;

        level = _level;
        XP = PokemonData.GetLevelXP(pokemonData.growthRate, level);
        UpdateStats();
        currentHP = HP;
        happiness = pokemonData.baseHappiness;

        status = _status;
        sleepTurns = _sleepTurns;

        metLevel = level;
        metMap = "Somewhere";

        metDate = System.DateTime.Today.Month + "/" + System.DateTime.Today.Day + "/" + System.DateTime.Today.Year;

        for (int i = 0; i < 4; i++)
        {
            if (_moveset[i] != null)
            {
                moveset[i] = new MoveInSet(_moveset[i]);
            } else { moveset[i] = null; }
        }

    }

    /// <summary>
    /// Common initiator for Pokemon. Necessary details, rest to random generation. Use for wild pokemon and trainers.
    /// </summary>
    public void InitPokemon(PokemonData _pokemonData, string _nickname, int _level, ItemData _caughtBall, string _OT)
    {

        int[] pIV = new int[6] {
            Random.Range(0,32),
            Random.Range(0,32),
            Random.Range(0,32),
            Random.Range(0,32),
            Random.Range(0,32),
            Random.Range(0,32)
        };

        bool pShiny = (Random.Range(0, SHINY_CHANCE)) == 1;

        Gender pGender;
        if(_pokemonData.femaleRatio >= 0)
        {
            pGender = (Random.Range(0f, 1.0f) < _pokemonData.femaleRatio ? Gender.FEMALE : Gender.MALE);
        } else { pGender = Gender.NONE; }

        Nature pNature = (Nature)Random.Range(0, NatureData.natures.Length);
        AbilityData pAbility = _pokemonData.standardAbilities[Random.Range(0, _pokemonData.standardAbilities.Count)];
        MoveData[] pMoveset = _pokemonData.GetBestMovesAtLevel(_level);

        InitPokemon(_pokemonData, _nickname, pGender, _level, 
            pShiny, _caughtBall, null, _OT, 
            pIV[0], pIV[1], pIV[2], pIV[3], pIV[4], pIV[5], 
            0, 0, 0, 0, 0, 0, 
            pNature, pAbility, pMoveset, Status.NONE, 0);
    }

    /// <summary>
    /// Default initatior.
    /// </summary>
    public void InitPokemon()
    {
        InitPokemon(pokemonData, nickname, level, caughtBall, OT);
    }

    public void UpdateStats()
    {
        previousLevelXP = PokemonData.GetLevelXP(pokemonData.growthRate, level);
        nextLevelXP = PokemonData.GetLevelXP(pokemonData.growthRate, level + 1);
        //HP - Stat = floor((2 * B + I + E) * L / 100 + L + 10)

        float hpRatio = GetHPRatio();

        HP = Mathf.FloorToInt((2f * pokemonData.baseHP + IV_HP + EV_HP) * (level / 100f) + level + 10);

        currentHP = Mathf.FloorToInt(HP * hpRatio);

        //OTHERS - Stat = floor(floor((2 * B + I + E) * L / 100 + 5) * N)
        attack = Mathf.FloorToInt(Mathf.Floor((2f * pokemonData.baseAttack + IV_Attack + EV_Attack) * (level / 100f) + 5) * NatureData.natures[(int)nature].ATK);
        defense = Mathf.FloorToInt(Mathf.Floor((2f * pokemonData.baseDefense + IV_Defense + EV_Defense) * (level / 100f) + 5) * NatureData.natures[(int)nature].DEF);
        speed = Mathf.FloorToInt(Mathf.Floor((2f * pokemonData.baseSpeed + IV_Speed + EV_Speed) * (level / 100f) + 5) * NatureData.natures[(int)nature].SPE);
        specialAttack = Mathf.FloorToInt(Mathf.Floor((2f * pokemonData.baseSpecialAttack + IV_SpecialAttack + EV_SpecialAttack) * (level / 100f) + 5) * NatureData.natures[(int)nature].SPA);
        specialDefense = Mathf.FloorToInt(Mathf.Floor((2f * pokemonData.baseSpecialDefense + IV_SpecialDefense + EV_SpecialDefense) * (level / 100f) + 5) * NatureData.natures[(int)nature].SPD);
    }

    public bool CheckForDeath()
    {
        if(currentHP <= 0)
        {
            status = Status.FAINTED;
            return true;
        }

        return false;
    }

    public bool CheckForLevelUp()
    {
        int prevL = level;
        while(level < 100 && XP > nextLevelXP)
        {
            level++;
            UpdateStats();
        }

        return (level > prevL);
    }

    public string GetName() { if(nickname != "") { return nickname; } return pokemonData.name; }

    public Sprite GetFrontSprite() { if (shiny) { return pokemonData.shinySprite; } return pokemonData.sprite; }
    public Sprite GetBackSprite() { if (shiny) { return pokemonData.shinyBackSprite; } return pokemonData.backSprite; }

    public float GetHPRatio() { return (float)currentHP / HP; }
    public float GetXPRatio() { return (float)(XP - previousLevelXP) / (nextLevelXP - previousLevelXP); }

    public bool IsType(PokemonTypeData type)
    {
        return (type.Equals(pokemonData.type1) || type.Equals(pokemonData.type2));
    }

    public void ModHP(int amount, bool instant = false)
    {
        if (instant)
            currentHP += amount;
        else
        {
            StartCoroutine(ModHPCoroutine(amount));
        }

        currentHP = Mathf.Clamp(currentHP, 0, HP);

    }

    private IEnumerator ModHPCoroutine(int amount)
    {
        for(int i = 0; i < Mathf.Abs(amount); i++)
        {
            currentHP += (int)Mathf.Sign(amount);

            if(currentHP>=HP || currentHP <= 0)
            {
                break;
            } else
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void ModXP(int amount, bool instant = false)
    {
        amount = Mathf.Abs(amount);
        if (instant)
        {
            XP += amount; CheckForLevelUp();
        }
        else
        {
            StartCoroutine(ModXPCoroutine(amount));
        }
    }

    private IEnumerator ModXPCoroutine(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            XP++;

            if (CheckForLevelUp())
            {
                Debug.Log("LEVELUP!");
            }

            if (i % 100 == 0)
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void AfflictStatus(Status s)
    { status = s; }

    public void AfflictStatus(string str)
    {
        status = (Status)System.Enum.Parse(typeof(Status), str, true);
    }


}
