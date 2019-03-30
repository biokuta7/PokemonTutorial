using UnityEngine;

public class FontFix : MonoBehaviour {


    public Font font;

    private void Start()
    {
        font.material.mainTexture.filterMode = FilterMode.Point;
    }

}
