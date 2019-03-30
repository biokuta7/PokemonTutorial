using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MoveData))]
public class MoveDataEditor : Editor {

    MoveData comp;

    private void OnEnable()
    {
        comp = (MoveData)target;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        GUIStyle descriptionStyle = new GUIStyle(EditorStyles.wordWrappedLabel)
        {
            fontSize = 15,
            richText = true
        };

        GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            font = (Font)Resources.Load("Fonts/pkmnrs"),
            fontSize = 24,
            alignment = TextAnchor.MiddleLeft
        };

        GUIStyle captionStyle = new GUIStyle(EditorStyles.wordWrappedLabel)
        {
            fontSize = 16,
            richText = true,
            alignment = TextAnchor.UpperLeft
        };


        //NAME
        EditorGUILayout.LabelField(comp.ID + ": " + comp.name, headerStyle, GUILayout.Height(32f));

        EditorGUILayout.Separator();
        
        //TYPE
        GUI.color = Color.white;
        GUIStyle toolbarStyle = new GUIStyle(EditorStyles.toolbar);
        toolbarStyle.alignment = TextAnchor.MiddleCenter;
        toolbarStyle.normal.textColor = Color.white;
        toolbarStyle.fontSize = 16;

        GUI.backgroundColor = comp.type.color;
        EditorGUILayout.LabelField(comp.type.internalName, toolbarStyle, GUILayout.Width(128));
        GUI.backgroundColor = Color.white;
        EditorGUILayout.Separator();

        //POWER AND ACCURACY
        ProgressBar(comp.basePower / 200f, comp.basePower + " BASE POWER");
        
        if (comp.accuracy > 0)
        {
            ProgressBar(comp.accuracy / 100f, comp.accuracy + "% ACCURACY");
        } else
        {
            ProgressBar(1f, "DOUBTLESS ACCURACY");
        }

        //EXTRA DETAILS

        string additionalEffectString = "";
        if(comp.additionalEffectChance > 0)
        {
            additionalEffectString = "\n<color=blue>" + comp.additionalEffectChance + "%</color> chance to afflict opponent with <color=blue>" + comp.functionCode + "</color>";
        }

        EditorGUILayout.LabelField("<color=blue>" +
            (comp.isSpecial ? "SPECIAL" : "PHYSICAL") + "\n" +
            comp.totalPP + "</color> TOTAL PP<color=blue>\n" +
            comp.priority + "</color> PRIORITY" + additionalEffectString
            , captionStyle, GUILayout.Height(84));

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("", EditorStyles.helpBox);

        //DESCRIPTION
        EditorGUILayout.LabelField(comp.description, descriptionStyle);

        EditorGUILayout.LabelField("", EditorStyles.helpBox);
        EditorGUILayout.Separator();

        //FLAGS & TARGETS

        EditorGUILayout.LabelField("This move targets " + comp.target, captionStyle, GUILayout.Height(32));
        EditorGUILayout.Separator();

        foreach(char c in comp.flags)
        {
            EditorGUILayout.LabelField("<color=red>Flag " + c + ":</color> " + comp.flagDesc(c), captionStyle);
        }


    }

    public void ProgressBar(float value, string label)
    {
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        EditorGUI.ProgressBar(rect, value, label);
        EditorGUILayout.Space();
    }

}
