using UnityEngine;
using System.Collections;

public class BuildingManipulator : MonoBehaviour {

    private bool holdingRoof = false;
    private Building grabbedBuilding;
    private int roofLevel;
	
	void Update () {
        if (Input.GetMouseButton(0)) {
            Piece piece = RaycastMouseToPiece();
            if (holdingRoof) { // Test if the player has move the mouse enough:
                if(piece != null && piece.level < roofLevel && piece.level > 0) {
                    grabbedBuilding.DeleteLevel(piece.level);
                }
            } else { // Test if the player is on the roof:
                if(piece != null && piece.isRoof) {
                    holdingRoof = true;
                    roofLevel = piece.level;
                    grabbedBuilding = piece.GetComponentInParent<Building>();
                    if(grabbedBuilding == null) {
                        Debug.LogError("BuildingManipulator.Update: building not found for the roof " + piece.name + ".");
                    } else {
                        Debug.Log("lol");
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
