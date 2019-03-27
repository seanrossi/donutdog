using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scene_button : MonoBehaviour {

    public string scene_name;
    public bool init;
    public bool mobile;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        //if (globals.isPaused)
            //return;
        if( init )
        {
            globals.isMobile = mobile;
            globals.init();
        }
        GetComponent<Animator>().SetBool("selected", true);
        StartCoroutine(LoadMenu());
    }

    IEnumerator LoadMenu()
    {
        yield return new WaitForSecondsRealtime(2);
        globals.lastScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene_name);
    }
}
