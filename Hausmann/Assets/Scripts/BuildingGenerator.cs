using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingGenerator : MonoBehaviour {
    
    public int numberOfLevels;
    public int numberOfRows;

    public Piece windowPrefab;
    public Piece cornerLeftPrefab;
    public Piece cornerRightPrefab;
    public Piece balcony1Prefab;


    void Start () {
	    for(int level = 0; level < numberOfLevels; level++) {
            // Corners
            Piece cornerLeft = Instantiate(cornerLeftPrefab, transform.position, transform.rotation) as Piece;
            cornerLeft.transform.parent = transform;
            cornerLeft.transform.Translate(numberOfRows * Utiles.METRIC_X, 0, level * Utiles.METRIC_Y);
            cornerLeft.name = "CornerLeftLevel" + level;

            Piece cornerRight = Instantiate(cornerRightPrefab, transform.position, transform.rotation) as Piece;
            cornerRight.transform.parent = transform;
            cornerRight.transform.Translate(-Utiles.METRIC_X, 0, level * Utiles.METRIC_Y);
            cornerRight.name = "CornerRightLevel" + level;

            // Level building
            for (int column = 0; column < numberOfRows; column++) {
                // Balcony
                if(level == 1 || level == 5) {
                    
                }
                // windows:
                else {
                    Piece window = Instantiate(windowPrefab, transform.position, transform.rotation) as Piece;
                    window.transform.parent = transform;
                    window.transform.Translate(column * Utiles.METRIC_X, 0, level * Utiles.METRIC_Y);
                    window.name = "WindowColumn" + column + "Level" + level;
                }
            }
        }
	}
	
	private Piece FindNextPiece(Piece _previousPiece, List<Piece> _nextPieces) {
        List<Piece> authorizedPieces = new List<Piece>();

        if (_nextPieces == null || _nextPieces.Count == 0) {
            Debug.LogError(GetType().Name + ".FindNextPiece: _nextPieces is null or empty.");
            return null;
        }

        if (_previousPiece == null) {
            Debug.LogError(GetType().Name + ".FindNextPiece: _previousPiece is null.");
            return null;
        }

        foreach(Piece nextPiece in _nextPieces) {
            if(_previousPiece.IsMatching(nextPiece)) {
                authorizedPieces.Add(nextPiece);
            }
        }

        return authorizedPieces[Random.Range(0, authorizedPieces.Count)];
    }
}
