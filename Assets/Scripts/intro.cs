using GameJolt.API;
using GameJolt.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class intro : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //if (!GameJoltAPI.Instance.HasSignedInUser)
        {
            //globals.isPaused = true;
            //GameJoltUI.Instance.ShowSignIn();
        }
    }
	
	// Update is called once per frame
	void Update () {
		//if(GameJoltAPI.Instance.HasSignedInUser)
        {
            //globals.isPaused = false;
        }
	}
}
