using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AbilityData))]
public class AbilityDataEditor : Editor {

    AbilityData comp;

    private void OnEnable()
    {
        comp = (AbilityData)target;
    }

    public override void OnInspectorGUI()
    {

        GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            font = (Font)Resources.Load("Fonts/pkmnrs"),
            fontSize = 24,
            alignment = TextAnchor.MiddleLeft
        };

        GUIStyle captionStyle = new GUIStyle(EditorStyles.wordWrappedLabel)
        {
            fontSize = 14,
            richText = true,
            alignment = TextAnchor.MiddleLeft
        };

        EditorGUILayout.LabelField(comp.ID + ": " + comp.name, headerStyle, GUILayout.Height(32f));

        EditorGUILayout.LabelField(comp.description, captionStyle, GUILayout.Height(32f));

    }

}
