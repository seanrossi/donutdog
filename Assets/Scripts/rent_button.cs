using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rent_button : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        if( globals.rentsPaid[globals.currentMonth] )
        {
            return;
        }
        else if( globals.wages >= globals.currentRent )
        {
            globals.rentsPaid[globals.currentMonth] = true;
            Camera.main.GetComponent<start_menu>().rent.text = "Rent Paid";
            globals.wages -= globals.currentRent;
        }
    }
}
