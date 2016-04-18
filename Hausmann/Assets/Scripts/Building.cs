using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour {

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
    public List<Piece> catPrefabs = new List<Piece>();
    public List<Piece> backgroundPrefabs = new List<Piece>();
    public List<Piece> groundBackgroundPrefabs = new List<Piece>();
    public List<Piece> roofBackgroundPrefabs = new List<Piece>();

    private int roofLevel;
    private bool hasPlants = false;
    private bool hasCats = false;

    void Start () {
        BuildTheBuilding();

    }

    public void BuildTheBuilding() {
        DestroyEverything();
        hasPlants = false;
        amountOfLevels = Random.Range(1,7);
        roofLevel = amountOfLevels;
        for (int level = 0; level <= amountOfLevels; level++) {
            GenerateLevel(level);
        }
    }

    // Generate the next piece according to its constraints:
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

    // Generate a level:
    private void GenerateLevel(int _levelIndex) {
        float high = (_levelIndex == 0 ? 0 : 1) * Utiles.METRIC_LARGE_Y + (_levelIndex == 0 ? 0 : _levelIndex - 1) * Utiles.METRIC_Y;
        int doorColumnIndex = Random.Range(0, amountOfColumns);
        Vector3 translationY;
        Vector3 translationX;
        Piece previousPiece = null;
        translationY = Vector3.up * high;

        // Corners:
        if (_levelIndex > 0 && _levelIndex < amountOfLevels) {
            Piece cornerLeft = Instantiate(cornerLeftPrefab, transform.position, transform.rotation) as Piece;
            cornerLeft.transform.parent = transform;
            cornerLeft.transform.Translate(translationY);
            cornerLeft.transform.Translate(-Utiles.METRIC_X, 0, 0);
            cornerLeft.name = "CornerLeftLevel" + _levelIndex;
            cornerLeft.level = _levelIndex;

            Piece cornerRight = Instantiate(cornerRightPrefab, transform.position, transform.rotation) as Piece;
            cornerRight.transform.parent = transform;
            cornerRight.transform.Translate(translationY);
            cornerRight.transform.Translate(amountOfColumns * Utiles.METRIC_X, 0, 0);
            cornerRight.name = "CornerRightLevel" + _levelIndex;
            cornerRight.level = _levelIndex;
        }
        // Main building:
        for (int column = 0; column < amountOfColumns; column++) {
            translationX = Vector3.right * column * Utiles.METRIC_X;
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
            piece.level = _levelIndex;
            previousPiece = piece;
            // Background:
            if (_levelIndex == 0) {
                piece = Instantiate(groundBackgroundPrefabs[Random.Range(0, groundBackgroundPrefabs.Count)], previousPiece.transform.position, previousPiece.transform.rotation) as Piece;
                piece.name = previousPiece.name + "GroundBackground";
            }
            else if (_levelIndex == amountOfLevels) {
                piece = Instantiate(roofBackgroundPrefabs[Random.Range(0, roofBackgroundPrefabs.Count)], previousPiece.transform.position, previousPiece.transform.rotation) as Piece;
                piece.name = previousPiece.name + "RoofBackground";
            }
            else {
                piece = Instantiate(backgroundPrefabs[Random.Range(0, backgroundPrefabs.Count)], previousPiece.transform.position, previousPiece.transform.rotation) as Piece;
                piece.name = previousPiece.name + "Background";
            }
            piece.transform.parent = previousPiece.transform;
            piece.SetTexture(0);
        }

    }

    public void DeleteLevel(int _levelIndex) {
        // We get all the children:
        Piece[] everyPieces = GetComponentsInChildren<Piece>();

        // We delete the pieces from the list which are at the targeted level:
        List<Piece> piecesToDelete = new List<Piece>(everyPieces);
        foreach (Piece piece in piecesToDelete) {
            if(piece.level == _levelIndex && !piece.isRoof) {
                Destroy(piece.gameObject);
            }
        }
        amountOfLevels--;
    }

    public void MoveRoofToLevel(int _levelIndex) {
        Vector3 translationY = Vector3.up * Utiles.METRIC_Y;
        int oldRoofLevel = roofLevel;
        roofLevel = _levelIndex;
        amountOfLevels = roofLevel;
        if (oldRoofLevel > _levelIndex) {
            DeleteLevel(_levelIndex);
        } else {
            for(int i = oldRoofLevel; i < _levelIndex; i++) {
                GenerateLevel(i);
            }
        }
        // We get all the children:
        Piece[] everyPieces = GetComponentsInChildren<Piece>();

        // We move the pieces from the list which are at the targeted level:
        List<Piece> piecesToMove = new List<Piece>(everyPieces);
        foreach (Piece piece in piecesToMove) {
            if (piece.isRoof) {
                piece.transform.Translate(translationY * (_levelIndex - piece.level));
                piece.level = _levelIndex;
            }
        }
    }

    public void AddPlants() {
        Piece[] pieces = GetComponentsInChildren<Piece>();
        foreach(Piece piece in pieces) {
            piece.AddPlants();
        }
    }

    public void RemovePlants() {
        Piece[] pieces = GetComponentsInChildren<Piece>();
        foreach (Piece piece in pieces) {
            piece.RemovePlants();
        }
    }

    public void DestroyEverything() {
        Piece[] pieces = GetComponentsInChildren<Piece>();
        foreach (Piece piece in pieces) {
            Destroy(piece.gameObject);
        }
    }

    public void SwitchPlants() {
        if (hasPlants) {
            RemovePlants();
        }
        else {
            AddPlants();
        }
        hasPlants = !hasPlants;
    }

    public void AddCats() {
        Piece[] pieces = GetComponentsInChildren<Piece>();
        foreach (Piece piece in pieces) {
            if(Random.Range(0, 10) >= 5)
                piece.AddCat(catPrefabs[Random.Range(0, catPrefabs.Count)]);
        }
    }

    public void RemoveCats() {
        Piece[] pieces = GetComponentsInChildren<Piece>();
        foreach (Piece piece in pieces) {
            if(piece.name == "Cat") {
                Destroy(piece.gameObject);
            }
        }
    }

    public void SwitchCats() {
        if (hasCats) {
            RemoveCats();
        }
        else {
            AddCats();
        }
        hasCats = !hasCats;
    }

}
