using UnityEngine;
using System.Collections;

public class BuildingManipulator : MonoBehaviour {
    [System.Serializable]
    public struct CursorTextures {
        public Texture2D release;
        public Texture2D holden;
    }

    private struct ZoomMovement {
        public Vector3 targetPosition;
        public float targetSize;
        public bool isZooming;
    }

    private bool holdingRoof = false;
    private Building grabbedBuilding;
    private int roofLevel;
    private Vector3 lastMousePosition;

    public int overedLevel;
    public float zoomValue;

    public CursorTextures standardCursor;
    public CursorTextures dragDropCursor;

    private float orthographicSize;
    private ZoomMovement cameraMovement;
    public bool hasZoomed = false;

    public void Start() {
        orthographicSize = GetComponent<Camera>().orthographicSize;
        cameraMovement.isZooming = false;
    }

    void Update () {
        Piece piece = RaycastMouseToPiece();
        // Click
        if (Input.GetMouseButton(0)) { // TODO: refaire avec une fonction qui d�tecte � quel �tage est la souris (objets invisibles qui raycast?)
            if (holdingRoof) { // Test if the player has moved the mouse enough
                Cursor.SetCursor(dragDropCursor.holden, Vector2.zero, CursorMode.ForceSoftware);
                // Moving down:
                if (piece != null && piece.level > 0 && piece.level < roofLevel) {
                    grabbedBuilding.MoveRoofToLevel(piece.level);
                    lastMousePosition = Input.mousePosition;
                    roofLevel--;
                } 
                // Moving up:
                else if (piece == null && lastMousePosition.y < Input.mousePosition.y && roofLevel < 6) {
                    grabbedBuilding.MoveRoofToLevel(roofLevel+1);
                    roofLevel++;
                    lastMousePosition = Input.mousePosition;
                }
            } else { // Test if the player is on the roof:
                Cursor.SetCursor(standardCursor.holden, Vector2.zero, CursorMode.ForceSoftware);
                if (piece != null && piece.isRoof) {
                    Cursor.SetCursor(dragDropCursor.holden, Vector2.zero, CursorMode.ForceSoftware);
                    holdingRoof = true;
                    roofLevel = piece.level;
                    grabbedBuilding = piece.GetComponentInParent<Building>();
                    lastMousePosition = Input.mousePosition;
                    if (grabbedBuilding == null) {
                        Debug.LogError("BuildingManipulator.Update: building not found for the roof " + piece.name + ".");
                    }
                }
            }
            
        } else {
            holdingRoof = false;
            if(RaycastMouseToRoof() != null) { // Drag & drop cursor:
                Cursor.SetCursor(dragDropCursor.release, Vector2.zero, CursorMode.ForceSoftware);
            } else { // Standard cursor:
                Cursor.SetCursor(standardCursor.release, Vector2.zero, CursorMode.ForceSoftware);
            }
        }
        
        if (piece != null) {
            overedLevel = piece.level;
        } else {
            overedLevel = -1;
        }

        // Camera zooming
        if(cameraMovement.isZooming) {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, cameraMovement.targetSize, 2f* Time.deltaTime);
        }
	}

    private Piece RaycastMouseToPiece() {
        Piece piece = null;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, ~(1 << 8));
        if (hit.collider != null) {
            piece = hit.transform.gameObject.GetComponent<Piece>();
        }
        return piece;
    }

    private Piece RaycastMouseToRoof() {
        Piece piece = RaycastMouseToPiece();
        if (piece != null && !piece.isRoof) {
            piece = null;
        }
        return piece;
    }

    public void ZoomIn(int level) {
        hasZoomed = true;
        cameraMovement.targetSize = zoomValue;
        cameraMovement.isZooming = true;
    }
}
