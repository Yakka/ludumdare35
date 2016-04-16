using UnityEngine;
using System.Collections;

public class BuildingManipulator : MonoBehaviour {

    private bool holdingRoof = false;
    private Building grabbedBuilding;
    private int roofLevel;
    private Vector3 lastMousePosition;
	
	void Update () {
        if (Input.GetMouseButton(0)) {
            Piece piece = RaycastMouseToPiece();
            if (holdingRoof) { // Test if the player has moved the mouse enough
                // Moving down:
                if(piece != null && piece.level > 0 && piece.level < roofLevel) {
                    grabbedBuilding.MoveRoofToLevel(piece.level);
                    lastMousePosition = Input.mousePosition;
                    roofLevel--;
                } 
                // Moving up:
                else if (piece == null && lastMousePosition.y < Input.mousePosition.y) {
                    grabbedBuilding.MoveRoofToLevel(roofLevel+1);
                    roofLevel++;
                    lastMousePosition = Input.mousePosition;
                }
            } else { // Test if the player is on the roof:
                if(piece != null && piece.isRoof) {
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
        }
	}

    private Piece RaycastMouseToPiece() {
        Piece piece = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            piece = hit.transform.gameObject.GetComponent<Piece>();
        }
        return piece;
    }
}
