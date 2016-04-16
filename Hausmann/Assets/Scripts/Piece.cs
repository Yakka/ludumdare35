using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// A piece of a building.
// Pieces obey to constraints.
public class Piece {

    public enum Constraint {
        None = 0,
        NumberOfConstraints
    }

    public List<Constraint> constraints = new List<Constraint>();

    public GameObject plane; // TODO: the displayed plane

    public Piece(List<Constraint> _constraints) {
        foreach(Constraint constraint in _constraints) {
            constraints.Add(constraint);
        }

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
        return true; // TODO
    }


}
