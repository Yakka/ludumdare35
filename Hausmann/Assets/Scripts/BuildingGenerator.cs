using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingGenerator : MonoBehaviour {
    
    public int amountOfLevels;
    public int amountOfColumns;

    public List<int> levelsWithBalconies = new List<int>();

    public Piece windowPrefab;
    public Piece cornerLeftPrefab;
    public Piece cornerRightPrefab;
    public Piece doorPrefab;
    public List<Piece> balconyPrefabs = new List<Piece>();
    public Piece groundWindowPrefab;
    public Piece roofPrefab;


    void Start () {
        Vector3 translationY;
        Vector3 translationX;
        float currentHigh = 0;
        int doorColumnIndex = Random.Range(0, amountOfColumns + 1);
        for (int level = 0; level <= amountOfLevels; level++) {
            Piece previousPiece = null;
            float metricY = level == 0 ? Utiles.METRIC_LARGE_Y : Utiles.METRIC_Y; // TODO: cleaner writing
            translationY = Vector3.back * currentHigh;

            // Corners:
            if (level > 0 && level < amountOfLevels) {
                Piece cornerLeft = Instantiate(cornerLeftPrefab, transform.position, transform.rotation) as Piece;
                cornerLeft.transform.parent = transform;
                cornerLeft.transform.Translate(translationY);
                cornerLeft.transform.Translate(Utiles.METRIC_X, 0, 0);
                cornerLeft.name = "CornerLeftLevel" + level;

                Piece cornerRight = Instantiate(cornerRightPrefab, transform.position, transform.rotation) as Piece;
                cornerRight.transform.parent = transform;
                cornerRight.transform.Translate(translationY);
                cornerRight.transform.Translate(-(amountOfColumns + 1) * Utiles.METRIC_X, 0, 0);
                cornerRight.name = "CornerRightLevel" + level;
            }
            // Main building:
            for (int column = 0; column <= amountOfColumns; column++) {
                translationX = Vector3.left * column * Utiles.METRIC_X;
                string name = string.Empty;
                Piece piece = null;
                // Ground level:
                if(level == 0) {
                    // Door:
                    if(column == doorColumnIndex) {
                        piece = Instantiate(doorPrefab, transform.position, transform.rotation) as Piece;
                        name = "Door";
                    }
                    // Ground Window:
                    else {
                        piece = Instantiate(groundWindowPrefab, transform.position, transform.rotation) as Piece;
                        name = "GroundWindow";
                    }
                    
                }
                // Roof level:
                else if(level == amountOfLevels) {
                    piece = Instantiate(roofPrefab, transform.position, transform.rotation) as Piece;
                    name = "Roof";
                }
                // Balcony:
                else if (levelsWithBalconies.Contains(level)) {
                    piece = Instantiate(FindNextPiece(balconyPrefabs, previousPiece), transform.position, transform.rotation) as Piece;
                    name = "Balcony";
                }
                // Windows:
                else {
                    piece = Instantiate(windowPrefab, transform.position, transform.rotation) as Piece;
                    name = "Window";
                }
                piece.transform.parent = transform;
                piece.transform.Translate(translationX + translationY);
                piece.name = name + column + "Level" + level;
                previousPiece = piece;
            }
            currentHigh += metricY;
        }
	}
	
	private Piece FindNextPiece(List<Piece> _nextPieces, Piece _previousPiece = null) {
        List<Piece> authorizedPieces = new List<Piece>();

        if (_nextPieces == null || _nextPieces.Count == 0) {
            Debug.LogError(GetType().Name + ".FindNextPiece: _nextPieces is null or empty.");
            return null;
        }

        // If there is not previous piece, it's free for all:
        if (_previousPiece == null) {
            int random = Random.Range(0, _nextPieces.Count);
            return _nextPieces[random];
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
