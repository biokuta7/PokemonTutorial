using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor
{

    ItemData comp;

    private void OnEnable()
    {
        comp = (ItemData)target;
    }

    public override void OnInspectorGUI()
    {

        //base.OnInspectorGUI();

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

        EditorGUILayout.BeginHorizontal();
        
        GUILayout.Label(comp.sprite.texture, captionStyle, GUILayout.Height(64), GUILayout.Width(64));
        EditorGUILayout.LabelField(comp.ID + ": " + comp.name, headerStyle, GUILayout.Height(64));

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("", EditorStyles.helpBox);

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("<color=blue>" + comp.itemType.ToString() + "</color>", captionStyle);
        EditorGUILayout.LabelField("<color=blue>$" + comp.price + ".00</color>", captionStyle);
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Use in overworld: <color=blue>" + comp.overworldUsabilityID.ToString() + "</color>", captionStyle);
        EditorGUILayout.LabelField("Use in battle: <color=blue>" + comp.battleUsabilityID.ToString() + "</color>", captionStyle);

        if (!comp.specialItemID.Equals(ItemData.SpecialItemType.NONE))
        {
            EditorGUILayout.LabelField("Special item: <color=blue>" + comp.specialItemID.ToString() + "</color>", captionStyle);
        }

        if (comp.TMmoveToLearn != null)
        {
            EditorGUILayout.LabelField("Teaches move <color=blue>" + comp.TMmoveToLearn.name + "</color>", captionStyle);
        }

        EditorGUILayout.LabelField("\"" + comp.description + "\"", captionStyle, GUILayout.Height(64f));

    }

}
