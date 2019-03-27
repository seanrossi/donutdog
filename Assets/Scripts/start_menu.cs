using GameJolt.API;
using GameJolt.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start_menu : MonoBehaviour {

    //UI ELEMENTS
    public TextMesh starCount;
    public TextMesh wages;
    public TextMesh rate;
    public TextMesh rent;

    //MONTH STATS
    public int rentDue;

    private void Awake()
    {
        
    }
    // Use this for initialization
    void Start () {
        //starCount.text = globals.keys.ToString();
        wages.text = "$" + globals.wages.ToString();
        rate.text = "$" + globals.rate.ToString() + "/day";
        if( globals.rentsPaid[globals.currentMonth] )
        {
            rentDue = 0;
            rent.text = "Paid";
        }
        else
        {
            globals.currentRent = rentDue;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
