using UnityEngine;
using System.Collections;

public class CustomButton : MonoBehaviour {

    public Texture2D texture1;
    public Texture2D texture2;

    private bool hasSwapped = false;
    private CanvasRenderer canvas;

    public void Start() {
        canvas = GetComponent<CanvasRenderer>();
    }

    public void SwapPicture() {
        if(hasSwapped) {
            canvas.SetTexture(texture1);
        } else {
            canvas.SetTexture(texture2);
        }
        hasSwapped = !hasSwapped;
    }
}
