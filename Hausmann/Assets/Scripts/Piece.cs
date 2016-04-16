using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// A piece of a building.
// Pieces obey to constraints.
public class Piece : MonoBehaviour {

    public enum Constraint {
        None = 0,
        Bottom,
        Top,
        NumberOfConstraints
    }

    public List<Constraint> constraints = new List<Constraint>();
    public int level;
    public int column;
    public List<Material> materials = new List<Material>();

    public void Start() {
        GetComponent<Renderer>().material = materials[Random.Range(0, materials.Count)];
    }

    // Checks if a list of constraints match with this object's constraints
    public bool IsMatching(List<Constraint> _constraintsToCompare) {
        bool isMatching = true;
        foreach(Constraint constraintToCompare in _constraintsToCompare) {
            foreach(Constraint constraint in constraints) {
                isMatching = IsMatching(constraint, constraintToCompare);
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

    // Checks if two constraints match together
    private bool IsMatching(Constraint _constraint1, Constraint _constraint2) {
        bool isMatching = true;
        // Switch of hell
        switch(_constraint1) {
            // Bottom:
            case Constraint.Bottom:
                switch(_constraint2) {
                    case Constraint.Bottom:
                        isMatching = false;
                        break;
                }
                break;

            // Top:
            case Constraint.Top:
                switch (_constraint2) {
                    case Constraint.Top:
                        isMatching = false;
                        break;
                }
                break;
            default:
                break;
        }

        return isMatching;
    }


}
