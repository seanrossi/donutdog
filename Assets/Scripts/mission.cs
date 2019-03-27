using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mission : MonoBehaviour {

    //LIST OF CONDITIONS: 
    //0 - ENDLESS
    //1 - SATISFY NUMBER OF CUSTOMERS
    //2 - SURVIVE FOR TIME LIMIT
    //3 - CLEAR ALL DONUTS
    //4 - REMOVE STALE AND/OR MOLDY DONUTS
    //5 - TIME TRIAL WITH OBSTACLES
    //6 - SURVIVAL WITH OBSTACLES
    //5 - BUILD STREAK
    //8 - BOSS STAGE
    public int condition;

    //Number of customers required to satisfy to pass mission, or number of streak needed
    public int target_customers;

    //0 - No Limit
    public int time_limit;
    public int sad_limit;
    public int mission_id;
    public float target_time;
    public int target_score;
    public int target_moves;
    public int tip;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
