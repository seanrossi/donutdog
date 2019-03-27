using GameJolt.API;
using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class warning : MonoBehaviour {

    public InputField name;

    // Use this for initialization
    void Start()
    {

        //GameJolt.API.Objects.User user = GameJoltAPI.Instance.CurrentUser;
        //Debug.Log("User: " + user.Name);
        if (GameJoltAPI.Instance.HasSignedInUser || globals.hasWarned)
        {
            Debug.Log("User is signed in, this message shouldn't show");
            gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 9;
            if( Social.localUser.authenticated )
            {
                transform.GetChild(0).GetComponent<TextMesh>().text = "Google sign in successful: \r\n" + ((PlayGamesLocalUser)Social.localUser).id;
            }
            Debug.Log("Auto login not working");

        }
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnMouseDown()
    {
        globals.name = name.text.ToString();
        if( globals.name != "" )
            globals.name = globals.name.Substring(0, 10);
        globals.hasWarned = true;
        gameObject.SetActive(false);
    }
}
