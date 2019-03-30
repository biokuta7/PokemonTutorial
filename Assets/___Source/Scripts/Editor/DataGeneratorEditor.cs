using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(DataGenerator))]
public class DataGeneratorEditor : Editor {

    DataGenerator comp;

    public void OnEnable()
    {
        comp = (DataGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //Cleaners

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Clear Item Data"))
        {
            comp.ClearItemData();
            AssetDatabase.DeleteAsset("Assets/Data/ScriptableObjects/Items");
        }
        if (GUILayout.Button("Clear Type Data"))
        {
            comp.ClearTypeData();
            AssetDatabase.DeleteAsset("Assets/Data/ScriptableObjects/Types");
        }
        if (GUILayout.Button("Clear Ability Data"))
        {
            comp.ClearAbilityData();
            AssetDatabase.DeleteAsset("Assets/Data/ScriptableObjects/Abilites");
        }
        if (GUILayout.Button("Clear Move Data"))
        {
            comp.ClearMoveData();
            AssetDatabase.DeleteAsset("Assets/Data/ScriptableObjects/Moves");
        }
        if (GUILayout.Button("Clear Pokemon Data"))
        {
            comp.ClearPokemonData();
            AssetDatabase.DeleteAsset("Assets/Data/ScriptableObjects/Pokemon");
        }
        
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Clear ALL Data"))
        {
            comp.ClearPokemonData();
            comp.ClearMoveData();
            comp.ClearAbilityData();
            comp.ClearTypeData();
            comp.ClearItemData();
            AssetDatabase.DeleteAsset("Assets/Data/ScriptableObjects/Pokemon");
            AssetDatabase.DeleteAsset("Assets/Data/ScriptableObjects/Moves");
            AssetDatabase.DeleteAsset("Assets/Data/ScriptableObjects/Abilites");
            AssetDatabase.DeleteAsset("Assets/Data/ScriptableObjects/Types");
            AssetDatabase.DeleteAsset("Assets/Data/ScriptableObjects/Items");
        }
        
        //Item, Move and Ability Generation

        EditorGUILayout.BeginHorizontal();

        
        if (GUILayout.Button("Generate Type Data"))
        {
            GenerateTypeData();
        }
        if (GUILayout.Button("Generate Ability Data"))
        {
            GenerateAbilityData();
        }
        if (GUILayout.Button("Generate Move Data"))
        {
            GenerateMoveData();
        }
        if (GUILayout.Button("Generate Item Data"))
        {
            GenerateItemData();
        }
        if (GUILayout.Button("Generate Pokemon Data"))
        {
            GeneratePokemonData();

        }

        EditorGUILayout.EndHorizontal();

        if(GUILayout.Button("Generate All Data"))
        {
            EditorGUI.BeginChangeCheck();
            EditorUtility.SetDirty(this);
            EditorSceneManager.MarkAllScenesDirty();
            
            GenerateTypeData();
            GenerateAbilityData();
            GenerateMoveData();
            GenerateItemData();
            GeneratePokemonData();
            
            AssetDatabase.SaveAssets();
            if(EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(comp, "arrays");
            }
        }

        EditorGUILayout.LabelField(
            "Generate in this order: Type, Ability, Move, Item, Pokemon. Clear beforehand.", 
            EditorStyles.helpBox);
    }

    void GenerateItemData() {
        Directory.CreateDirectory(Application.dataPath + "/Data/ScriptableObjects/Items");

        int index = 0;

        foreach (ItemData item in comp.GenerateItems())
        {
            if (EditorUtility.DisplayCancelableProgressBar(
                "Item Generation Progress",
                "Currently generating " + item.name + " ability data...",
                ((float)index) / DataGenerator.NUMBER_OF_ITEMS))
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            string paddedindexStr = index.ToString().PadLeft(3, '0'); // results in 009

            AssetDatabase.CreateAsset(item, "Assets/Data/ScriptableObjects/Items/" + paddedindexStr + "-" + item.name + "ItemData.asset");

            EditorUtility.SetDirty(item);
            index++;
        }

        EditorUtility.ClearProgressBar();
    }
    void GenerateTypeData() {
        Directory.CreateDirectory(Application.dataPath + "/Data/ScriptableObjects/Types");

        int index = 0;

        foreach (PokemonTypeData typeData in comp.GenerateTypes())
        {
            if (EditorUtility.DisplayCancelableProgressBar(
                "Type Generation Progress",
                "Currently generating " + typeData.name + " type data...",
                ((float)index) / DataGenerator.NUMBER_OF_TYPES))
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            string paddedindexStr = index.ToString().PadLeft(3, '0'); // results in 009

            AssetDatabase.CreateAsset(typeData, "Assets/Data/ScriptableObjects/Types/" + paddedindexStr + "-" + typeData.name + "TypeData.asset");

            EditorUtility.SetDirty(typeData);

            index++;
        }

        EditorUtility.ClearProgressBar();
    }
    void GenerateAbilityData() {
        Directory.CreateDirectory(Application.dataPath + "/Data/ScriptableObjects/Abilites");

        int index = 0;

        foreach (AbilityData ability in comp.GenerateAbilities())
        {
            if (EditorUtility.DisplayCancelableProgressBar(
                "Ability Generation Progress",
                "Currently generating " + ability.name + " ability data...",
                ((float)index) / DataGenerator.NUMBER_OF_ABILITIES))
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            string paddedindexStr = index.ToString().PadLeft(3, '0'); // results in 009

            AssetDatabase.CreateAsset(ability, "Assets/Data/ScriptableObjects/Abilites/" + paddedindexStr + "-" + ability.name + "AbilityData.asset");
            
            EditorUtility.SetDirty(ability);

            index++;
        }

        EditorUtility.ClearProgressBar();
    }
    void GenerateMoveData() {
        Directory.CreateDirectory(Application.dataPath + "/Data/ScriptableObjects/Moves");

        int index = 0;

        foreach (MoveData move in comp.GenerateMoves())
        {
            if (EditorUtility.DisplayCancelableProgressBar(
                "Move Generation Progress",
                "Currently generating " + move.name + " ability data...",
                ((float)index) / DataGenerator.NUMBER_OF_MOVES))
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            string paddedindexStr = index.ToString().PadLeft(3, '0'); // results in 009

            AssetDatabase.CreateAsset(move, "Assets/Data/ScriptableObjects/Moves/" + paddedindexStr + "-" + move.name + "MoveData.asset");

            EditorUtility.SetDirty(move);
            index++;
        }

        EditorUtility.ClearProgressBar();
    }
    void GeneratePokemonData()
    {
        Directory.CreateDirectory(Application.dataPath + "/Data/ScriptableObjects/Pokemon");
        int index = 0;
        foreach (PokemonData pokemonData in comp.GeneratePokemon())
        {

            string paddedindexStr = index.ToString().PadLeft(3, '0'); // results in 009

            if (EditorUtility.DisplayCancelableProgressBar(
                "Pokemon Generation Progress",
                "Currently generating " + pokemonData.name + " data...",
                ((float)index) / DataGenerator.NUMBER_OF_POKEMON))
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            AssetDatabase.CreateAsset(pokemonData, "Assets/Data/ScriptableObjects/Pokemon/" + paddedindexStr + "-" + pokemonData.name + "PokemonData.asset");


            EditorUtility.SetDirty(pokemonData);

            index++;
        }

        EditorUtility.ClearProgressBar();
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }

}
