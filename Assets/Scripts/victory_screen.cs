using GameJolt.API;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class victory_screen : MonoBehaviour {

    public Text base_score;
    public Text time_taken;
    public Text time_multiplied;
    public Text total_score;
    public Text wage;
    public Text tipText;
    bool isMobile;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SaveData()
    {
        if (GameJoltAPI.Instance.HasSignedInUser)
        {
            GameJolt.API.Objects.User user = GameJoltAPI.Instance.CurrentUser;
            GameData data = new GameData();
            data.keys = globals.keys;
            data.stars = globals.stars;
            //BinaryFormatter bf = new BinaryFormatter();
            //string dataString = "";
            //dataString = JsonUtility.ToJson(data);
            //bf.Serialize(dataString, data);
            string statsString = "";

            //CONVERT MISSION_STATS TO STRING 
            for (int i = 0; i < globals.mission_stats.Count; i++)
            {
                statsString += globals.mission_stats[i].rank.ToString();
            }

            GameJolt.API.DataStore.Set("mStatss", statsString, false, null);
            GameJolt.API.DataStore.Set("keysss", globals.keys.ToString(), false, null);
            GameJolt.API.DataStore.Set("wages", globals.wages.ToString(), false, null);
            GameJolt.API.DataStore.Set("rate", globals.rate.ToString(), false, null);
            //GameJolt.API.DataStore.Set("stars", globals.stars.ToString(), false, null);
        }
    }

    public void SaveData(GameJolt.API.Objects.User user)
    {
        GameData data = new GameData();
        data.keys = globals.keys;
        data.stars = globals.stars;
        //BinaryFormatter bf = new BinaryFormatter();
        //string dataString = "";
        //dataString = JsonUtility.ToJson(data);
        //bf.Serialize(dataString, data);

        string statsString = "";

        //CONVERT MISSION_STATS TO STRING 
        for( int i = 0; i < globals.mission_stats.Count; i++ )
        {
            statsString += globals.mission_stats[i].rank.ToString();
        }

        GameJolt.API.DataStore.Set("mStatss", statsString, false, null);
        GameJolt.API.DataStore.Set("keysss", globals.keys.ToString(), false, null);
        GameJolt.API.DataStore.Set("wages", globals.wages.ToString(), false, null);
        GameJolt.API.DataStore.Set("rate", globals.rate.ToString(), false, null);
        //GameJolt.API.DataStore.Set("stars", globals.stars.ToString(), false, null);

    }

    private void SubmitScore(int score, GameJolt.API.Objects.User user, int table_id)
    {
        GameJolt.API.Scores.Add(score, score.ToString(), table_id, "", (bool success) => {
            Debug.Log(string.Format("Score Add {0}.", success ? "Successful" : "Failed"));
        });
    }

    private void SubmitGuestScore(int score, int table_id)
    {
        GameJolt.API.Scores.Add(score, score.ToString(), globals.name, table_id, "", (bool success) => {
            Debug.Log(string.Format("Score Add {0}.", success ? "Successful" : "Failed"));
        });
        
    }

    private void OnDataSaved(bool success)
    {
        //SceneManager.LoadScene("month_01_menu");
    }

    public void SetStats( int bs, float tt, int ts, int tid, bool mobile, int tip )
    {
        tipText.text = "$" + tip.ToString();
        wage.text = "$" + globals.rate.ToString();
        base_score.text = bs.ToString();
        time_taken.text = ((int)tt).ToString();
        time_multiplied.text = ((int)tt * 100).ToString();
        total_score.text = ts.ToString();
        isMobile = mobile;
        if( isMobile )
        {
            globals.SaveLocalData();
        }
        if (GameJoltAPI.Instance.HasSignedInUser)
        {
            GameJolt.API.Objects.User user = GameJoltAPI.Instance.CurrentUser;
            SaveData(user);
            SubmitScore(ts, user, tid);
        }
        else
        {
            if( globals.name != "" )
                SubmitGuestScore(ts, tid);
        }
    }

    public void SetStatsPuzzle()
    {
        wage.text = "$" + globals.rate.ToString();
    }

    public void SetStatsSurvival(int bs, int unhappy, int ts, int tid, bool mobile, int tip)
    {
        tipText.text = "$" + tip.ToString();
        wage.text = "$" + globals.rate.ToString();
        base_score.text = bs.ToString();
        time_taken.text = (unhappy).ToString();
        time_multiplied.text = (unhappy * -500).ToString();
        total_score.text = ts.ToString();
        isMobile = mobile;
        if (isMobile)
        {
            globals.SaveLocalData();
        }
        if (GameJoltAPI.Instance.HasSignedInUser)
        {
            GameJolt.API.Objects.User user = GameJoltAPI.Instance.CurrentUser;
            SaveData(user);
            SubmitScore(ts, user, tid);
        }
        else
        {
            if (globals.name != "")
                SubmitGuestScore(ts, tid);
        }
    }

    public void SetStatsBoss(int leftover, float tt, int tid, bool mobile, int tip)
    {
        tipText.text = "$" + tip.ToString();
        wage.text = "$" + globals.rate.ToString();
        base_score.text = (leftover * 500).ToString();
        time_taken.text = ((int)tt).ToString();
        time_multiplied.text = ((int)tt * 100).ToString();
        int ts = (leftover * 500) + ((int)tt * 100);
        total_score.text = ts.ToString();
        isMobile = mobile;
        if (isMobile)
        {
            globals.SaveLocalData();
        }
        if (GameJoltAPI.Instance.HasSignedInUser)
        {
            GameJolt.API.Objects.User user = GameJoltAPI.Instance.CurrentUser;
            SaveData(user);
            SubmitScore(ts, user, tid);
        }
        else
        {
            if (globals.name != "")
                SubmitGuestScore(ts, tid);
        }
    }

    public void submitting()
    {
        GameObject panel = transform.GetChild(0).transform.GetChild(0).gameObject;
        int panelChildren = panel.transform.childCount;
        for (int i = 0; i < panelChildren; i++)
        {
            panel.transform.GetChild(i).gameObject.SetActive(false);
        }

        GameObject submitting = new GameObject("submitting");
        submitting.transform.SetParent(panel.transform);
        Text submittingText = submitting.AddComponent<Text>();
        Vector3 tempPos;
        if (isMobile)
        {
            tempPos = new Vector3(-1500f, 1000f, 1f);
            submitting.transform.localScale = new Vector3(3f, 3f, 3f);
            submittingText.fontSize = 164;
        }
        else
        {
            tempPos = new Vector3(-25f, 0f, 1f);
            submitting.transform.localScale = new Vector3(1f, 1f, 1f);
            submittingText.fontSize = 32;
        }
        submitting.transform.localPosition = tempPos;
        //submitting.transform.localScale = new Vector3(3f, 3f, 3f);

        submittingText.text = "Fetching Scores";
        //submittingText.fontSize = 164;
        submittingText.horizontalOverflow = HorizontalWrapMode.Overflow;
        submittingText.verticalOverflow = VerticalWrapMode.Overflow;
        submittingText.font = (Font)Resources.Load("TruenoBlkOl");
    }

    public void showScores( GameJolt.API.Objects.Score[] scores )
    {
        //Debug.Log("Score[0]: " + scores[0]);
        //Debug.Log("Score[1]: " + scores[1]);

        //CLEAR CURRENT CANVAS
        GameObject panel = transform.GetChild(0).transform.GetChild(0).gameObject;
        int panelChildren = panel.transform.childCount;
        for (int i = 0; i < panelChildren; i++)
        {
            panel.transform.GetChild(i).gameObject.SetActive(false);
        }

        GameObject submitting = new GameObject("submitting");
        submitting.transform.SetParent(panel.transform);
        Text submittingText = submitting.AddComponent<Text>();
        
        Vector3 tempPos = new Vector3(-700f, 1800f, 1f);
        submitting.transform.localPosition = tempPos;
        submitting.transform.localScale = new Vector3(3f, 3f, 3f);

        submittingText.text = "TOP 5";
        submittingText.fontSize = 164;
        submittingText.horizontalOverflow = HorizontalWrapMode.Overflow;
        submittingText.verticalOverflow = VerticalWrapMode.Overflow;
        submittingText.font = (Font)Resources.Load("TruenoBlkOl");

        int hardLimit = 5;
        for( int i = 0; i < scores.Length; i++ )
        {
            GameObject score0Name = new GameObject("score0Name");
            score0Name.transform.SetParent(transform.GetChild(0));
            GameObject score0Score = new GameObject("score0Name");
            score0Score.transform.SetParent(transform.GetChild(0));

            Text score0NameText = score0Name.AddComponent<Text>();
            //float yPos = 400 - (100 * i );
            if( isMobile )
                tempPos = new Vector3(-350f, 400 - (100 * i ), 1f);
            else
                tempPos = new Vector3(-25f, 150 - (50 * i), 1f);
            score0Name.transform.localPosition = tempPos;
            score0Name.transform.localScale = new Vector3(0.25f, 0.25f, 1f);

            score0NameText.text = "" + (i + 1) + ": " + scores[i].PlayerName;
            score0NameText.fontSize = 128;
            score0NameText.horizontalOverflow = HorizontalWrapMode.Overflow;
            score0NameText.verticalOverflow = VerticalWrapMode.Overflow;
            score0NameText.font = (Font)Resources.Load("TruenoBlkOl");

            Text score0scoreText = score0Score.AddComponent<Text>();
            if( isMobile )
                tempPos = new Vector3(100f, 400 - (100 * i), 1f);
            else
                tempPos = new Vector3(250f, 150 - (50 * i), 1f);
            score0Score.transform.localPosition = tempPos;
            score0Score.transform.localScale = new Vector3(0.25f, 0.25f, 1f);

            score0scoreText.text = scores[i].Value.ToString();
            score0scoreText.fontSize = 128;
            score0scoreText.horizontalOverflow = HorizontalWrapMode.Overflow;
            score0scoreText.verticalOverflow = VerticalWrapMode.Overflow;
            score0scoreText.font = (Font)Resources.Load("TruenoBlkOl");

            //If max limit is reached, break from loop
            if (i == hardLimit - 1)
                break;
        }
        GameObject score1Name = new GameObject("score0Name");
        score1Name.transform.SetParent(transform.GetChild(0));
        GameObject score1Score = new GameObject("score0Name");
        score1Score.transform.SetParent(transform.GetChild(0));

        Text score1NameText = score1Name.AddComponent<Text>();
        //float yPos = 400 - (100 * i );
        if (isMobile)
            tempPos = new Vector3(-350f, 400 - (100 * 5), 1f);
        else
            tempPos = new Vector3(-25f, 150 - (50 * 5), 1f);
        score1Name.transform.localPosition = tempPos;
        score1Name.transform.localScale = new Vector3(0.25f, 0.25f, 1f);

        score1NameText.text = "Your score: ";
        score1NameText.fontSize = 128;
        score1NameText.horizontalOverflow = HorizontalWrapMode.Overflow;
        score1NameText.verticalOverflow = VerticalWrapMode.Overflow;
        score1NameText.font = (Font)Resources.Load("TruenoBlkOl");

        Text score1scoreText = score1Score.AddComponent<Text>();
        if (isMobile)
            tempPos = new Vector3(100f, 400 - (100 * 5), 1f);
        else
            tempPos = new Vector3(250f, 150 - (50 * 5), 1f);
        score1Score.transform.localPosition = tempPos;
        score1Score.transform.localScale = new Vector3(0.25f, 0.25f, 1f);

        score1scoreText.text = total_score.text;
        score1scoreText.fontSize = 128;
        score1scoreText.horizontalOverflow = HorizontalWrapMode.Overflow;
        score1scoreText.verticalOverflow = VerticalWrapMode.Overflow;
        score1scoreText.font = (Font)Resources.Load("TruenoBlkOl");

    }
}
