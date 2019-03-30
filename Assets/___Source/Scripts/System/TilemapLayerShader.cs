using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

[ExecuteAlways]
public class TilemapLayerShader : MonoBehaviour
{

    public Color hiddenLayerColor = Color.grey;

    Tilemap[] tilemapLayers;

    private void Start()
    {
        if(Application.IsPlaying(gameObject))
        {
            ResetColors();
        } else {
            tilemapLayers = GetComponentsInChildren<Tilemap>();
        }
    }

    private void OnRenderObject()
    {
        if (!Application.IsPlaying(gameObject)) {
            if (tilemapLayers != null && tilemapLayers.Length > 0 && Selection.gameObjects.Length > 0)
            {

                Tilemap selectedTilemap = Selection.activeGameObject.GetComponent<Tilemap>();

                foreach (Tilemap t in tilemapLayers)
                {
                    if (selectedTilemap != null && !t.Equals(selectedTilemap))
                    {
                        t.color = hiddenLayerColor;
                    }
                    else
                    {
                        t.color = Color.white;
                    }
                }
            } else
            {
                ResetColors();
            }
        }
    }

    private void ResetColors()
    {
        foreach (Tilemap t in tilemapLayers)
        {
            t.color = Color.white;
        }
    }

}
