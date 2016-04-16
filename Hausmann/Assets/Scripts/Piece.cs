using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// A piece of a building.
// Pieces obey to constraints.
public class Piece : MonoBehaviour {

    public enum Constraint {
        None = 0,
        Balcony1,
        Balcony2,
        NumberOfConstraints
    }

    public List<Constraint> constraints = new List<Constraint>();
    public int level;
    public int column;
    public List<Material> materials = new List<Material>();

    public void Start() {
        GetComponent<Renderer>().material = materials[Random.Range(0, materials.Count)];

        if(constraints.Count == 0) {
            constraints.Add(Constraint.None);
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
                if(_constraint2 != Constraint.Balcony1) {
                    isMatching = false;
                }
                break;
            // Balcony2:
            case Constraint.Balcony2:
                if (_constraint2 != Constraint.Balcony2) {
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


}
