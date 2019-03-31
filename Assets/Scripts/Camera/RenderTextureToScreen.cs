using UnityEngine;

[ExecuteInEditMode]
public class RenderTextureToScreen : MonoBehaviour {

	public RenderTexture bl;
    public Material TransitionMaterial;

    void OnRenderImage(RenderTexture src, RenderTexture dst) {

        if (bl != null)
        {
            if (TransitionMaterial != null)
                Graphics.Blit(bl, dst, TransitionMaterial);
            else
            {
                Graphics.Blit(bl, dst);
            }
        }
	}

}
