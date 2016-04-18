using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// A piece of a building.
// Pieces obey to constraints.
public class Piece : MonoBehaviour {
    [System.Serializable]
    public class Textures {
        public Sprite mainTexture = null;
        public List<Sprite> plantTextures = new List<Sprite>();
    }

    public enum Constraint {
        None = 0,
        Balcony,
        Balcony1,
        Balcony2,
        Background,
        Plantable,
        Animal,
        Window,
        WindowGrid,
        AmountOfConstraints
    }

    public List<Constraint> constraints = new List<Constraint>();
    public List<Textures> textures = new List<Textures>();
    public bool isRoof = false;
    private int textureIndex = 0;
    [HideInInspector]
    public int level = 0;
    private float probabilitySwitchBackground = 1f;
    private float changeTextureCooldown;
    private const float CHANGE_TEXTURE_COOLDOWN = 20f;

    public void Awake() {
        textureIndex = Random.Range(0, textures.Count);
        SetTexture(textureIndex);
        if (constraints.Count == 0) {
            constraints.Add(Constraint.None);
        }
        changeTextureCooldown = Random.Range(0f, CHANGE_TEXTURE_COOLDOWN);
        // TODO: Random offset in the animation
    }

    public void SetTexture(int _index) {
        if(_index < textures.Count) {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.sprite = textures[_index].mainTexture;
            textureIndex = _index;
        } else {
            Debug.LogError("Piece.SetTexture: _index is out of range of textures.");
        }
    }

    // Checks if a list of constraints match with this object's constraints
    public bool IsMatching(List<Constraint> _constraintsToCompare) {
        bool isMatching = true;
        foreach(Constraint constraintToCompare in _constraintsToCompare) {
            foreach(Constraint constraint in constraints) {
                isMatching = IsMatching(constraint, constraintToCompare) & IsMatching(constraintToCompare, constraint);
                if(!isMatching) {
                    break;
                }
            }
            if (!isMatching) {
                break;
            }
        }

        return isMatching;
    }

    public bool IsMatching(Piece _pieceToCompare) {
        return IsMatching(_pieceToCompare.constraints);
    }

    // Checks if two constraints match together
    private bool IsMatching(Constraint _constraint1, Constraint _constraint2) {
        bool isMatching = true;
        // Switch of hell
        switch(_constraint1) {
            // Balcony1:
            case Constraint.Balcony1:
                if(_constraint2 == Constraint.Balcony2) {
                    isMatching = false;
                }
                break;
            // Balcony2:
            case Constraint.Balcony2:
                if (_constraint2 == Constraint.Balcony1) {
                    isMatching = false;
                }
                break;
            // None:
            case Constraint.None:
                break;
            default:
                break;
        }

        return isMatching;
    }

    public void Update() {
        if(constraints.Contains(Constraint.Background)) {
            if(Random.Range(0f, 1f) < probabilitySwitchBackground * Time.deltaTime && changeTextureCooldown <= 0) {
                changeTextureCooldown = CHANGE_TEXTURE_COOLDOWN;
                textureIndex = textureIndex == 0 ? 1 : 0; // Swap
                SetTexture(textureIndex);
            } else {
                changeTextureCooldown -= Time.deltaTime;
            }
        }
    }

    public void AddPlants() {
        if (textures[textureIndex].plantTextures.Count != 0 && constraints.Contains(Constraint.Plantable)) {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.sprite = textures[textureIndex].plantTextures[Random.Range(0, textures[textureIndex].plantTextures.Count)];
        }
    }

    public void RemovePlants() {
        if (constraints.Contains(Constraint.Plantable)) {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.sprite = textures[textureIndex].mainTexture;
        }
    }

    public void AddCat(Piece _catPrefab) {
        Piece cat = Instantiate(_catPrefab, transform.position, transform.rotation) as Piece;
        SpriteRenderer renderer = cat.GetComponent<SpriteRenderer>();
        renderer.sprite = cat.textures[0].mainTexture;
        cat.transform.parent = transform;
        cat.SetTexture(0);
        cat.name = "Cat";
    }
}
