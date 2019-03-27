using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scoreButton : MonoBehaviour {

    public GameObject grid;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        Debug.Log("Attempting to prompt score");
        grid.GetComponent<Grid_Map>().submitAndShowScore();

        GetComponent<BoxCollider2D>().enabled = false;
    }
}
