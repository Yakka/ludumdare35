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
    public Piece backgroundPrefab;
    public Piece groundBackgroundPrefab;
    public Piece roofBackgroundPrefab;

    void Start () {
        
        float currentHigh = 0;
        for (int level = 0; level <= amountOfLevels; level++) {
            GenerateLevel(level, currentHigh);
            float metricY = level == 0 ? Utiles.METRIC_LARGE_Y : Utiles.METRIC_Y; // The level 0 is higher than other levels
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

    // Generate a level
    private void GenerateLevel(int _levelIndex, float _high) {
        int doorColumnIndex = Random.Range(0, amountOfColumns + 1);
        Vector3 translationY;
        Vector3 translationX;
        Vector3 translationZ = Vector3.down; ; // Only for the backgrounds.
        Piece previousPiece = null;
        translationY = Vector3.back * _high;

        // Corners:
        if (_levelIndex > 0 && _levelIndex < amountOfLevels) {
            Piece cornerLeft = Instantiate(cornerLeftPrefab, transform.position, transform.rotation) as Piece;
            cornerLeft.transform.parent = transform;
            cornerLeft.transform.Translate(translationY);
            cornerLeft.transform.Translate(Utiles.METRIC_X, 0, 0);
            cornerLeft.name = "CornerLeftLevel" + _levelIndex;

            Piece cornerRight = Instantiate(cornerRightPrefab, transform.position, transform.rotation) as Piece;
            cornerRight.transform.parent = transform;
            cornerRight.transform.Translate(translationY);
            cornerRight.transform.Translate(-(amountOfColumns + 1) * Utiles.METRIC_X, 0, 0);
            cornerRight.name = "CornerRightLevel" + _levelIndex;
        }
        // Main building:
        for (int column = 0; column <= amountOfColumns; column++) {
            translationX = Vector3.left * column * Utiles.METRIC_X;
            string name = string.Empty;
            Piece piece = null;
            // Ground level:
            if (_levelIndex == 0) {
                // Door:
                if (column == doorColumnIndex) {
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
            else if (_levelIndex == amountOfLevels) {
                piece = Instantiate(roofPrefab, transform.position, transform.rotation) as Piece;
                name = "Roof";
            }
            // Balcony:
            else if (levelsWithBalconies.Contains(_levelIndex)) {
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
            piece.name = name + column + "Level" + _levelIndex;
            previousPiece = piece;
            // Background:
            if (_levelIndex == 0) {
                piece = Instantiate(groundBackgroundPrefab, previousPiece.transform.position, previousPiece.transform.rotation) as Piece;
                piece.name = previousPiece.name + "GroundBackground";
            }
            else if (_levelIndex == amountOfLevels) {
                piece = Instantiate(roofBackgroundPrefab, previousPiece.transform.position, previousPiece.transform.rotation) as Piece;
                piece.name = previousPiece.name + "RoofBackground";
            }
            else {
                piece = Instantiate(backgroundPrefab, previousPiece.transform.position, previousPiece.transform.rotation) as Piece;
                piece.name = previousPiece.name + "Background";
            }
            piece.transform.parent = previousPiece.transform;
            piece.transform.Translate(translationZ);
        }

    }
}
