using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PixelPerfectMainCamera))]
public class PixelPerfectMainCameraEditor : Editor
{

    PixelPerfectMainCamera comp;

    void OnEnable()
    {
        comp = (PixelPerfectMainCamera)target;
    }

	public override void OnInspectorGUI()
    {

        

        EditorGUI.BeginChangeCheck();

        comp.PPU = EditorGUILayout.IntField("Pixels Per Unit", comp.PPU);
        comp.screenHeight = EditorGUILayout.IntField("Screen height", comp.screenHeight);
        comp.screenWidth = EditorGUILayout.IntField("Screen width", comp.screenWidth);

        if (EditorGUI.EndChangeCheck()) {
            comp.UpdateOrthoSize();
        }

        EditorGUILayout.LabelField("Calculated Orthographic Size", comp.orthoSize + "");

        if(GUILayout.Button("Apply to Main Camera"))
        {

            comp.ApplyOrthoSize();

        }




    }

}
