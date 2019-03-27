using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class retry_button : MonoBehaviour {

    public GameObject grid;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        grid.GetComponent<Grid_Map>().restart();
    }
}
