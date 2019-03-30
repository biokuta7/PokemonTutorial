using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapSettings))]
public class MapSettingsEditor : Editor
{

    MapSettings comp;

    private void OnEnable()
    {
        comp = (MapSettings)target;
    }

    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();

        
        DisplayEncounters("Grass", comp.grassEncounters);

        DisplayEncounters("Water", comp.waterEncounters);

        DisplayEncounters("Random", comp.randomEncounters);




    }

    private void DisplayEncounters(string title, MapSettings.Encounter[] encounters)
    {

        GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            font = (Font)Resources.Load("Fonts/pkmnrs"),
            fontSize = 24,
            alignment = TextAnchor.MiddleLeft
        };

        GUIStyle captionStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize = 14,
            richText = true,
            alignment = TextAnchor.MiddleCenter
        };

        GUIStyle textureStyle = new GUIStyle(EditorStyles.label)
        {
            alignment = TextAnchor.MiddleCenter
        };

        if (encounters.Length > 0)
        {

            GUILayout.Label(title + " Encounters", headerStyle, GUILayout.Width(256f), GUILayout.Height(96f));

            EditorGUILayout.BeginHorizontal();
            foreach (MapSettings.Encounter e in encounters)
            {
                GUILayout.Label(e.pokemonData.sprite.texture, textureStyle, GUILayout.Width(96f), GUILayout.Height(96f));
            }
            EditorGUILayout.EndHorizontal();

            float sum = 0;

            EditorGUILayout.BeginHorizontal();
            foreach (MapSettings.Encounter e in encounters)
            {
                GUILayout.Label(
                    e.pokemonData.name + "\nLVL " +
                    e.minLevel + "-" + e.maxLevel + "\n" +
                    e.percentageChance + "%"
                    , captionStyle, GUILayout.Width(96f), GUILayout.Height(64f));

                sum += e.percentageChance;

            }
            EditorGUILayout.EndHorizontal();

            ProgressBar(sum / 100f, sum + "/100");
        }
    }

    public void ProgressBar(float value, string label)
    {
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        EditorGUI.ProgressBar(rect, value, label);
        EditorGUILayout.Space();
    }

}
