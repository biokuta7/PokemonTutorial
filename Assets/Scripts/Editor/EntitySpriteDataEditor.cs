using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EntitySpriteData))]
public class EntitySpriteDataEditor : Editor
{

    EntitySpriteData comp;
    int timer = 0;
    int indexer = 0;
    int[] spriteIndexArray = { 0, 1, 0, 2 };

    public void OnEnable()
    {
        comp = (EntitySpriteData)target;
    }

    public override bool RequiresConstantRepaint()
    {
        return true;
    }
    

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        
        timer++;

        if (timer >= 16)
        {
            timer = 0;
            indexer++;
            if (indexer >= 4) { indexer = 0; }
        }
        int index = spriteIndexArray[indexer];

        int middle = Screen.width / 2;

        if (comp.northSprites.Length == 3)
        {
            Rect northSpritesRect = new Rect(middle - 64, 400, 64, 96);
            Sprite s = comp.northSprites[index];
            DrawTexturePreview(northSpritesRect, s);
        }
        if (comp.eastSprites.Length == 3)
        {
            Rect eastSpritesRect = new Rect(middle, 400, 64, 96);
            Sprite s = comp.eastSprites[index];
            DrawTexturePreview(eastSpritesRect, s);
        }

        if (comp.southSprites.Length == 3)
        {
            Rect southSpritesRect = new Rect(middle + 64, 400, 64, 96);
            Sprite s = comp.southSprites[index];
            DrawTexturePreview(southSpritesRect, s);
        }



    }

    private void DrawTexturePreview(Rect position, Sprite sprite)
    {
        Vector2 fullSize = new Vector2(sprite.texture.width, sprite.texture.height);
        Vector2 size = new Vector2(sprite.textureRect.width, sprite.textureRect.height);

        Rect coords = sprite.textureRect;
        coords.x /= fullSize.x;
        coords.width /= fullSize.x;
        coords.y /= fullSize.y;
        coords.height /= fullSize.y;

        Vector2 ratio;
        ratio.x = position.width / size.x;
        ratio.y = position.height / size.y;
        float minRatio = Mathf.Min(ratio.x, ratio.y);

        Vector2 center = position.center;
        position.width = size.x * minRatio;
        position.height = size.y * minRatio;
        position.center = center;

        GUI.DrawTextureWithTexCoords(position, sprite.texture, coords);
    }

}
