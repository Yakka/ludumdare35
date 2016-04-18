using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
    [System.Serializable]
    public struct CursorTextures {
        public Texture2D standard;
        public Texture2D over;
    }

    public CursorTextures cursor;
    private bool isOver = false;

    public void Update() {
        if(isOver) {
            Cursor.SetCursor(cursor.over, Vector2.zero, CursorMode.ForceSoftware);
        } else {
            Cursor.SetCursor(cursor.standard, Vector2.zero, CursorMode.ForceSoftware);
        }
    }

    public void LoadGame() {
        SceneManager.LoadScene(1);
    }

    public void SwapCursor() {
        isOver = !isOver;
    }
}
