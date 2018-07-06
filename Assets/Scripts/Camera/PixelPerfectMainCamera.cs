using UnityEngine;

public class PixelPerfectMainCamera : MonoBehaviour {

    public int PPU = 16;

    public int screenHeight = 384;
    public int screenWidth = 512;

    public float orthoSize = 7.5f;

	private Camera pixelCamera;

	void Start() {
		pixelCamera = GetComponent<Camera> ();
	}


    public void UpdateOrthoSize()
    {
        orthoSize = (screenHeight / PPU) * .5f;
    }

    public void ApplyOrthoSize()
    {
		Start ();
		pixelCamera.rect = new Rect (0, 0, 1, 1);
        Camera.main.orthographicSize = orthoSize;
		float screenRatio = pixelCamera.aspect;
		float targetRatio = (float) screenWidth / screenHeight;

		Debug.Log (screenRatio + " " + targetRatio);

		float newScreenWidth = targetRatio/screenRatio;

		Rect rect = new Rect ((1- newScreenWidth) /2, 0, newScreenWidth, 1);

		pixelCamera.rect = rect;

    } 

}
