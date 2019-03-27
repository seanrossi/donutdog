using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class credits : MonoBehaviour {

    const float MAX_TIMER = 3f;
    float timer = 0f;

    private void Awake()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestIdToken()
            .Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        {
            Social.localUser.Authenticate((bool success) => {
                if (success)
                {   
                    globals.isSignedInGoogle = true;
                }
            });
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if( timer >= MAX_TIMER )
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.transform.parent = null;
            gameObject.SetActive(false);
        }
	}
}
