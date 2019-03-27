using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class calendar_page : MonoBehaviour {

    public string mission;
    public int mission_id;
    public int keysRequired, starsRequired;
    bool locked = true;

	// Use this for initialization
	void Start () {
        //foreach( Transform child in transform )
        //{
        //child.GetComponent<MeshRenderer>().sortingOrder = 2;
        //}
        if( !globals.mission_stats.ContainsKey(mission_id) )
        {  
            mission_status currentMission;
            currentMission.mission_id = mission_id;
            currentMission.score = 0;
            currentMission.time = 0;
            currentMission.rank = 0;
            globals.mission_stats[currentMission.mission_id] = currentMission;
        }
        Debug.Log("Keys found: " + globals.keys.ToString());
        if (globals.keys >= keysRequired && globals.stars >= starsRequired)
            locked = false;
        else
            GetComponent<Animator>().SetBool("isLocked", true);    

        if( globals.mission_stats[mission_id].rank > 0 )
        {
            int rank = globals.mission_stats[mission_id].rank;
            if (rank > 1)
                transform.GetChild(0).GetComponent<Animator>().SetBool("perfect", true);
            else if (rank > 0)
                transform.GetChild(0).GetComponent<Animator>().SetBool("passed", true);
            else
                transform.GetChild(0).GetComponent<Animator>().SetBool("attempted", true);

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        if (!locked)
        {
            transform.GetComponent<Animator>().SetBool("isSelected", true);
            //globals.setStatus(mission_id, 0, 0, 0);
            SceneManager.LoadScene(mission);
        }
    }
}
