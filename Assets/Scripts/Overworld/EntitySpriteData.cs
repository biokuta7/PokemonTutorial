using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEntitySpriteData", menuName = "Entity Sprite Data", order = 0)]
public class EntitySpriteData : ScriptableObject
{
    [Header("Neutral, Left Stride, Right Stride")]
    public Sprite[] northSprites;
    public Sprite[] eastSprites;
    public Sprite[] southSprites;
}
