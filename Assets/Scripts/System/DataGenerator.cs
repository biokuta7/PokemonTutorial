using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class DataGenerator : MonoBehaviour
{
    public Dictionary<string, PokemonTypeData> pokemonTypeDataDictionary;
    public Dictionary<string, PokemonData> pokemonDataDictionary;
    public Dictionary<string, AbilityData> abilityDataDictionary;
    public Dictionary<string, ItemData> itemDataDictionary;
    public Dictionary<string, MoveData> moveDataDictionary;

    [SerializeField]
    private PokemonTypeData[] pokemonTypeDataSerializationArray;
    [SerializeField]
    private PokemonData[] pokemonDataSerializationArray;
    [SerializeField]
    private AbilityData[] abilityDataSerializationArray;
    [SerializeField]
    private ItemData[] itemDataSerializationArray;
    [SerializeField]
    private MoveData[] moveDataSerializationArray;


    public static DataGenerator instance;

    //MAX 17
    public static int NUMBER_OF_TYPES = 17;
    //MAX 151
    public static int NUMBER_OF_POKEMON = 151;
    //MAX 164
    public static int NUMBER_OF_ABILITIES = 164;
    //MAX 559
    public static int NUMBER_OF_MOVES = 559;
    //MAX 525
    public static int NUMBER_OF_ITEMS = 525;

    private void Awake()
    {
        instance = this;
        InitDictionaries();
    }

    private void Start()
    {
        Debugger();
    }

    private void InitDictionaries()
    {
        pokemonTypeDataDictionary = new Dictionary<string, PokemonTypeData>();
        pokemonDataDictionary = new Dictionary<string, PokemonData>();
        abilityDataDictionary = new Dictionary<string, AbilityData>();
        itemDataDictionary = new Dictionary<string, ItemData>();
        moveDataDictionary = new Dictionary<string, MoveData>();

        foreach (PokemonTypeData pokemonTypeData in pokemonTypeDataSerializationArray)
        { pokemonTypeDataDictionary.Add(pokemonTypeData.internalName, pokemonTypeData); }
        foreach (PokemonData pokemonData in pokemonDataSerializationArray)
        { pokemonDataDictionary.Add(pokemonData.internalName, pokemonData); }
        foreach (AbilityData abilityData in abilityDataSerializationArray)
        { abilityDataDictionary.Add(abilityData.internalName, abilityData); }
        foreach (ItemData itemData in itemDataSerializationArray)
        { itemDataDictionary.Add(itemData.internalName, itemData); }
        foreach (MoveData moveData in moveDataSerializationArray)
        { moveDataDictionary.Add(moveData.internalName, moveData); }
    }

    public void Debugger()
    {

        PokemonData bulbasaur;
        if(pokemonDataDictionary.TryGetValue("BULBASAUR", out bulbasaur))
        {
            Debug.Log(bulbasaur.name);
        } else { Debug.LogError("Couldn't find Bulbasaur!"); }

    }
    
    #region cleaners
    public void ClearPokemonData()
    {
        if (pokemonDataDictionary != null)
        {
            pokemonDataDictionary.Clear();
            pokemonDataDictionary = null;
        }

        pokemonDataSerializationArray = null;

    }
    public void ClearTypeData()
    {
        if (pokemonTypeDataDictionary != null)
        {
            pokemonTypeDataDictionary.Clear();
            pokemonTypeDataDictionary = null;
        }


        pokemonTypeDataSerializationArray = null;

    }
    public void ClearItemData()
    {
        if (itemDataDictionary != null)
        {
            itemDataDictionary.Clear();
            itemDataDictionary = null;
        }


        itemDataSerializationArray = null;

    }
    public void ClearMoveData()
    {
        if (moveDataDictionary != null)
        {
            moveDataDictionary.Clear();
            moveDataDictionary = null;
        }


        moveDataSerializationArray = null;

    }
    public void ClearAbilityData()
    {
        if (abilityDataDictionary != null)
        {
            abilityDataDictionary.Clear();
            abilityDataDictionary = null;
        }


        abilityDataSerializationArray = null;

    }

    #endregion

    #region generators



    public PokemonData[] GeneratePokemon()
    {
        pokemonDataDictionary = new Dictionary<string, PokemonData>();
        IniFileAccessor.SetPath("/Data/Text Files/pokemon.ini");

        for (int i = 1; i <= NUMBER_OF_POKEMON; i++)
        {
            string indexStr = (i).ToString();

            string paddedindexStr = indexStr.ToString().PadLeft(3, '0'); // results in 009

            PokemonData pokemonData = ScriptableObject.CreateInstance<PokemonData>();

            pokemonData.Init();

            //NAME AND BASIC INFO

            pokemonData.number = i;

            pokemonData.name = IniFileAccessor.ReadValue(indexStr, "Name");
            pokemonData.internalName = IniFileAccessor.ReadValue(indexStr, "InternalName");

            pokemonData.kind = IniFileAccessor.ReadValue(indexStr, "Kind");
            pokemonData.dexEntry = IniFileAccessor.ReadValue(indexStr, "Pokedex");

            pokemonData.habitat = (PokemonData.Habitat) Enum.Parse(typeof(PokemonData.Habitat), IniFileAccessor.ReadValue(indexStr, "Habitat").ToUpper());

            //TYPE

            string type1String = IniFileAccessor.ReadValue(indexStr, "Type1");
            string type2String = IniFileAccessor.ReadValue(indexStr, "Type2");
            
            pokemonTypeDataDictionary.TryGetValue(type1String, out pokemonData.type1);
            pokemonTypeDataDictionary.TryGetValue(type2String, out pokemonData.type2);

            string[] eggGroups = IniFileAccessor.ReadValue(indexStr, "Compatibility").Split(',');
            pokemonData.eggGroup1 = (PokemonData.EggGroup)Enum.Parse(typeof(PokemonData.EggGroup), eggGroups[0].ToUpper());

            if(eggGroups.Length > 1)
            {
                pokemonData.eggGroup2 = (PokemonData.EggGroup)Enum.Parse(typeof(PokemonData.EggGroup), eggGroups[1].ToUpper());
            } else { pokemonData.eggGroup2 = PokemonData.EggGroup.NONE; }

            //STATS

            string[] baseStats = IniFileAccessor.ReadValue(indexStr, "BaseStats").Split(',');

            if (!Int32.TryParse(baseStats[0], out pokemonData.baseHP)) { Debug.LogError("Not a number! Check DataGenerator.cs"); }
            if (!Int32.TryParse(baseStats[1], out pokemonData.baseAttack)) { Debug.LogError("Not a number! Check DataGenerator.cs"); }
            if (!Int32.TryParse(baseStats[2], out pokemonData.baseDefense)) { Debug.LogError("Not a number! Check DataGenerator.cs"); }
            if (!Int32.TryParse(baseStats[3], out pokemonData.baseSpeed)) { Debug.LogError("Not a number! Check DataGenerator.cs"); }
            if (!Int32.TryParse(baseStats[4], out pokemonData.baseSpecialAttack)) { Debug.LogError("Not a number! Check DataGenerator.cs"); }
            if (!Int32.TryParse(baseStats[5], out pokemonData.baseSpecialDefense)) { Debug.LogError("Not a number! Check DataGenerator.cs"); }

            string[] evStats = IniFileAccessor.ReadValue(indexStr, "EffortPoints").Split(',');

            if (!Int32.TryParse(evStats[0], out pokemonData.EV_HP)) { Debug.LogError("Not a number! Check DataGenerator.cs"); }
            if (!Int32.TryParse(evStats[1], out pokemonData.EV_Attack)) { Debug.LogError("Not a number! Check DataGenerator.cs"); }
            if (!Int32.TryParse(evStats[2], out pokemonData.EV_Defense)) { Debug.LogError("Not a number! Check DataGenerator.cs"); }
            if (!Int32.TryParse(evStats[3], out pokemonData.EV_Speed)) { Debug.LogError("Not a number! Check DataGenerator.cs"); }
            if (!Int32.TryParse(evStats[4], out pokemonData.EV_SpecialAttack)) { Debug.LogError("Not a number! Check DataGenerator.cs"); }
            if (!Int32.TryParse(evStats[5], out pokemonData.EV_SpecialDefense)) { Debug.LogError("Not a number! Check DataGenerator.cs"); }

            pokemonData.baseEXP = Int32.Parse(IniFileAccessor.ReadValue(indexStr, "BaseEXP"));
            pokemonData.rareness = Int32.Parse(IniFileAccessor.ReadValue(indexStr, "Rareness"));
            pokemonData.baseHappiness = Int32.Parse(IniFileAccessor.ReadValue(indexStr, "Happiness"));
            pokemonData.stepsToHatch = Int32.Parse(IniFileAccessor.ReadValue(indexStr, "StepsToHatch"));
            pokemonData.growthRate = (PokemonData.GrowthRate)Enum.Parse(typeof(PokemonData.GrowthRate), IniFileAccessor.ReadValue(indexStr, "GrowthRate").ToUpper());
            pokemonData.femaleRatio = PokemonData.String2GenderRate(IniFileAccessor.ReadValue(indexStr, "GenderRate"));

            

            if (!float.TryParse(IniFileAccessor.ReadValue(indexStr, "Height"), out pokemonData.height)) { Debug.LogError("Not a number! Check DataGenerator.cs"); }
            if (!float.TryParse(IniFileAccessor.ReadValue(indexStr, "Weight"), out pokemonData.weight)) { Debug.LogError("Not a number! Check DataGenerator.cs"); }

            //SPRITES

            pokemonData.sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Graphics/Sprites/Pokemon/" + paddedindexStr + ".png", typeof(Sprite));
            pokemonData.backSprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Graphics/Sprites/Pokemon/" + paddedindexStr + "b.png", typeof(Sprite));
            pokemonData.shinySprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Graphics/Sprites/Pokemon/" + paddedindexStr + "s.png", typeof(Sprite));
            pokemonData.shinyBackSprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Graphics/Sprites/Pokemon/" + paddedindexStr + "sb.png", typeof(Sprite));

            pokemonData.footprintSprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Graphics/Sprites/Footprints/footprint" + paddedindexStr + ".png", typeof(Sprite));

            //ABILITIES
            string[] abilityString = IniFileAccessor.ReadValue(indexStr, "Abilities").Split(',');
            string hiddenAbilityString = IniFileAccessor.ReadValue(indexStr, "HiddenAbility");

            foreach(string ability in abilityString)
            {
                AbilityData abilityData;
                if(abilityDataDictionary.TryGetValue(ability, out abilityData))
                {
                    pokemonData.standardAbilities.Add(abilityData);
                } else { Debug.LogWarning("Couldn't find Ability " + ability); }
            }

            AbilityData hiddenAbilityData;
            if(abilityDataDictionary.TryGetValue(hiddenAbilityString, out hiddenAbilityData))
            {
                pokemonData.hiddenAbility = hiddenAbilityData;
            } else { Debug.LogWarning("Couldn't find Hidden Ability " + hiddenAbilityString ); }

            //MOVES

            string[] moves = IniFileAccessor.ReadValue(indexStr, "Moves").Split(',');

            for(int m = 0; m < moves.Length; m+=2)
            {

                int level = Int32.Parse(moves[m]);
                string moveName = moves[m + 1];

                MoveData moveToLearn;
                if(moveDataDictionary.TryGetValue(moveName, out moveToLearn))
                {
                    LearnedMove learnedMove = new LearnedMove(moveToLearn, level);
                    pokemonData.learnedMoves.Add(learnedMove);
                } else { Debug.LogWarning("Couldn't find move " + moveName); }

            }

            string[] eggMoves = IniFileAccessor.ReadValue(indexStr, "EggMoves").Split(',');
            foreach (string eggMove in eggMoves)
            {
                MoveData moveData;
                if (moveDataDictionary.TryGetValue(eggMove, out moveData))
                {
                    pokemonData.eggMoves.Add(moveData);
                }
                else { Debug.LogWarning("Couldn't find egg move " + eggMove); }
            }

            pokemonDataDictionary.Add(pokemonData.internalName, pokemonData);
        }


        //Evolutions
        for (int i = 1; i <= NUMBER_OF_POKEMON; i++)
        {
            string indexStr = i + "";

            PokemonData pokemonData;

            if (pokemonDataDictionary.TryGetValue(IniFileAccessor.ReadValue(indexStr, "InternalName"), out pokemonData))
            {

                string[] evolutionString = IniFileAccessor.ReadValue(indexStr, "Evolutions").Split(',');
                

                if (evolutionString.Length >= 3)
                {
                    for (int e = 0; e < evolutionString.Length; e += 3)
                    {
                        int pokemonIndex = e;
                        int typeIndex = e + 1;
                        int levelIndex = e + 2;

                        PokemonData evolvedPokemonData;
                        if (pokemonDataDictionary.TryGetValue(evolutionString[pokemonIndex], out evolvedPokemonData))
                        {

                            switch (evolutionString[typeIndex])
                            {
                                case "Level":
                                    int levelToEvolve = Int32.Parse(evolutionString[levelIndex]);
                                    Evolution levelEvolution = new Evolution(evolvedPokemonData, levelToEvolve);
                                    pokemonData.evolutions.Add(levelEvolution);
                                    break;
                                case "Item":
                                    string itemToEvolve = evolutionString[levelIndex];
                                    ItemData itemData;
                                    if (itemDataDictionary.TryGetValue(itemToEvolve, out itemData))
                                    {
                                        Evolution itemEvolution = new Evolution(evolvedPokemonData, itemData);
                                        pokemonData.evolutions.Add(itemEvolution);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        } else { Debug.LogWarning("Couldn't find resulting Pokemon of this evolution!"); }
                    }
                }
            }


        }

        pokemonDataSerializationArray = new PokemonData[pokemonDataDictionary.Count];

        pokemonDataDictionary.Values.CopyTo(pokemonDataSerializationArray, 0);

        return pokemonDataSerializationArray;
    }

    public PokemonTypeData[] GenerateTypes()
    {

        pokemonTypeDataDictionary = new Dictionary<string, PokemonTypeData>();

        IniFileAccessor.SetPath("/Data/Text Files/types.ini");

        for (int i = 0; i <= NUMBER_OF_TYPES; i++)
        {

            string indexStr = (i).ToString();


            PokemonTypeData typeData = ScriptableObject.CreateInstance<PokemonTypeData>();

            typeData.Init();

            typeData.name = IniFileAccessor.ReadValue(indexStr, "Name");
            typeData.internalName = IniFileAccessor.ReadValue(indexStr, "InternalName");
            typeData.isSpecial = IniFileAccessor.ReadValue(indexStr, "IsSpecialType").Equals("true");

            Color color;

            if (ColorUtility.TryParseHtmlString(IniFileAccessor.ReadValue(indexStr, "Color"), out color))
            {
                typeData.color = color;
            }

            pokemonTypeDataDictionary.Add(typeData.internalName, typeData);

        }


        for (int i = 0; i <= NUMBER_OF_TYPES; i++)
        {
            string indexStr = i + "";

            string pInternalName = IniFileAccessor.ReadValue(indexStr, "InternalName");

            PokemonTypeData typeData;

            if (pokemonTypeDataDictionary.TryGetValue(pInternalName, out typeData))
            {

                string[] pWeaknesses = IniFileAccessor.ReadValue(indexStr, "Weaknesses").Split(',');
                string[] pResistances = IniFileAccessor.ReadValue(indexStr, "Resistances").Split(',');
                string[] pImmunities = IniFileAccessor.ReadValue(indexStr, "Immunities").Split(',');


                foreach (string weakness in pWeaknesses)
                {
                    if (weakness == "") { break; }

                    PokemonTypeData weaknessTypeData;

                    if (pokemonTypeDataDictionary.TryGetValue(weakness, out weaknessTypeData))
                    {
                        typeData.weaknesses.Add(weaknessTypeData);
                    }
                }

                foreach (string resistance in pResistances)
                {
                    if (resistance == "") { break; }

                    PokemonTypeData resistanceTypeData;

                    if (pokemonTypeDataDictionary.TryGetValue(resistance, out resistanceTypeData))
                    {
                        typeData.resistances.Add(resistanceTypeData);
                    }
                }

                foreach (string immunity in pImmunities)
                {
                    if (immunity == "") { break; }

                    PokemonTypeData immunityTypeData;

                    if (pokemonTypeDataDictionary.TryGetValue(immunity, out immunityTypeData))
                    {
                        typeData.immunities.Add(immunityTypeData);
                    }
                }
            }
        }

        pokemonTypeDataSerializationArray = new PokemonTypeData[pokemonTypeDataDictionary.Count];

        pokemonTypeDataDictionary.Values.CopyTo(pokemonTypeDataSerializationArray, 0);

        return pokemonTypeDataSerializationArray;

    }

    public AbilityData[] GenerateAbilities()
    {

        abilityDataDictionary = new Dictionary<string, AbilityData>();

        TXTFileAccessor.SetPath("Assets/Data/Text Files/abilities.txt");

        for (int i = 0; i < NUMBER_OF_ABILITIES; i++)
        {

            AbilityData ability = ScriptableObject.CreateInstance<AbilityData>();

            string[] dataString = TXTFileAccessor.ReadValue(i).Split(',');

            ability.ID = Int32.Parse(dataString[0]);
            ability.internalName = dataString[1];
            ability.name = dataString[2];
            ability.description = dataString[3];

            abilityDataDictionary.Add(ability.internalName, ability);

        }

        abilityDataSerializationArray = new AbilityData[abilityDataDictionary.Count];
        abilityDataDictionary.Values.CopyTo(abilityDataSerializationArray, 0);
        return abilityDataSerializationArray;

    }

    public MoveData[] GenerateMoves()
    {
        moveDataDictionary = new Dictionary<string, MoveData>();

        TXTFileAccessor.SetPath("Assets/Data/Text Files/moves.txt");

        for (int i = 0; i < NUMBER_OF_MOVES; i++)
        {

            MoveData move = ScriptableObject.CreateInstance<MoveData>();

            string[] dataString = TXTFileAccessor.ReadValue(i).Split('\"');

            string[] dataStringArray = dataString[0].Split(',');

            move.ID = Int32.Parse(dataStringArray[0]);
            move.internalName = dataStringArray[1];
            move.name = dataStringArray[2];
            move.functionCode = dataStringArray[3];
            move.basePower = Int32.Parse(dataStringArray[4]);
            pokemonTypeDataDictionary.TryGetValue(dataStringArray[5], out move.type);
            move.isSpecial = dataStringArray[6].Equals("Special");
            move.accuracy = Int32.Parse(dataStringArray[7]);
            move.totalPP = Int32.Parse(dataStringArray[8]);
            move.additionalEffectChance = Int32.Parse(dataStringArray[9]);
            move.target = dataStringArray[10];
            move.priority = Int32.Parse(dataStringArray[11]);
            move.flags = dataStringArray[12].ToCharArray();

            move.description = dataString[1];
            
            moveDataDictionary.Add(move.internalName, move);

        }

        moveDataSerializationArray = new MoveData[moveDataDictionary.Count];
        moveDataDictionary.Values.CopyTo(moveDataSerializationArray, 0);
        return moveDataSerializationArray;

    }

    public ItemData[] GenerateItems()
    {
        itemDataDictionary = new Dictionary<string, ItemData>();

        TXTFileAccessor.SetPath("Assets/Data/Text Files/items.txt");

        for (int i = 0; i < NUMBER_OF_ITEMS; i++)
        {

            ItemData item = ScriptableObject.CreateInstance<ItemData>();

            string[] dataString = TXTFileAccessor.ReadValue(i).Split('\"');

            string[] dataStringArray = dataString[0].Split(',');
            
            item.ID = Int32.Parse(dataStringArray[0]);
            item.internalName = dataStringArray[1];
            item.name = dataStringArray[2];
            item.itemType = (ItemData.ItemType) Int32.Parse(dataStringArray[3]);
            item.price = Int32.Parse(dataStringArray[4]);

            string moveName = "";

            if (dataString.Length == 3)
            {
                
                string description = dataString[1];
                string[] dataStringSecondaryArray = dataString[2].Substring(1).Split(',');
                

                item.description = description;
                item.overworldUsabilityID = (ItemData.OverworldType) Int32.Parse(dataStringSecondaryArray[0]);
                item.battleUsabilityID = (ItemData.BattleType) Int32.Parse(dataStringSecondaryArray[1]);
                item.specialItemID = (ItemData.SpecialItemType) Int32.Parse(dataStringSecondaryArray[2]);
                
                moveName = dataStringSecondaryArray[3];


            } else
            {
                item.description = dataStringArray[5];
                item.overworldUsabilityID = (ItemData.OverworldType) Int32.Parse(dataStringArray[6]);
                item.battleUsabilityID = (ItemData.BattleType) Int32.Parse(dataStringArray[7]);
                item.specialItemID = (ItemData.SpecialItemType) Int32.Parse(dataStringArray[8]);
                moveName = dataStringArray[9];
            }

            moveName = moveName.TrimEnd(',', '\n', '\r');

            MoveData moveData;

            if (moveDataDictionary.TryGetValue(moveName, out moveData))
            {
                item.TMmoveToLearn = moveData;
            }
            


            //SPRITE
            string paddedindexStr = item.ID.ToString().PadLeft(3, '0'); // results in 009
            item.sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Graphics/Sprites/Icons/item" + paddedindexStr + ".png", typeof(Sprite));

            itemDataDictionary.Add(item.internalName, item);

        }

        itemDataSerializationArray = new ItemData[itemDataDictionary.Count];
        itemDataDictionary.Values.CopyTo(itemDataSerializationArray, 0);
        return itemDataSerializationArray;

    }

    #endregion
}