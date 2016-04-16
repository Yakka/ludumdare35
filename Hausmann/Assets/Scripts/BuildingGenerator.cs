using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingGenerator : MonoBehaviour {
    
    public int numberOfLevels;
    public int numberOfRows;

    public GameObject windowPrefab; // TEMP

	// Use this for initialization
	void Start () {
        Vector3 initialPosition = transform.position;
	    for(int level = 0; level < numberOfLevels; level++) {
            for(int column = 0; column < numberOfRows; column++) {
                GameObject window = Instantiate(windowPrefab, transform.position, transform.rotation) as GameObject;
                window.transform.parent = transform;
                window.transform.Translate(column * Utiles.METRIC_X, 0, level * Utiles.METRIC_Y);
                window.name = "WindowColumn" + column + "Level" + level;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
