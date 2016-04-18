using UnityEngine;
using System.Collections;

public class CustomButton : MonoBehaviour {

    public Texture2D texture1;
    public Texture2D texture2;

    public bool drawOnOver = false;
    public int levelToDraw;
    public BuildingManipulator buildingManipulator;

    private bool hasSwapped = false;
    private CanvasRenderer canvas;
    public bool hasDisappeared = false;


    public void Start() {
        canvas = GetComponent<CanvasRenderer>();
        if(hasDisappeared) {
            Disappear();
        }
    }

    public void SwapPicture() {
        if(hasSwapped) {
            canvas.SetTexture(texture1);
        } else {
            canvas.SetTexture(texture2);
        }
        hasSwapped = !hasSwapped;
    }

    public void CleanButton() {
        canvas.SetTexture(texture1);
        hasSwapped = false;
    }

    public void Disappear() {
        hasDisappeared = true;
        GetComponent<UnityEngine.UI.Image>().enabled = false;
    }

    public void Appear() {
        hasDisappeared = false;
        GetComponent<UnityEngine.UI.Image>().enabled = true;
    }

    public void SwitchAppearance() {
        if(hasDisappeared) {
            Appear();
        } else {
            Disappear();
        }
    }

    public void Update() {
        if(drawOnOver) {
            if(buildingManipulator.overedLevel == levelToDraw && !hasDisappeared) {
                GetComponent<UnityEngine.UI.Image>().enabled = true;
            }
            else {
                GetComponent<UnityEngine.UI.Image>().enabled = false;
            }
        }
    }

    
}
