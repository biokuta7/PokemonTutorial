using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(PokemonTypeData))]
public class PokemonTypeDataEditor : Editor {

    PokemonTypeData comp;

    public void OnEnable()
    {
        comp = (PokemonTypeData) target;
    }

    public override void OnInspectorGUI()
    {

        //Styling

        GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);
        headerStyle.font = (Font)Resources.Load("Fonts/pkmnrs");
        headerStyle.fontSize = 24;
        headerStyle.alignment = TextAnchor.MiddleLeft;

        GUI.color = Color.white;
        GUIStyle toolbarStyle = new GUIStyle(EditorStyles.toolbar);

        toolbarStyle.alignment = TextAnchor.MiddleCenter;
        toolbarStyle.normal.textColor = Color.white;
        toolbarStyle.fontSize = 16;

        EditorGUILayout.LabelField("Type", headerStyle, GUILayout.Width(128), GUILayout.Height(32));

        GUI.backgroundColor = comp.color;
        EditorGUILayout.LabelField(comp.internalName, toolbarStyle, GUILayout.Width(128));

        DisplayOverflowingList("Weaknesses", comp.weaknesses, 128f, 32f, headerStyle, toolbarStyle);
        DisplayOverflowingList("Resistances", comp.resistances, 128f, 32f, headerStyle, toolbarStyle);
        DisplayOverflowingList("Immunities", comp.immunities, 128f, 32f, headerStyle, toolbarStyle);

    }

    void DisplayOverflowingList(string header, List<PokemonTypeData> typeDataList, float width, float height, GUIStyle headerStyle, GUIStyle toolbarStyle)
    {
        if (typeDataList.Count > 0)
        {
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("", EditorStyles.helpBox);
            

            EditorGUILayout.LabelField(header, headerStyle, GUILayout.Width(width), GUILayout.Height(height));
            EditorGUILayout.BeginHorizontal();
            int index = 0;
            foreach (PokemonTypeData typeData in typeDataList)
            {
                GUI.backgroundColor = typeData.color;
                EditorGUILayout.LabelField(typeData.internalName, toolbarStyle, GUILayout.Width(width), GUILayout.Height(height));
                index++;
                if (index == Mathf.FloorToInt(Screen.width / 128f))
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    index = 0;
                }

            }
            EditorGUILayout.EndHorizontal();
        }
    }

}
