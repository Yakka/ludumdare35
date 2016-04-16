using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingGenerator : MonoBehaviour {
    
    public int numberOfLevels;
    public int numberOfRows;

    public Piece windowPrefab;
    public Piece cornerLeftPrefab;
    public Piece cornerRightPrefab;
    public List<Piece> balconyPrefabs = new List<Piece>();


    void Start () {
        Piece previousPiece = null;
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
                string name = string.Empty;
                Piece piece = null;
                // Balcony
                if(level == 1 || level == 5) {
                    piece = Instantiate(FindNextPiece(balconyPrefabs, previousPiece), transform.position, transform.rotation) as Piece;
                    name = "Balcony1";
                }
                // Windows:
                else {
                    piece = Instantiate(windowPrefab, transform.position, transform.rotation) as Piece;
                    name = "Window";
                }
                piece.transform.parent = transform;
                piece.transform.Translate(column * Utiles.METRIC_X, 0, level * Utiles.METRIC_Y);
                piece.name = name + column + "Level" + level;
                previousPiece = piece;
            }
        }
	}
	
	private Piece FindNextPiece(List<Piece> _nextPieces, Piece _previousPiece = null) {
        List<Piece> authorizedPieces = new List<Piece>();

        if (_nextPieces == null || _nextPieces.Count == 0) {
            Debug.LogError(GetType().Name + ".FindNextPiece: _nextPieces is null or empty.");
            return null;
        }

        // If their is not previous piece, it's free for all:
        if (_previousPiece == null) {
            return _nextPieces[Random.Range(0, authorizedPieces.Count)];
        }
        // Else, we search all the pieces matching with the previous one:
        foreach(Piece nextPiece in _nextPieces) {
            if(_previousPiece.IsMatching(nextPiece)) {
                authorizedPieces.Add(nextPiece);
            }
        }

        return authorizedPieces[Random.Range(0, authorizedPieces.Count)];
    }
}
