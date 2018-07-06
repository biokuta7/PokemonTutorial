using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PokemonData))]
public class PokemonDataEditor : Editor {

    PokemonData comp;
    

    public void OnEnable()
    {
        comp = (PokemonData)target;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        GUIStyle descriptionStyle = new GUIStyle(EditorStyles.wordWrappedLabel)
        {
            
            fontSize = 15,
            font = (Font)Resources.Load("Fonts/consola"),
            richText = true
        };

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


        //BASIC DATA

        EditorGUILayout.LabelField("BASIC DATA", headerStyle, GUILayout.Height(32f));

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(
            "<b>No. " + comp.number + ": " + comp.name + "\n" +
            "The <color=blue>" + comp.kind + "</color> Pokemon, " +
            "lives in <color=blue>" + comp.habitat.ToString().ToLower() + "</color> area\n<color=blue>" +
            comp.height + "</color> m <color=blue>" + comp.weight + "</color> kg\n\n" +
            "</b><i>\"" + comp.dexEntry + "\"</i>", 
            descriptionStyle, GUILayout.Width(500f), GUILayout.Height(156f));

        GUILayout.Label(comp.footprintSprite.texture, textureStyle, GUILayout.Width(64f), GUILayout.Height(128f));

        GUILayout.Label("", textureStyle, GUILayout.Width(64f));

        EditorGUILayout.EndHorizontal();

        if (comp.femaleRatio >= 0f)
        {
            EditorGUILayout.LabelField(
                "Has a <color=blue>" + comp.femaleRatio * 100f + "</color>% chance to be female", descriptionStyle
                );
        } else
        {
            EditorGUILayout.LabelField(
                "Is <color=blue>genderless</color>", descriptionStyle
                );
        }
        EditorGUILayout.LabelField(
            "Takes an egg <color=blue>" + comp.stepsToHatch + "</color> steps to hatch", descriptionStyle
            );
        EditorGUILayout.LabelField(
            "Has a catch ratio of <color=blue>" + comp.rareness + "</color>", descriptionStyle
            );
        EditorGUILayout.LabelField(
            "Starts with <color=blue>" + comp.baseHappiness + "</color> base happiness", descriptionStyle
            );
        EditorGUILayout.LabelField(
            "Grows experience at a <color=blue>" + comp.growthRate.ToString() + "</color> speed", descriptionStyle
            );
        EditorGUILayout.LabelField(
            "On defeat, yields base <color=blue>" + comp.baseEXP + "</color> EXP to victor", descriptionStyle
            );


        //TYPES

        GUI.backgroundColor = Color.black;
        EditorGUILayout.LabelField("", EditorStyles.helpBox);

        EditorGUILayout.LabelField("TYPES", headerStyle, GUILayout.Height(32f));

        EditorGUILayout.BeginHorizontal();
        GUI.color = Color.white;
        GUIStyle toolbarStyle = new GUIStyle(EditorStyles.toolbar);

        toolbarStyle.alignment = TextAnchor.MiddleCenter;
        toolbarStyle.normal.textColor = Color.white;
        toolbarStyle.fontSize = 16;

        GUI.backgroundColor = comp.type1.color;
        EditorGUILayout.LabelField(comp.type1.internalName, toolbarStyle, GUILayout.Width(128));

        if (!comp.type2.internalName.Equals(""))
        {
            GUI.backgroundColor = comp.type2.color;
            EditorGUILayout.LabelField(comp.type2.internalName, toolbarStyle, GUILayout.Width(128));
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();
        toolbarStyle.normal.textColor = Color.black;
        EditorGUILayout.BeginHorizontal();
        GUI.backgroundColor = Color.white;
        EditorGUILayout.LabelField(comp.eggGroup1.ToString(), toolbarStyle, GUILayout.Width(128));
        if (!comp.eggGroup2.ToString().Equals("NONE"))
        {
            EditorGUILayout.LabelField(comp.eggGroup2.ToString(), toolbarStyle, GUILayout.Width(128));
        }
        toolbarStyle.normal.textColor = Color.white;
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.Separator();

        //Abilities
        GUI.backgroundColor = Color.black;
        EditorGUILayout.LabelField("", EditorStyles.helpBox);

        EditorGUILayout.LabelField("ABILITIES", headerStyle, GUILayout.Height(32f));

        EditorGUILayout.BeginHorizontal();

        string abilitiesString = "<color=blue>" + comp.standardAbilities[0].name;

        for(int i = 1; i < comp.standardAbilities.Count; i++)
        {
            abilitiesString += ", " + comp.standardAbilities[i].name;
        }

        EditorGUILayout.LabelField(abilitiesString + "</color>", descriptionStyle);

        EditorGUILayout.EndHorizontal();

        if (comp.hiddenAbility != null)
        {
            EditorGUILayout.LabelField("Hidden Ability: <color=blue>" + comp.hiddenAbility.name + "</color>", descriptionStyle);
        }

        EditorGUILayout.Separator();

        //Sprites
        GUI.backgroundColor = Color.black;
        EditorGUILayout.LabelField("", EditorStyles.helpBox);

        EditorGUILayout.LabelField("SPRITES", headerStyle, GUILayout.Height(32f));


        EditorGUILayout.BeginHorizontal();
            GUILayout.Label(comp.sprite.texture, textureStyle, GUILayout.Width(128f), GUILayout.Height(128f));
            GUILayout.Label(comp.backSprite.texture, textureStyle, GUILayout.Width(128f), GUILayout.Height(128f));
            GUILayout.Label(comp.shinySprite.texture, textureStyle, GUILayout.Width(128f), GUILayout.Height(128f));
            GUILayout.Label(comp.shinyBackSprite.texture, textureStyle, GUILayout.Width(128f), GUILayout.Height(128f));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Main Sprite", captionStyle, GUILayout.Width(128f));
        EditorGUILayout.LabelField("Back Sprite", captionStyle, GUILayout.Width(128f));
        EditorGUILayout.LabelField("Shiny Sprite", captionStyle, GUILayout.Width(128f));
        EditorGUILayout.LabelField("Shiny Back Sprite", captionStyle, GUILayout.Width(128f));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        //BASE STATS
        GUI.backgroundColor = Color.black;
        EditorGUILayout.LabelField("", EditorStyles.helpBox);

        EditorGUILayout.LabelField("BASE STATS", headerStyle, GUILayout.Height(32f));

        GUI.backgroundColor = Color.cyan;

        EditorGUIUtility.labelWidth = 128f;

        GUI.backgroundColor = Color.white;
        ProgressBar(comp.baseHP / 200f, comp.baseHP + " HP");
        ProgressBar(comp.baseAttack / 200f, comp.baseAttack + " Attack");
        ProgressBar(comp.baseDefense / 200f, comp.baseDefense + " Defense");
        ProgressBar(comp.baseSpecialAttack / 200f, comp.baseSpecialAttack + " Special Attack");
        ProgressBar(comp.baseSpecialDefense / 200f, comp.baseSpecialDefense + " Special Defense");
        ProgressBar(comp.baseSpeed / 200f, comp.baseSpeed + " Speed");

        EditorGUILayout.Separator();

        string EVYieldString = "Yields <color=blue>";

        if(comp.EV_HP>0) { EVYieldString += comp.EV_HP + "</color> HP EV(s) <color=blue>"; }
        else if (comp.EV_Attack > 0) { EVYieldString += comp.EV_Attack + "</color> Attack EV(s) <color=blue>"; }
        else if (comp.EV_Defense > 0) { EVYieldString += comp.EV_Defense + "</color> Defense EV(s) <color=blue>"; }
        else if (comp.EV_Speed > 0) { EVYieldString += comp.EV_Speed + "</color> Speed EV(s) <color=blue>"; }
        else if (comp.EV_SpecialAttack > 0) { EVYieldString += comp.EV_SpecialAttack + "</color> Special Attack EV(s) <color=blue>"; }
        else if (comp.EV_SpecialDefense > 0) { EVYieldString += comp.EV_SpecialDefense + "</color> Special Defense EV(s) <color=blue>"; }

        EVYieldString += "</color>";

        EditorGUILayout.LabelField(EVYieldString, captionStyle, GUILayout.Height(32f));

        //EVOLUTIONS

        if (comp.evolutions.Count > 0)
        {
            GUI.backgroundColor = Color.black;
            EditorGUILayout.LabelField("", EditorStyles.helpBox);

            EditorGUILayout.LabelField("EVOLUTIONS", headerStyle, GUILayout.Height(32f));

            foreach (Evolution evolution in comp.evolutions)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("", GUILayout.Width(64f));

                switch (evolution.evolutionType) {
                    case Evolution.EvolutionType.LEVEL:
                        GUILayout.Label(evolution.resultant.sprite.texture, GUILayout.Width(64f), GUILayout.Height(64f));
                        EditorGUILayout.LabelField("Evolves into <b>" + evolution.resultant.name + "</b> at level <color=blue>" + evolution.levelToEvolve + "</color>", captionStyle, GUILayout.Height(64f));        
                        break;
                    case Evolution.EvolutionType.ITEM:
                        GUILayout.Label(evolution.resultant.sprite.texture, textureStyle, GUILayout.Width(64f), GUILayout.Height(64f));
                        EditorGUILayout.LabelField("Evolves into <b>" + evolution.resultant.name + "</b> with item <color=blue>" + evolution.itemToEvolve.name + "</color>", captionStyle, GUILayout.Height(64f));
                        GUILayout.Label(evolution.itemToEvolve.sprite.texture, textureStyle, GUILayout.Width(32f), GUILayout.Height(64f));
                        //GUILayout.Label("", captionStyle, GUILayout.Width(128f), GUILayout.Height(64f));
                        break;
                    default:
                        break;

                }
                
                EditorGUILayout.LabelField("", GUILayout.Width(64f));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Separator();
            }

        }

        //MOVES

        if (comp.learnedMoves.Count > 0)
        {
            GUI.backgroundColor = Color.black;
            EditorGUILayout.LabelField("", EditorStyles.helpBox);

            EditorGUILayout.LabelField("MOVES", headerStyle, GUILayout.Height(32f));

            foreach (LearnedMove learnedMove in comp.learnedMoves)
            {
                EditorGUILayout.BeginHorizontal();

                GUI.backgroundColor = learnedMove.move.type.color;
                EditorGUILayout.LabelField(learnedMove.move.type.internalName, toolbarStyle, GUILayout.Width(128));
                GUI.backgroundColor = Color.white;
                EditorGUILayout.LabelField("Learns <b>" + learnedMove.move.name.PadRight(40, '.') + "</b> at level <color=blue>" + learnedMove.level.ToString().PadLeft(2,' ') + "</color>", descriptionStyle, GUILayout.Height(20f));
                //EditorGUILayout.Separator();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Separator();

            foreach (MoveData eggMove in comp.eggMoves)
            {
                EditorGUILayout.BeginHorizontal();
                GUI.backgroundColor = eggMove.type.color;
                EditorGUILayout.LabelField(eggMove.type.internalName, toolbarStyle, GUILayout.Width(128));
                GUI.backgroundColor = Color.white;
                EditorGUILayout.LabelField("Learns <b>" + eggMove.name.PadRight(40, '.') + "</b> as <color=blue>egg move</color>", descriptionStyle, GUILayout.Height(20f));
                //EditorGUILayout.Separator();
                EditorGUILayout.EndHorizontal();
            }

        }

    }

    public void ProgressBar(float value, string label)
    {
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        EditorGUI.ProgressBar(rect, value, label);
        EditorGUILayout.Space();
    }

}
